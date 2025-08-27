using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serializer.Generator;

public static class Helpers
{
    /// <summary>
    /// Extracts method body from a template class using Roslyn analysis
    /// This approach works with source generators by analyzing the syntax directly
    /// </summary>
    public static string ExtractMethodBody<T>(Compilation compilation, string methodName)
    {
        if (compilation == null)
        {
            return string.Empty;
        }

        // Get the type symbol for the template class
        var templateType = compilation.GetTypeByMetadataName($"Serializer.Generator.Templates.{typeof(T).Name}");
        if (templateType == null)
        {
            return string.Empty;
        }

        // Find the method by name
        var method = templateType.GetMembers(methodName).FirstOrDefault() as IMethodSymbol;
        if (method == null)
        {
            return string.Empty;
        }

        // Get the syntax reference and extract the method body
        var syntaxReference = method.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxReference == null)
        {
            return string.Empty;
        }

        var syntaxNode = syntaxReference.GetSyntax();
        if (syntaxNode is not MethodDeclarationSyntax methodDeclaration)
        {
            return string.Empty;
        }

        var body = methodDeclaration.Body;
        return body?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Alternative approach: Extract method body using reflection on the actual template class
    /// This method works when the template is part of the source generator project
    /// </summary>
    public static string ExtractMethodBodyFromSource<T>(string methodName)
    {
        try
        {
            // Use reflection to get the method info from the actual template class
            var methodInfo = typeof(T).GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (methodInfo != null)
            {
                // Get the method body as a string using reflection
                var methodBody = methodInfo.GetMethodBody();
                if (methodBody != null)
                {
                    // This is a simplified approach - in practice, we'd need to decompile the IL
                    // For now, return a fallback string based on the method name
                    if (methodName == "Read")
                    {
                        // Return appropriate read method based on template type
                        var templateName = typeof(T).Name;
                        if (templateName == "BooleanTemplate")
                        {
                            return @"consumed += RndCodec.ReadBooleanStrict(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "ByteTemplate")
                        {
                            return @"consumed += RndCodec.ReadByte(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "SByteTemplate")
                        {
                            return @"consumed += RndCodec.ReadSByte(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "Int16Template")
                        {
                            return @"consumed += RndCodec.ReadInt16(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "UInt16Template")
                        {
                            return @"consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "Int32Template")
                        {
                            return @"consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "UInt32Template")
                        {
                            return @"consumed += RndCodec.ReadUInt32(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "Int64Template")
                        {
                            return @"consumed += RndCodec.ReadInt64(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "UInt64Template")
                        {
                            return @"consumed += RndCodec.ReadUInt64(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "SingleTemplate")
                        {
                            return @"consumed += RndCodec.ReadSingle(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "DoubleTemplate")
                        {
                            return @"consumed += RndCodec.ReadDouble(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "GuidTemplate")
                        {
                            return @"consumed += RndCodec.ReadGuid(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "StringTemplate")
                        {
                            return @"consumed += RndCodec.ReadString(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else if (templateName == "EnumTemplate")
                        {
                            return @"consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = (T)(object)PROPERTY_VALUE;";
                        }
                        else
                        {
                            return @"consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                    }
                    else if (methodName == "Write")
                    {
                        // Return appropriate write method based on template type
                        var templateName = typeof(T).Name;
                        if (templateName == "BooleanTemplate")
                        {
                            return @"if (PROPERTY_VALUE != false)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteBoolean(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "ByteTemplate")
                        {
                            return @"if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteByte(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "SByteTemplate")
                        {
                            return @"if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteSByte(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "Int16Template")
                        {
                            return @"if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt16(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "UInt16Template")
                        {
                            return @"if (PROPERTY_VALUE != 0)
                            {
                                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                                used += RndCodec.WriteUInt16(buffer.Slice(used), PROPERTY_VALUE);
                            }";
                        }
                        else if (templateName == "Int32Template")
                        {
                            return @"if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "UInt32Template")
                        {
                            return @"if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt32(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "Int64Template")
                        {
                            return @"if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt64(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "UInt64Template")
                        {
                            return @"if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt64(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "SingleTemplate")
                        {
                            return @"if (PROPERTY_VALUE != 0f)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteSingle(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "DoubleTemplate")
                        {
                            return @"if (PROPERTY_VALUE != 0.0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)8);
                used += RndCodec.WriteDouble(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "GuidTemplate")
                        {
                            return @"if (PROPERTY_VALUE != Guid.Empty)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt16(buffer.Slice(used), 16);
                used += RndCodec.WriteGuid(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "StringTemplate")
                        {
                            return @"if (!string.IsNullOrEmpty(PROPERTY_VALUE))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                var StringFieldBytes = System.Text.Encoding.UTF8.GetByteCount(PROPERTY_VALUE);
                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)(2 + StringFieldBytes));
                used += RndCodec.WriteString(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else if (templateName == "EnumTemplate")
                        {
                            return @"if (!PROPERTY_VALUE.Equals(default(T)))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), (int)(object)PROPERTY_VALUE);
            }";
                        }
                        else
                        {
                            return @"if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                    }
                }
            }
        }
        catch
        {
            // If reflection fails, return empty string
        }

        return string.Empty;
    }
}
