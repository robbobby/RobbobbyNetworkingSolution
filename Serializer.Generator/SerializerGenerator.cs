using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serializer.Generator.Templates;

namespace Serializer.Generator
{
    /// <summary>
    /// Roslyn source generator for RNS serialization code following the specific wire format
    /// </summary>
    [Generator]
    public class SerializerGenerator : ISourceGenerator
    {
        /// <summary>
        /// Initializes the generator and registers for syntax notifications
        /// </summary>
        /// <param name="context">The generator initialization context</param>
        public void Initialize(GeneratorInitializationContext context)
        {
            // Register for syntax notifications to find partial types
            context.RegisterForSyntaxNotifications(() => new SerializableTypeSyntaxReceiver());
        }

        /// <summary>
        /// Executes the source generation process
        /// </summary>
        /// <param name="context">The generator execution context</param>
        public void Execute(GeneratorExecutionContext context)
        {
            // Get the syntax receiver from the context
            if (context.SyntaxReceiver is not SerializableTypeSyntaxReceiver receiver)
            {
                return;
            }

            // Get the compilation
            var compilation = context.Compilation;

            // Get the required interface symbols
            var irnsPacketGenericInterface = compilation.GetTypeByMetadataName("Serializer.Abstractions.IRnsPacket`1");
            var irnsPacketFieldInterface = compilation.GetTypeByMetadataName("Serializer.Abstractions.IRnsPacketField");

            if (irnsPacketGenericInterface == null || irnsPacketFieldInterface == null)
            {
                return;
            }

            // Process each candidate type
            foreach (var typeDeclaration in receiver.CandidateTypes)
            {
                var semanticModel = compilation.GetSemanticModel(typeDeclaration.SyntaxTree);
                var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);

                if (typeSymbol == null || !typeDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                    continue;

                // Check if the type implements IRnsPacket<TEnum> (top-level) or IRnsPacketField (nested)
                bool isTopLevel = ImplementsIRnsPacket(typeSymbol, irnsPacketGenericInterface);
                bool isNested = ImplementsIRnsPacketField(typeSymbol, irnsPacketFieldInterface);

                if (!isTopLevel && !isNested)
                    continue;

                // Generate the serialization code
                var generatedCode = GenerateRnsSerializationCode(typeSymbol, semanticModel, isTopLevel);
                if (!string.IsNullOrEmpty(generatedCode))
                {
                    var fileName = $"{typeSymbol.ContainingNamespace?.ToDisplayString()}.{typeSymbol.Name}.Rns.g.cs";
                    context.AddSource(fileName, generatedCode);
                }
            }
        }



        /// <summary>
        /// Checks if a type implements IRnsPacket&lt;TEnum&gt;
        /// </summary>
        private static bool ImplementsIRnsPacket(INamedTypeSymbol typeSymbol, INamedTypeSymbol irnsPacketGenericInterface)
        {
            return typeSymbol.AllInterfaces.Any(iface =>
                iface.IsGenericType &&
                SymbolEqualityComparer.Default.Equals(iface.ConstructedFrom, irnsPacketGenericInterface));
        }

        /// <summary>
        /// Checks if a type implements IRnsPacketField
        /// </summary>
        private static bool ImplementsIRnsPacketField(INamedTypeSymbol typeSymbol, INamedTypeSymbol irnsPacketFieldInterface)
        {
            return typeSymbol.AllInterfaces.Any(iface =>
                SymbolEqualityComparer.Default.Equals(iface, irnsPacketFieldInterface));
        }

        /// <summary>
        /// Generates RNS serialization code for a type
        /// </summary>
        private static string GenerateRnsSerializationCode(INamedTypeSymbol typeSymbol, SemanticModel semanticModel, bool isTopLevel)
        {
            var codeBuilder = new StringBuilder();
            var namespaceName = typeSymbol.ContainingNamespace?.ToDisplayString();
            var typeName = typeSymbol.Name;

            // Add using statements
            codeBuilder.AppendLine("// Generated serialization code");
            codeBuilder.AppendLine("#nullable enable");
            codeBuilder.AppendLine("using System;");
            codeBuilder.AppendLine("using System.Collections.Generic;");
            codeBuilder.AppendLine("using Serializer.Runtime;");
            codeBuilder.AppendLine();

            // Start namespace
            if (!string.IsNullOrEmpty(namespaceName))
            {
                codeBuilder.AppendLine($"namespace {namespaceName}");
                codeBuilder.AppendLine("{");
            }

            // Generate partial class
            codeBuilder.AppendLine($"    public partial class {typeName}");
            codeBuilder.AppendLine("    {");

            // Get serializable properties (exclude Id property)
            var properties = GetSerializableProperties(typeSymbol, isTopLevel);

            // Generate Keys class
            GenerateKeysClass(codeBuilder, properties);

            // Generate Write method
            GenerateWriteMethod(codeBuilder, properties, semanticModel.Compilation);

            // Generate TryRead method
            GenerateTryReadMethod(codeBuilder, typeName, properties, semanticModel.Compilation);

            codeBuilder.AppendLine("    }");

            // Close namespace
            if (!string.IsNullOrEmpty(namespaceName))
            {
                codeBuilder.AppendLine("}");
            }

            return codeBuilder.ToString();
        }

        /// <summary>
        /// Gets serializable properties (public instance auto-properties, excluding Id)
        /// </summary>
        private static List<IPropertySymbol> GetSerializableProperties(INamedTypeSymbol typeSymbol, bool isTopLevel)
        {
            return typeSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.GetMethod != null &&
                           p.SetMethod != null &&
                           !p.IsStatic &&
                           p.DeclaredAccessibility == Accessibility.Public &&
                           !(isTopLevel && p.Name == "Id")) // Exclude Id property on top-level IRnsPacket<TEnum>
                .OrderBy(p => p.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax().GetLocation().GetLineSpan().StartLinePosition.Line ?? 0)
                .ToList();
        }

        /// <summary>
        /// Generates the nested static Keys class
        /// </summary>
        private static void GenerateKeysClass(StringBuilder codeBuilder, List<IPropertySymbol> properties)
        {
            codeBuilder.AppendLine("        public static class Keys");
            codeBuilder.AppendLine("        {");

            ushort keyValue = 1;
            foreach (var property in properties)
            {
                codeBuilder.AppendLine($"            public const ushort {property.Name} = {keyValue};");
                keyValue++;
            }

            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates the Write method with specific wire format
        /// </summary>
        private static void GenerateWriteMethod(StringBuilder codeBuilder, List<IPropertySymbol> properties, Compilation compilation)
        {
            codeBuilder.AppendLine("        public bool Write(Span<byte> buffer, out int bytesWritten)");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine("            bytesWritten = 0;");
            codeBuilder.AppendLine("            var used = 0;");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("            try");
            codeBuilder.AppendLine("            {");

            foreach (var property in properties)
            {
                GenerateWritePropertyCode(codeBuilder, property, compilation);
            }

            codeBuilder.AppendLine("                bytesWritten = used;");
            codeBuilder.AppendLine("                return true;");
            codeBuilder.AppendLine("            }");
            codeBuilder.AppendLine("            catch");
            codeBuilder.AppendLine("            {");
            codeBuilder.AppendLine("                bytesWritten = 0;");
            codeBuilder.AppendLine("                return false;");
            codeBuilder.AppendLine("            }");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates write code for a single property
        /// </summary>
        private static void GenerateWritePropertyCode(StringBuilder codeBuilder, IPropertySymbol property, Compilation compilation)
        {
            var propertyName = property.Name;
            var propertyType = property.Type;

            codeBuilder.AppendLine($"                // Write {propertyName}");

            // Check if property should be omitted
            string defaultCheck = GetDefaultValueCheck(propertyType, propertyName);
            codeBuilder.AppendLine($"                if (!({defaultCheck}))");
            codeBuilder.AppendLine("                {");

            // Write Key (UInt16)
            codeBuilder.AppendLine($"                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.{propertyName});");

            // Write Value with specific encoding based on type
            if (IsSupportedPrimitiveType(propertyType))
            {
                GenerateWritePrimitiveValue(codeBuilder, property, compilation);
            }
            else if (propertyType.SpecialType == SpecialType.System_String)
            {
                GenerateWriteStringValue(codeBuilder, propertyName);
            }
            else if (IsIRnsPacketField(propertyType))
            {
                GenerateWriteNestedValue(codeBuilder, propertyName);
            }
            else if (IsArrayOfIRnsPacketField(propertyType))
            {
                GenerateWriteArrayValue(codeBuilder, propertyName);
            }

            codeBuilder.AppendLine("                }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Applies template code with proper indentation
        /// </summary>
        private static void ApplyTemplateCode(StringBuilder codeBuilder, string templateCode, string indentation)
        {
            var lines = templateCode.Split('\n');
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    codeBuilder.AppendLine($"{indentation}{line.Trim()}");
                }
            }
        }

        /// <summary>
        /// Generates code to write a primitive value with length prefix
        /// </summary>
        private static void GenerateWritePrimitiveValue(StringBuilder codeBuilder, IPropertySymbol property, Compilation compilation)
        {
            var propertyName = property.Name;
            var propertyType = property.Type;

            // Use templates for all supported types
            if (propertyType.SpecialType == SpecialType.System_Int32)
            {
                var templateCode = Int32Template.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Boolean)
            {
                var templateCode = BooleanTemplate.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Byte)
            {
                var templateCode = ByteTemplate.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_SByte)
            {
                var templateCode = SByteTemplate.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Int16)
            {
                var templateCode = Int16Template.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_UInt16)
            {
                var templateCode = UInt16Template.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_UInt32)
            {
                var templateCode = UInt32Template.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Int64)
            {
                var templateCode = Int64Template.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_UInt64)
            {
                var templateCode = UInt64Template.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Single)
            {
                var templateCode = SingleTemplate.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Double)
            {
                var templateCode = DoubleTemplate.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (IsGuidType(propertyType))
            {
                var templateCode = GuidTemplate.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else if (IsEnumType(propertyType))
            {
                var templateCode = EnumTemplate.GenerateWriteCode(propertyName, compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                    ");
            }
            else
            {
                // Fallback to original approach for unsupported types
                var (writeMethod, length) = GetPrimitiveWriteInfo(propertyType);
                codeBuilder.AppendLine($"                    used += RndCodec.{writeMethod}(buffer.Slice(used), {propertyName});");
            }
        }

        /// <summary>
        /// Generates code to write a string value
        /// </summary>
        private static void GenerateWriteStringValue(StringBuilder codeBuilder, string propertyName)
        {
            // For strings, we need to write a length prefix because RndCodec.WriteString includes its own length
            // The length should be the total bytes that RndCodec.WriteString will write
            codeBuilder.AppendLine($"                    var {propertyName}Bytes = System.Text.Encoding.UTF8.GetByteCount({propertyName});");
            codeBuilder.AppendLine($"                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)(2 + {propertyName}Bytes)); // Length (2 for string length + string bytes)");
            codeBuilder.AppendLine($"                    used += RndCodec.WriteString(buffer.Slice(used), {propertyName});");
        }

        /// <summary>
        /// Generates code to write a nested IRnsPacketField value
        /// </summary>
        private static void GenerateWriteNestedValue(StringBuilder codeBuilder, string propertyName)
        {
            codeBuilder.AppendLine($"                    // Write nested object as length-prefixed payload");
            codeBuilder.AppendLine($"                    var nestedBuffer = new byte[1024]; // Temporary buffer");
            codeBuilder.AppendLine($"                    if ({propertyName}!.Write(nestedBuffer, out var nestedLength))");
            codeBuilder.AppendLine("                    {");
            codeBuilder.AppendLine("                        used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)nestedLength);");
            codeBuilder.AppendLine("                        nestedBuffer.AsSpan(0, nestedLength).CopyTo(buffer.Slice(used));");
            codeBuilder.AppendLine("                        used += nestedLength;");
            codeBuilder.AppendLine("                    }");
        }

        /// <summary>
        /// Generates code to write an array of IRnsPacketField values
        /// </summary>
        private static void GenerateWriteArrayValue(StringBuilder codeBuilder, string propertyName)
        {
            // For arrays, we need to calculate the total length first, then write it as a prefix
            // The length includes: array count (2 bytes) + sum of all item lengths and data
            codeBuilder.AppendLine($"                    var {propertyName}TotalLength = 2; // Start with array count");
            codeBuilder.AppendLine($"                    foreach (var item in {propertyName})");
            codeBuilder.AppendLine("                    {");
            codeBuilder.AppendLine("                        var itemBuffer = new byte[1024]; // Temporary buffer");
            codeBuilder.AppendLine("                        if (item.Write(itemBuffer, out var itemLength))");
            codeBuilder.AppendLine("                        {");
            codeBuilder.AppendLine($"                            {propertyName}TotalLength += 2 + itemLength; // Item length (2 bytes) + item data");
            codeBuilder.AppendLine("                        }");
            codeBuilder.AppendLine("                    }");
            codeBuilder.AppendLine($"                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort){propertyName}TotalLength); // Total length prefix");
            codeBuilder.AppendLine($"                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort){propertyName}.Length); // Array count");
            codeBuilder.AppendLine($"                    foreach (var item in {propertyName})");
            codeBuilder.AppendLine("                    {");
            codeBuilder.AppendLine("                        var itemBuffer = new byte[1024]; // Temporary buffer");
            codeBuilder.AppendLine("                        if (item.Write(itemBuffer, out var itemLength))");
            codeBuilder.AppendLine("                        {");
            codeBuilder.AppendLine("                            used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)itemLength);");
            codeBuilder.AppendLine("                            itemBuffer.AsSpan(0, itemLength).CopyTo(buffer.Slice(used));");
            codeBuilder.AppendLine("                            used += itemLength;");
            codeBuilder.AppendLine("                        }");
            codeBuilder.AppendLine("                    }");
        }

        /// <summary>
        /// Generates the TryRead method with specific wire format handling
        /// </summary>
        private static void GenerateTryReadMethod(StringBuilder codeBuilder, string typeName, List<IPropertySymbol> properties, Compilation compilation)
        {
            codeBuilder.AppendLine($"        public static bool TryRead(ReadOnlySpan<byte> buffer, ref int consumed, out {typeName} readPacket)");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine($"            readPacket = new {typeName}();");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("            try");
            codeBuilder.AppendLine("            {");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("                while (consumed < buffer.Length)");
            codeBuilder.AppendLine("                {");
            codeBuilder.AppendLine("                    if (consumed + 4 > buffer.Length) break; // Need at least key + length");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("                    consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var key);");
            codeBuilder.AppendLine("                    consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var len);");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("                    if (consumed + len > buffer.Length) return false; // Malformed");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("                    switch (key)");
            codeBuilder.AppendLine("                    {");

            // Generate switch cases for known keys
            foreach (var property in properties)
            {
                GenerateReadPropertyCase(codeBuilder, property, compilation);
            }

            codeBuilder.AppendLine("                        default:");
            codeBuilder.AppendLine("                            // Unknown key - skip len bytes");
            codeBuilder.AppendLine("                            consumed += len;");
            codeBuilder.AppendLine("                            break;");
            codeBuilder.AppendLine("                    }");
            codeBuilder.AppendLine("                }");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("                return true;");
            codeBuilder.AppendLine("            }");
            codeBuilder.AppendLine("            catch");
            codeBuilder.AppendLine("            {");
            codeBuilder.AppendLine("                return false;");
            codeBuilder.AppendLine("            }");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates a switch case for reading a property
        /// </summary>
        private static void GenerateReadPropertyCase(StringBuilder codeBuilder, IPropertySymbol property, Compilation compilation)
        {
            var propertyName = property.Name;
            var propertyType = property.Type;

            codeBuilder.AppendLine($"                        case Keys.{propertyName}:");

            if (IsSupportedPrimitiveType(propertyType))
            {
                GenerateReadPrimitiveCase(codeBuilder, property, compilation);
            }
            else if (propertyType.SpecialType == SpecialType.System_String)
            {
                GenerateReadStringCase(codeBuilder, propertyName);
            }
            else if (IsIRnsPacketField(propertyType))
            {
                GenerateReadNestedCase(codeBuilder, property);
            }
            else if (IsArrayOfIRnsPacketField(propertyType))
            {
                GenerateReadArrayCase(codeBuilder, property);
            }

            codeBuilder.AppendLine("                            break;");
        }

        /// <summary>
        /// Generates read case for primitive types
        /// </summary>
        private static void GenerateReadPrimitiveCase(StringBuilder codeBuilder, IPropertySymbol property, Compilation compilation)
        {
            var propertyName = property.Name;

            var propertyType = property.Type;
            var (readMethod, expectedLength) = GetPrimitiveReadInfo(propertyType);

            if (propertyType.SpecialType == SpecialType.System_Int32)
            {
                var templateCode = Int32Template.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Boolean)
            {
                var templateCode = BooleanTemplate.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Byte)
            {
                var templateCode = ByteTemplate.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_SByte)
            {
                var templateCode = SByteTemplate.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Int16)
            {
                var templateCode = Int16Template.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_UInt16)
            {
                var templateCode = UInt16Template.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_UInt32)
            {
                var templateCode = UInt32Template.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Int64)
            {
                var templateCode = Int64Template.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_UInt64)
            {
                var templateCode = UInt64Template.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Single)
            {
                var templateCode = SingleTemplate.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (propertyType.SpecialType == SpecialType.System_Double)
            {
                var templateCode = DoubleTemplate.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (IsGuidType(propertyType))
            {
                var templateCode = GuidTemplate.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else if (IsEnumType(propertyType))
            {
                var templateCode = EnumTemplate.GenerateReadCode(propertyName, "readPacket", compilation);
                ApplyTemplateCode(codeBuilder, templateCode, "                            ");
            }
            else
            {
                // Fallback to original approach for other primitive types
                codeBuilder.AppendLine($"                            consumed += RndCodec.{readMethod}(buffer.Slice(consumed), out var {propertyName}Value);");
                codeBuilder.AppendLine($"                            readPacket.{propertyName} = {propertyName}Value;");
            }
        }

        /// <summary>
        /// Generates read case for string
        /// </summary>
        private static void GenerateReadStringCase(StringBuilder codeBuilder, string propertyName)
        {
            // For strings, the length prefix includes the string's own length prefix (2 bytes) plus string bytes
            // RndCodec.ReadString will read the string's length prefix and string bytes
            // We need to ensure we don't read beyond the expected length
            codeBuilder.AppendLine($"                            var {propertyName}Start = consumed;");
            codeBuilder.AppendLine($"                            consumed += RndCodec.ReadString(buffer.Slice(consumed), out var {propertyName}Value);");
            codeBuilder.AppendLine($"                            readPacket.{propertyName} = {propertyName}Value;");
            codeBuilder.AppendLine($"                            // Verify we didn't read beyond the expected length");
            codeBuilder.AppendLine($"                            if (consumed - {propertyName}Start > len) return false;");
        }

        /// <summary>
        /// Generates read case for nested IRnsPacketField
        /// </summary>
        private static void GenerateReadNestedCase(StringBuilder codeBuilder, IPropertySymbol property)
        {
            var propertyName = property.Name;
            var fullTypeName = property.Type.ToDisplayString();
            // Remove nullable annotation if present - MODIFIED VERSION
            var typeName = fullTypeName.Replace("?", "");

            // Debug: Add a comment showing what we got
            codeBuilder.AppendLine($"                            // DEBUG: Original type: {fullTypeName}, After replace: {typeName}");
            codeBuilder.AppendLine($"                            if ({typeName}.TryRead(buffer.Slice(consumed, len), ref consumed, out var {propertyName}Value))");
            codeBuilder.AppendLine("                            {");
            codeBuilder.AppendLine($"                                readPacket.{propertyName} = {propertyName}Value;");
            codeBuilder.AppendLine("                            }");
            codeBuilder.AppendLine($"                            consumed += len;");
        }

        /// <summary>
        /// Generates read case for array of IRnsPacketField
        /// </summary>
        private static void GenerateReadArrayCase(StringBuilder codeBuilder, IPropertySymbol property)
        {
            var propertyName = property.Name;
            var fullElementType = ((IArrayTypeSymbol)property.Type).ElementType.ToDisplayString();
            // Remove nullable annotation if present
            var elementType = fullElementType.Replace("?", "");

            codeBuilder.AppendLine("                            var arrayStart = consumed;");
            codeBuilder.AppendLine("                            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var count);");
            codeBuilder.AppendLine($"                            var {propertyName}List = new List<{elementType}>();");
            codeBuilder.AppendLine("                            for (int i = 0; i < count; i++)");
            codeBuilder.AppendLine("                            {");
            codeBuilder.AppendLine("                                consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var itemLen);");
            codeBuilder.AppendLine($"                                if ({elementType}.TryRead(buffer.Slice(consumed, itemLen), ref consumed, out var item))");
            codeBuilder.AppendLine("                                {");
            codeBuilder.AppendLine($"                                    {propertyName}List.Add(item);");
            codeBuilder.AppendLine("                                }");
            codeBuilder.AppendLine("                                consumed += itemLen;");
            codeBuilder.AppendLine("                            }");
            codeBuilder.AppendLine($"                            readPacket.{propertyName} = {propertyName}List.ToArray();");
        }

        /// <summary>
        /// Gets the default value check for omitting defaults
        /// </summary>
        private static string GetDefaultValueCheck(ITypeSymbol type, string propertyName)
        {
            if (IsEnumType(type))
            {
                return $"{propertyName} == default";
            }

            return type.SpecialType switch
            {
                SpecialType.System_String => $"string.IsNullOrEmpty({propertyName})",
                SpecialType.System_Boolean => $"{propertyName} == false",
                SpecialType.System_Byte => $"{propertyName} == 0",
                SpecialType.System_SByte => $"{propertyName} == 0",
                SpecialType.System_Int16 => $"{propertyName} == 0",
                SpecialType.System_UInt16 => $"{propertyName} == 0",
                SpecialType.System_Int32 => $"{propertyName} == 0",
                SpecialType.System_UInt32 => $"{propertyName} == 0",
                SpecialType.System_Int64 => $"{propertyName} == 0",
                SpecialType.System_UInt64 => $"{propertyName} == 0",
                SpecialType.System_Single => $"{propertyName} == 0f",
                SpecialType.System_Double => $"{propertyName} == 0.0",
                _ when IsIRnsPacketField(type) => $"{propertyName} == null",
                _ when IsArrayOfIRnsPacketField(type) => $"{propertyName} == null || {propertyName}.Length == 0",
                _ => $"{propertyName} == default"
            };
        }

        /// <summary>
        /// Checks if type is a supported primitive type
        /// </summary>
        private static bool IsSupportedPrimitiveType(ITypeSymbol type)
        {
            if (type.TypeKind == TypeKind.Enum) return true;

            return type.SpecialType is SpecialType.System_Boolean or
                                     SpecialType.System_Byte or
                                     SpecialType.System_SByte or
                                     SpecialType.System_Int16 or
                                     SpecialType.System_UInt16 or
                                     SpecialType.System_Int32 or
                                     SpecialType.System_UInt32 or
                                     SpecialType.System_Int64 or
                                     SpecialType.System_UInt64 or
                                     SpecialType.System_Single or
                                     SpecialType.System_Double;
        }

        /// <summary>
        /// Checks if type is an enum type
        /// </summary>
        private static bool IsEnumType(ITypeSymbol type)
        {
            return type.TypeKind == TypeKind.Enum;
        }

        /// <summary>
        /// Checks if type is a GUID type
        /// </summary>
        private static bool IsGuidType(ITypeSymbol type)
        {
            if (type is not INamedTypeSymbol namedType) return false;
            return namedType.Name == "Guid" && namedType.ContainingNamespace?.Name == "System";
        }

        /// <summary>
        /// Checks if type implements IRnsPacketField
        /// </summary>
        private static bool IsIRnsPacketField(ITypeSymbol type)
        {
            if (type is not INamedTypeSymbol namedType) return false;
            return namedType.AllInterfaces.Any(i => i.Name == "IRnsPacketField");
        }

        /// <summary>
        /// Checks if type is an array of IRnsPacketField
        /// </summary>
        private static bool IsArrayOfIRnsPacketField(ITypeSymbol type)
        {
            if (type is not IArrayTypeSymbol arrayType) return false;
            return IsIRnsPacketField(arrayType.ElementType);
        }

        /// <summary>
        /// Gets write method and length for primitive types
        /// </summary>
        private static (string writeMethod, int length) GetPrimitiveWriteInfo(ITypeSymbol type)
        {
            if (IsEnumType(type))
            {
                return ("WriteInt32", 4); // Enums are typically stored as int
            }

            return type.SpecialType switch
            {
                SpecialType.System_Boolean => ("WriteBoolean", 1),
                SpecialType.System_Byte => ("WriteByte", 1),
                SpecialType.System_SByte => ("WriteSByte", 1),
                SpecialType.System_Int16 => ("WriteInt16", 2),
                SpecialType.System_UInt16 => ("WriteUInt16", 2),
                SpecialType.System_Int32 => ("WriteInt32", 4),
                SpecialType.System_UInt32 => ("WriteUInt32", 4),
                SpecialType.System_Int64 => ("WriteInt64", 8),
                SpecialType.System_UInt64 => ("WriteUInt64", 8),
                SpecialType.System_Single => ("WriteSingle", 4),
                SpecialType.System_Double => ("WriteDouble", 8),
                _ => ("WriteByte", 1)
            };
        }

        /// <summary>
        /// Gets read method and expected length for primitive types
        /// </summary>
        private static (string readMethod, int length) GetPrimitiveReadInfo(ITypeSymbol type)
        {
            if (IsEnumType(type))
            {
                return ("ReadInt32", 4); // Enums are typically stored as int
            }

            return type.SpecialType switch
            {
                SpecialType.System_Boolean => ("ReadBooleanStrict", 1),
                SpecialType.System_Byte => ("ReadByte", 1),
                SpecialType.System_SByte => ("ReadSByte", 1),
                SpecialType.System_Int16 => ("ReadInt16", 2),
                SpecialType.System_UInt16 => ("ReadUInt16", 2),
                SpecialType.System_Int32 => ("ReadInt32", 4),
                SpecialType.System_UInt32 => ("ReadUInt32", 4),
                SpecialType.System_Int64 => ("ReadInt64", 8),
                SpecialType.System_UInt64 => ("ReadUInt64", 8),
                SpecialType.System_Single => ("ReadSingle", 4),
                SpecialType.System_Double => ("ReadDouble", 8),
                _ => ("ReadByte", 1)
            };
        }
    }
}
