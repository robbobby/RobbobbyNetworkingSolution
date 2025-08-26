using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            // Emit the runtime helper once
            EmitRuntimeHelper(context);

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
        /// Emits the runtime helper class once per compilation
        /// </summary>
        private static void EmitRuntimeHelper(GeneratorExecutionContext context)
        {
            var source = @"// Generated RnsKeyRegistry runtime helper
#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Serializer.Generator.Runtime
{
    public static class RnsKeyRegistry
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<ushort, string>> _byType = new();

        public static void Register<T>(IReadOnlyDictionary<ushort, string> map) => _byType[typeof(T)] = map;

        public static bool TryGet(Type t, ushort key, out string? name)
        {
            name = null;
            return _byType.TryGetValue(t, out var m) && m.TryGetValue(key, out name);
        }
    }
}";
            context.AddSource("RnsKeyRegistry.g.cs", source);
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
            codeBuilder.AppendLine("using Serializer.Generator.Runtime;");
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

            // Generate static constructor with key registry
            GenerateStaticConstructor(codeBuilder, typeName, properties);

            // Generate Write method
            GenerateWriteMethod(codeBuilder, properties);

            // Generate TryRead method
            GenerateTryReadMethod(codeBuilder, typeName, properties);

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
        /// Generates static constructor that registers key map
        /// </summary>
        private static void GenerateStaticConstructor(StringBuilder codeBuilder, string typeName, List<IPropertySymbol> properties)
        {
            codeBuilder.AppendLine($"        static {typeName}()");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine($"            RnsKeyRegistry.Register<{typeName}>(new Dictionary<ushort, string>");
            codeBuilder.AppendLine("            {");
            
            foreach (var property in properties)
            {
                codeBuilder.AppendLine($"                {{ Keys.{property.Name}, nameof({property.Name}) }},");
            }
            
            codeBuilder.AppendLine("            });");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates the Write method with specific wire format
        /// </summary>
        private static void GenerateWriteMethod(StringBuilder codeBuilder, List<IPropertySymbol> properties)
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
                GenerateWritePropertyCode(codeBuilder, property);
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
        private static void GenerateWritePropertyCode(StringBuilder codeBuilder, IPropertySymbol property)
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
                GenerateWritePrimitiveValue(codeBuilder, property);
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
        /// Generates code to write a primitive value with length prefix
        /// </summary>
        private static void GenerateWritePrimitiveValue(StringBuilder codeBuilder, IPropertySymbol property)
        {
            var propertyName = property.Name;
            var propertyType = property.Type;
            
            var (writeMethod, length) = GetPrimitiveWriteInfo(propertyType);
            
            codeBuilder.AppendLine($"                    used += RndCodec.WriteUInt16(buffer.Slice(used), {length}); // Length");
            codeBuilder.AppendLine($"                    used += RndCodec.{writeMethod}(buffer.Slice(used), {propertyName});");
        }

        /// <summary>
        /// Generates code to write a string value
        /// </summary>
        private static void GenerateWriteStringValue(StringBuilder codeBuilder, string propertyName)
        {
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
            codeBuilder.AppendLine($"                    // Write array count");
            codeBuilder.AppendLine($"                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort){propertyName}.Length);");
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
        private static void GenerateTryReadMethod(StringBuilder codeBuilder, string typeName, List<IPropertySymbol> properties)
        {
            codeBuilder.AppendLine($"        public static bool TryRead(ReadOnlySpan<byte> buffer, out {typeName} readPacket, out int bytesRead)");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine($"            readPacket = new {typeName}();");
            codeBuilder.AppendLine("            bytesRead = 0;");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("            try");
            codeBuilder.AppendLine("            {");
            codeBuilder.AppendLine("                var consumed = 0;");
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
                GenerateReadPropertyCase(codeBuilder, property);
            }

            codeBuilder.AppendLine("                        default:");
            codeBuilder.AppendLine("                            // Unknown key - skip len bytes");
            codeBuilder.AppendLine("                            consumed += len;");
            codeBuilder.AppendLine("                            break;");
            codeBuilder.AppendLine("                    }");
            codeBuilder.AppendLine("                }");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("                bytesRead = consumed;");
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
        private static void GenerateReadPropertyCase(StringBuilder codeBuilder, IPropertySymbol property)
        {
            var propertyName = property.Name;
            var propertyType = property.Type;

            codeBuilder.AppendLine($"                        case Keys.{propertyName}:");
            
            if (IsSupportedPrimitiveType(propertyType))
            {
                GenerateReadPrimitiveCase(codeBuilder, property);
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
        private static void GenerateReadPrimitiveCase(StringBuilder codeBuilder, IPropertySymbol property)
        {
            var propertyName = property.Name;
            var propertyType = property.Type;
            var (readMethod, expectedLength) = GetPrimitiveReadInfo(propertyType);
            
            codeBuilder.AppendLine($"                            if (len != {expectedLength}) return false;");
            codeBuilder.AppendLine($"                            consumed += RndCodec.{readMethod}(buffer.Slice(consumed), out var {propertyName}Value);");
            codeBuilder.AppendLine($"                            readPacket.{propertyName} = {propertyName}Value;");
        }

        /// <summary>
        /// Generates read case for string
        /// </summary>
        private static void GenerateReadStringCase(StringBuilder codeBuilder, string propertyName)
        {
            codeBuilder.AppendLine($"                            consumed += RndCodec.ReadString(buffer.Slice(consumed), out var {propertyName}Value);");
            codeBuilder.AppendLine($"                            readPacket.{propertyName} = {propertyName}Value;");
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
            codeBuilder.AppendLine($"                            if ({typeName}.TryRead(buffer.Slice(consumed, len), out var {propertyName}Value, out var {propertyName}Read))");
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
            codeBuilder.AppendLine($"                                if ({elementType}.TryRead(buffer.Slice(consumed, itemLen), out var item, out var itemRead))");
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
            return type.SpecialType switch
            {
                SpecialType.System_String => $"string.IsNullOrEmpty({propertyName})",
                SpecialType.System_Boolean => $"{propertyName} == false",
                SpecialType.System_Int32 => $"{propertyName} == 0",
                SpecialType.System_Single => $"{propertyName} == 0f",
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
            return type.SpecialType is SpecialType.System_Boolean or 
                                     SpecialType.System_Int32 or 
                                     SpecialType.System_Single;
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
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => ("WriteBoolean", 1),
                SpecialType.System_Int32 => ("WriteInt32", 4),
                SpecialType.System_Single => ("WriteSingle", 4),
                _ => ("WriteByte", 1)
            };
        }

        /// <summary>
        /// Gets read method and expected length for primitive types
        /// </summary>
        private static (string readMethod, int length) GetPrimitiveReadInfo(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => ("ReadBooleanStrict", 1),
                SpecialType.System_Int32 => ("ReadInt32", 4),
                SpecialType.System_Single => ("ReadSingle", 4),
                _ => ("ReadByte", 1)
            };
        }
    }
}