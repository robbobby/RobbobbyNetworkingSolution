using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace Serializer.Generator.Templates
{
    /// <summary>
    /// Manages template extraction and variable substitution for code generation
    /// </summary>
    public static class TemplateManager
    {
        /// <summary>
        /// Extracts the method body from a template method and performs variable substitution
        /// </summary>
        /// <param name="templateType">The template class type</param>
        /// <param name="methodName">The method name to extract</param>
        /// <param name="replacements">Dictionary of placeholder names to replacement values</param>
        /// <returns>The substituted method body as a string</returns>
        public static string ExtractMethodBody(Type templateType, string methodName, Dictionary<string, string> replacements)
        {
            var method = templateType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            if (method == null)
            {
                throw new ArgumentException($"Method {methodName} not found in template type {templateType.Name}");
            }

            // Get the method body by decompiling the IL or using reflection
            // For now, we'll use a simpler approach with predefined templates
            var templateKey = $"{templateType.Name}.{methodName}";
            var template = GetTemplateString(templateKey);
            
            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentException($"Template not found for {templateKey}");
            }

            return SubstituteVariables(template, replacements);
        }

        /// <summary>
        /// Performs variable substitution in a template string
        /// </summary>
        private static string SubstituteVariables(string template, Dictionary<string, string> replacements)
        {
            var result = template;
            foreach (var replacement in replacements)
            {
                result = result.Replace(replacement.Key, replacement.Value);
            }
            return result;
        }

        /// <summary>
        /// Gets the template string for a given template key
        /// </summary>
        private static string GetTemplateString(string templateKey)
        {
            return templateKey switch
            {
                "Int32Template.Read" => @"consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var PropertyNameValue);
            readPacket.PropertyName = PropertyNameValue;",
                
                "Int32Template.Write" => @"if (!(value == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), value);
            }",
                
                "StringTemplate.Read" => @"var PropertyNameStart = consumed;
            consumed += RndCodec.ReadString(buffer.Slice(consumed), out var PropertyNameValue);
            readPacket.PropertyName = PropertyNameValue;
            // Verify we didn't read beyond the expected length
            if (consumed - PropertyNameStart > len) return false;",
                
                "StringTemplate.Write" => @"var PropertyNameBytes = System.Text.Encoding.UTF8.GetByteCount(PropertyName);
            used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)(2 + PropertyNameBytes)); // Length (2 for string length + string bytes)
            used += RndCodec.WriteString(buffer.Slice(used), PropertyName);",
                
                "NestedObjectTemplate.Read" => @"if (TargetType.TryRead(buffer.Slice(consumed, len), ref consumed, out var PropertyNameValue))
            {
                readPacket.PropertyName = PropertyNameValue;
            }
            consumed += len;",
                
                "NestedObjectTemplate.Write" => @"// Write nested object as length-prefixed payload
            var nestedBuffer = new byte[1024]; // Temporary buffer
            if (PropertyName!.Write(nestedBuffer, out var nestedLength))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)nestedLength);
                nestedBuffer.AsSpan(0, nestedLength).CopyTo(buffer.Slice(used));
                used += nestedLength;
            }",
                
                "ArrayTemplate.Read" => @"var arrayStart = consumed;
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var count);
            var PropertyNameList = new List<ElementType>();
            for (int i = 0; i < count; i++)
            {
                consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var itemLen);
                if (ElementType.TryRead(buffer.Slice(consumed, itemLen), ref consumed, out var item))
                {
                    PropertyNameList.Add(item);
                }
                consumed += itemLen;
            }
            readPacket.PropertyName = PropertyNameList.ToArray();",
                
                "ArrayTemplate.Write" => @"var PropertyNameTotalLength = 2; // Start with array count
            foreach (var item in PropertyName)
            {
                var itemBuffer = new byte[1024]; // Temporary buffer
                if (item.Write(itemBuffer, out var itemLength))
                {
                    PropertyNameTotalLength += 2 + itemLength; // Item length (2 bytes) + item data
                }
            }
            used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)PropertyNameTotalLength); // Total length prefix
            used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)PropertyName.Length); // Array count
            foreach (var item in PropertyName)
            {
                var itemBuffer = new byte[1024]; // Temporary buffer
                if (item.Write(itemBuffer, out var itemLength))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)itemLength);
                    itemBuffer.AsSpan(0, itemLength).CopyTo(buffer.Slice(used));
                    used += itemLength;
                }
            }",
                
                _ => string.Empty
            };
        }
    }
}
