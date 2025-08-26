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
    /// Roslyn source generator for serialization code
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
            // Register for syntax notifications to find types with [RnsSerializable] attribute
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
                context.AddSource("debug.txt", "// Syntax receiver not found");
                return;
            }

            // Get the compilation
            var compilation = context.Compilation;

            // Get the RnsSerializable attribute symbol
            var rnsSerializableAttribute = compilation.GetTypeByMetadataName("Serializer.Abstractions.RnsSerializableAttribute");
            if (rnsSerializableAttribute == null)
            {
                context.AddSource("debug.txt", "// RnsSerializableAttribute not found");
                return;
            }

            // Get the IRnsPacket interface symbol
            var irnsPacketInterface = compilation.GetTypeByMetadataName("Serializer.Abstractions.IRnsPacket");
            if (irnsPacketInterface == null)
            {
                context.AddSource("debug.txt", "// IRnsPacket interface not found");
                return;
            }

            // Get the IRnsPacket<TId> interface symbol
            var irnsPacketGenericInterface = compilation.GetTypeByMetadataName("Serializer.Abstractions.IRnsPacket`1");
            if (irnsPacketGenericInterface == null)
            {
                context.AddSource("debug.txt", "// IRnsPacket<TId> interface not found");
                return;
            }

            // Debug: Log what we found
            var debugInfo = $"// Found {receiver.CandidateTypes.Count} candidate types\n";
            debugInfo += $"// RnsSerializableAttribute: {rnsSerializableAttribute}\n";
            debugInfo += $"// IRnsPacket: {irnsPacketInterface}\n";
            debugInfo += $"// IRnsPacket<TId>: {irnsPacketGenericInterface}\n";

            // Log each candidate type
            foreach (var typeDeclaration in receiver.CandidateTypes)
            {
                debugInfo += $"// Candidate: {typeDeclaration.Identifier.Text}\n";
            }

            context.AddSource("debug.txt", debugInfo);

            // Process each candidate type
            foreach (var typeDeclaration in receiver.CandidateTypes)
            {
                var semanticModel = compilation.GetSemanticModel(typeDeclaration.SyntaxTree);
                var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);

                if (typeSymbol == null)
                    continue;

                // Check if the type has the [RnsSerializable] attribute
                if (!HasRnsSerializableAttribute(typeSymbol, rnsSerializableAttribute))
                    continue;

                // Check if the type implements IRnsPacket<TId>
                if (!ImplementsIRnsPacketTId(typeSymbol, irnsPacketGenericInterface))
                    continue;

                // Generate the serialization code
                var generatedCode = GenerateSerializationCode(typeSymbol, semanticModel);
                if (!string.IsNullOrEmpty(generatedCode))
                {
                    var fileName = $"{typeSymbol.Name}.Generated.cs";
                    context.AddSource(fileName, generatedCode);
                }
            }
        }

        /// <summary>
        /// Checks if a type has the [RnsSerializable] attribute
        /// </summary>
        private static bool HasRnsSerializableAttribute(INamedTypeSymbol typeSymbol, INamedTypeSymbol rnsSerializableAttribute)
        {
            return typeSymbol.GetAttributes()
                .Any(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, rnsSerializableAttribute));
        }

        /// <summary>
        /// Checks if a type implements IRnsPacket<TId>
        /// </summary>
        private static bool ImplementsIRnsPacketTId(INamedTypeSymbol typeSymbol, INamedTypeSymbol irnsPacketGenericInterface)
        {
            // Check if the type directly implements IRnsPacket<TId>
            if (typeSymbol.AllInterfaces.Any(iface =>
                iface.IsGenericType &&
                SymbolEqualityComparer.Default.Equals(iface.ConstructedFrom, irnsPacketGenericInterface)))
                return true;

            // Check if any base type implements IRnsPacket<TId>
            var baseType = typeSymbol.BaseType;
            while (baseType != null)
            {
                if (baseType.AllInterfaces.Any(iface =>
                    iface.IsGenericType &&
                    SymbolEqualityComparer.Default.Equals(iface.ConstructedFrom, irnsPacketGenericInterface)))
                    return true;
                baseType = baseType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Generates serialization code for a type
        /// </summary>
        private static string GenerateSerializationCode(INamedTypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            var codeBuilder = new StringBuilder();

            // Add using statements
            codeBuilder.AppendLine("using System;");
            codeBuilder.AppendLine();

            // Generate the partial class with serialization methods
            GeneratePartialClass(codeBuilder, typeSymbol, semanticModel);

            return codeBuilder.ToString();
        }

        /// <summary>
        /// Generates the packet keys interface
        /// </summary>
        private static void GeneratePacketKeysInterface(StringBuilder codeBuilder, INamedTypeSymbol typeSymbol)
        {
            var namespaceName = typeSymbol.ContainingNamespace?.ToDisplayString() ?? "";
            var typeName = typeSymbol.Name;
            var interfaceName = $"I{typeName}Keys";

            codeBuilder.AppendLine($"namespace {namespaceName}");
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine($"    /// <summary>");
            codeBuilder.AppendLine($"    /// Interface for packet keys of {typeName}");
            codeBuilder.AppendLine($"    /// </summary>");
            codeBuilder.AppendLine($"    public interface {interfaceName}");
            codeBuilder.AppendLine("    {");
            codeBuilder.AppendLine($"        int PacketTypeId {{ get; }}");
            codeBuilder.AppendLine($"        string PacketName {{ get; }}");
            codeBuilder.AppendLine($"        int PacketVersion {{ get; }}");
            codeBuilder.AppendLine($"        Type PacketType {{ get; }}");
            codeBuilder.AppendLine("    }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates the packet keys static class
        /// </summary>
        private static void GeneratePacketKeysClass(StringBuilder codeBuilder, INamedTypeSymbol typeSymbol)
        {
            var namespaceName = typeSymbol.ContainingNamespace?.ToDisplayString() ?? "";
            var typeName = typeSymbol.Name;
            var keysClassName = $"{typeName}Keys";
            var interfaceName = $"I{typeName}Keys";

            codeBuilder.AppendLine($"    /// <summary>");
            codeBuilder.AppendLine($"    /// Packet keys for {typeName}");
            codeBuilder.AppendLine($"    /// </summary>");
            codeBuilder.AppendLine($"    public class {keysClassName} : {interfaceName}");
            codeBuilder.AppendLine("    {");
            codeBuilder.AppendLine($"        public int PacketTypeId => {GetPacketTypeId(typeName)};");
            codeBuilder.AppendLine($"        public string PacketName => \"{typeName}\";");
            codeBuilder.AppendLine($"        public int PacketVersion => 1;");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine($"        /// <summary>");
            codeBuilder.AppendLine($"        /// Runtime-accessible metadata for {typeName}");
            codeBuilder.AppendLine($"        /// </summary>");
            codeBuilder.AppendLine($"        public Type PacketType => typeof({typeName});");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine($"        /// <summary>");
            codeBuilder.AppendLine($"        /// Singleton instance for RnsKeys property access");
            codeBuilder.AppendLine($"        /// </summary>");
            codeBuilder.AppendLine($"        public static readonly {interfaceName} Instance = new {keysClassName}();");
            codeBuilder.AppendLine("    }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates the partial class with RNS Binary Spec v1 serialization methods and compile-time field constants
        /// </summary>
        private static void GeneratePartialClass(StringBuilder codeBuilder, INamedTypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            var namespaceName = typeSymbol.ContainingNamespace?.ToDisplayString() ?? "";
            var typeName = typeSymbol.Name;

            codeBuilder.AppendLine($"namespace {namespaceName}");
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine($"    /// <summary>");
            codeBuilder.AppendLine($"    /// Generated partial class for {typeName} with RNS Binary Spec v1 serialization support");
            codeBuilder.AppendLine($"    /// </summary>");
            codeBuilder.AppendLine($"    public partial class {typeName}");
            codeBuilder.AppendLine("    {");

            // Generate compile-time field constants as requested by the user
            codeBuilder.AppendLine("        /// <summary>");
            codeBuilder.AppendLine("        /// Compile-time field number constants for fast lookup");
            codeBuilder.AppendLine("        /// </summary>");
            codeBuilder.AppendLine("        public static class Keys");
            codeBuilder.AppendLine("        {");

            var properties = GetSerializableProperties(typeSymbol);
            var fieldNumber = 1;
            foreach (var property in properties)
            {
                codeBuilder.AppendLine($"            public const int {property.Name} = {fieldNumber};");
                fieldNumber++;
            }

            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();

            // Generate serialization methods
            GenerateWriteMethod(codeBuilder, typeSymbol, semanticModel);
            GenerateTryReadMethod(codeBuilder, typeSymbol, semanticModel);

            codeBuilder.AppendLine("    }");
            codeBuilder.AppendLine("}");
        }

        /// <summary>
        /// Generates the Write method for RNS Binary Spec v1 serialization with protobuf-style encoding
        /// </summary>
        private static void GenerateWriteMethod(StringBuilder codeBuilder, INamedTypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            codeBuilder.AppendLine($"        /// <summary>");
            codeBuilder.AppendLine($"        /// Writes the current instance to the specified buffer using RNS Binary Spec v1");
            codeBuilder.AppendLine($"        /// </summary>");
            codeBuilder.AppendLine($"        /// <param name=\"buffer\">The destination buffer</param>");
            codeBuilder.AppendLine($"        /// <param name=\"bytesWritten\">The number of bytes written</param>");
            codeBuilder.AppendLine($"        /// <returns>True if successful, false if buffer is too small</returns>");
            codeBuilder.AppendLine($"        public bool Write(System.Span<byte> buffer, out int bytesWritten)");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine("            bytesWritten = 0;");
            codeBuilder.AppendLine("            var dst = buffer;");
            codeBuilder.AppendLine();

            // Generate write code for each property using RNS Binary Spec v1
            var properties = GetSerializableProperties(typeSymbol);
            var fieldNumber = 1;
            foreach (var property in properties)
            {
                var propertyType = property.Type;
                var propertyName = property.Name;
                
                // Generate field write logic based on type
                if (propertyType.SpecialType == SpecialType.System_String)
                {
                    codeBuilder.AppendLine($"            // {propertyName} (field {fieldNumber}, WT=2)");
                    codeBuilder.AppendLine($"            if (!string.IsNullOrEmpty({propertyName}))");
                    codeBuilder.AppendLine("            {");
                    codeBuilder.AppendLine($"                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.{propertyName}, 2), out var w1)) {{ bytesWritten = 0; return false; }}");
                    codeBuilder.AppendLine($"                dst = dst[w1..]; bytesWritten += w1;");
                    codeBuilder.AppendLine($"                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, {propertyName}, out var w2)) {{ bytesWritten = 0; return false; }}");
                    codeBuilder.AppendLine($"                dst = dst[w2..]; bytesWritten += w2;");
                    codeBuilder.AppendLine("            }");
                    codeBuilder.AppendLine();
                }
                else if (propertyType.SpecialType == SpecialType.System_Single)
                {
                    codeBuilder.AppendLine($"            // {propertyName} (field {fieldNumber}, WT=5)");
                    codeBuilder.AppendLine($"            if ({propertyName} != 0f)");
                    codeBuilder.AppendLine("            {");
                    codeBuilder.AppendLine($"                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.{propertyName}, 5), out var w1)) {{ bytesWritten = 0; return false; }}");
                    codeBuilder.AppendLine($"                dst = dst[w1..]; bytesWritten += w1;");
                    codeBuilder.AppendLine($"                if (!Serializer.Runtime.RnsCodec.TryWriteFloat32(dst, {propertyName}, out var w2)) {{ bytesWritten = 0; return false; }}");
                    codeBuilder.AppendLine($"                dst = dst[w2..]; bytesWritten += w2;");
                    codeBuilder.AppendLine("            }");
                    codeBuilder.AppendLine();
                }
                else if (propertyType.SpecialType == SpecialType.System_Int32)
                {
                    codeBuilder.AppendLine($"            // {propertyName} (field {fieldNumber}, WT=0 via ZigZag)");
                    codeBuilder.AppendLine($"            if ({propertyName} != 0)");
                    codeBuilder.AppendLine("            {");
                    codeBuilder.AppendLine($"                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.{propertyName}, 0), out var w1)) {{ bytesWritten = 0; return false; }}");
                    codeBuilder.AppendLine($"                dst = dst[w1..]; bytesWritten += w1;");
                    codeBuilder.AppendLine($"                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, Serializer.Runtime.RnsCodec.ZigZag32({propertyName}), out var w2)) {{ bytesWritten = 0; return false; }}");
                    codeBuilder.AppendLine($"                dst = dst[w2..]; bytesWritten += w2;");
                    codeBuilder.AppendLine("            }");
                    codeBuilder.AppendLine();
                }
                else if (propertyType.SpecialType == SpecialType.System_Boolean)
                {
                    codeBuilder.AppendLine($"            // {propertyName} (field {fieldNumber}, WT=0)");
                    codeBuilder.AppendLine($"            if ({propertyName})");
                    codeBuilder.AppendLine("            {");
                    codeBuilder.AppendLine($"                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.{propertyName}, 0), out var w1)) {{ bytesWritten = 0; return false; }}");
                    codeBuilder.AppendLine($"                dst = dst[w1..]; bytesWritten += w1;");
                    codeBuilder.AppendLine($"                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, 1u, out var w2)) {{ bytesWritten = 0; return false; }}");
                    codeBuilder.AppendLine($"                dst = dst[w2..]; bytesWritten += w2;");
                    codeBuilder.AppendLine("            }");
                    codeBuilder.AppendLine();
                }
                // Handle nested objects and arrays would go here, but for now keep it simple

                fieldNumber++;
            }

            codeBuilder.AppendLine("            return true;");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates the TryRead method for RNS Binary Spec v1 deserialization with protobuf-style decoding
        /// </summary>
        private static void GenerateTryReadMethod(StringBuilder codeBuilder, INamedTypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            codeBuilder.AppendLine($"        /// <summary>");
            codeBuilder.AppendLine($"        /// Attempts to read a {typeSymbol.Name} instance from the specified buffer using RNS Binary Spec v1");
            codeBuilder.AppendLine($"        /// </summary>");
            codeBuilder.AppendLine($"        /// <param name=\"buffer\">The source buffer</param>");
            codeBuilder.AppendLine($"        /// <param name=\"readPacket\">The read packet</param>");
            codeBuilder.AppendLine($"        /// <param name=\"bytesRead\">The number of bytes read</param>");
            codeBuilder.AppendLine($"        /// <returns>True if successful, false otherwise</returns>");
            codeBuilder.AppendLine($"        public static bool TryRead(System.ReadOnlySpan<byte> buffer, out {typeSymbol.Name} readPacket, out int bytesRead)");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine($"            readPacket = new {typeSymbol.Name}();");
            codeBuilder.AppendLine("            bytesRead = 0;");
            codeBuilder.AppendLine("            var src = buffer;");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("            while (!src.IsEmpty)");
            codeBuilder.AppendLine("            {");
            codeBuilder.AppendLine("                if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var tag, out var tr)) return false;");
            codeBuilder.AppendLine("                src = src[tr..]; bytesRead += tr;");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("                var field = (int)(tag >> 3);");
            codeBuilder.AppendLine("                var wt = (int)(tag & 7);");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("                switch (field)");
            codeBuilder.AppendLine("                {");

            // Generate switch cases for each property
            var properties = GetSerializableProperties(typeSymbol);
            var fieldNumber = 1;
            foreach (var property in properties)
            {
                var propertyType = property.Type;
                var propertyName = property.Name;

                if (propertyType.SpecialType == SpecialType.System_String)
                {
                    codeBuilder.AppendLine($"                    case Keys.{propertyName} when wt == 2:");
                    codeBuilder.AppendLine("                    {");
                    codeBuilder.AppendLine("                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;");
                    codeBuilder.AppendLine("                        src = src[r..]; bytesRead += r;");
                    codeBuilder.AppendLine($"                        readPacket.{propertyName} = s;");
                    codeBuilder.AppendLine("                        break;");
                    codeBuilder.AppendLine("                    }");
                }
                else if (propertyType.SpecialType == SpecialType.System_Single)
                {
                    codeBuilder.AppendLine($"                    case Keys.{propertyName} when wt == 5:");
                    codeBuilder.AppendLine("                    {");
                    codeBuilder.AppendLine("                        if (!Serializer.Runtime.RnsCodec.TryReadFloat32(src, out var v, out var r)) return false;");
                    codeBuilder.AppendLine("                        src = src[r..]; bytesRead += r;");
                    codeBuilder.AppendLine($"                        readPacket.{propertyName} = v;");
                    codeBuilder.AppendLine("                        break;");
                    codeBuilder.AppendLine("                    }");
                }
                else if (propertyType.SpecialType == SpecialType.System_Int32)
                {
                    codeBuilder.AppendLine($"                    case Keys.{propertyName} when wt == 0:");
                    codeBuilder.AppendLine("                    {");
                    codeBuilder.AppendLine("                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var zz, out var r)) return false;");
                    codeBuilder.AppendLine("                        src = src[r..]; bytesRead += r;");
                    codeBuilder.AppendLine($"                        readPacket.{propertyName} = Serializer.Runtime.RnsCodec.UnZigZag32(zz);");
                    codeBuilder.AppendLine("                        break;");
                    codeBuilder.AppendLine("                    }");
                }
                else if (propertyType.SpecialType == SpecialType.System_Boolean)
                {
                    codeBuilder.AppendLine($"                    case Keys.{propertyName} when wt == 0:");
                    codeBuilder.AppendLine("                    {");
                    codeBuilder.AppendLine("                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var v, out var r)) return false;");
                    codeBuilder.AppendLine("                        src = src[r..]; bytesRead += r;");
                    codeBuilder.AppendLine($"                        readPacket.{propertyName} = v != 0;");
                    codeBuilder.AppendLine("                        break;");
                    codeBuilder.AppendLine("                    }");
                }

                fieldNumber++;
            }

            codeBuilder.AppendLine("                    default:");
            codeBuilder.AppendLine("                        // Skip unknown");
            codeBuilder.AppendLine("                        if (!SkipUnknown(ref src, ref bytesRead, wt)) return false;");
            codeBuilder.AppendLine("                        break;");
            codeBuilder.AppendLine("                }");
            codeBuilder.AppendLine("            }");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("            return true;");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();

            // Add the SkipUnknown helper method
            codeBuilder.AppendLine("        private static bool SkipUnknown(ref System.ReadOnlySpan<byte> src, ref int consumed, int wt)");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine("            switch (wt)");
            codeBuilder.AppendLine("            {");
            codeBuilder.AppendLine("                case 0:");
            codeBuilder.AppendLine("                    if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out _, out var r0)) return false;");
            codeBuilder.AppendLine("                    src = src[r0..]; consumed += r0; return true;");
            codeBuilder.AppendLine("                case 1:");
            codeBuilder.AppendLine("                    if (src.Length < 8) return false;");
            codeBuilder.AppendLine("                    src = src[8..]; consumed += 8; return true;");
            codeBuilder.AppendLine("                case 2:");
            codeBuilder.AppendLine("                    if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var len, out var r2)) return false;");
            codeBuilder.AppendLine("                    src = src[r2..];");
            codeBuilder.AppendLine("                    if (src.Length < len) return false;");
            codeBuilder.AppendLine("                    src = src[(int)len..]; consumed += r2 + (int)len; return true;");
            codeBuilder.AppendLine("                case 5:");
            codeBuilder.AppendLine("                    if (src.Length < 4) return false;");
            codeBuilder.AppendLine("                    src = src[4..]; consumed += 4; return true;");
            codeBuilder.AppendLine("                default: return false;");
            codeBuilder.AppendLine("            }");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();

            // Add the MakeTag helper method
            codeBuilder.AppendLine("        private static uint MakeTag(int f, int wt) => (uint)((f << 3) | wt);");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates the GetSerializedSize method
        /// </summary>
        private static void GenerateGetSerializedSizeMethod(StringBuilder codeBuilder, INamedTypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            codeBuilder.AppendLine($"        /// <summary>");
            codeBuilder.AppendLine($"        /// Gets the total number of bytes required to serialize this instance");
            codeBuilder.AppendLine($"        /// </summary>");
            codeBuilder.AppendLine($"        /// <returns>The serialized size in bytes</returns>");
            codeBuilder.AppendLine($"        public int GetSerializedSize()");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine("            var size = 0;");
            codeBuilder.AppendLine();

            // Calculate size for each property
            var properties = GetSerializableProperties(typeSymbol);
            foreach (var property in properties)
            {
                var propertyType = property.Type;
                var propertyName = property.Name;

                // Always count 2 bytes (key + flag), but only count value size if not default
                if (IsPrimitiveType(propertyType))
                {
                    var defaultValue = GetDefaultValue(propertyType);
                    codeBuilder.AppendLine($"            // {propertyName}: 2 bytes (key + flag) + value size (if not default)");
                    codeBuilder.AppendLine($"            size += 2; // Always count key and flag");
                    codeBuilder.AppendLine($"            if ({propertyName} != {defaultValue})");
                    codeBuilder.AppendLine("            {");
                    codeBuilder.AppendLine($"                size += {GetPrimitiveTypeSize(propertyType)};");
                    codeBuilder.AppendLine("            }");
                    codeBuilder.AppendLine();
                }
                else if (propertyType.SpecialType == SpecialType.System_String)
                {
                    codeBuilder.AppendLine($"            // {propertyName}: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)");
                    codeBuilder.AppendLine($"            size += 2; // Always count key and flag");
                    codeBuilder.AppendLine($"            if (!string.IsNullOrEmpty({propertyName}))");
                    codeBuilder.AppendLine("            {");
                    codeBuilder.AppendLine($"                size += 2 + System.Text.Encoding.UTF8.GetByteCount({propertyName} ?? \"\");");
                    codeBuilder.AppendLine("            }");
                    codeBuilder.AppendLine();
                }
                else if (propertyType.TypeKind == TypeKind.Enum)
                {
                    var underlyingType = ((INamedTypeSymbol)propertyType).EnumUnderlyingType;
                    if (underlyingType != null)
                    {
                        var defaultValue = GetDefaultValue(underlyingType);
                        codeBuilder.AppendLine($"            // {propertyName}: 2 bytes (key + flag) + value size (if not default)");
                        codeBuilder.AppendLine($"            size += 2; // Always count key and flag");
                        codeBuilder.AppendLine($"            if (({underlyingType.Name}){propertyName} != {defaultValue})");
                        codeBuilder.AppendLine("            {");
                        codeBuilder.AppendLine($"                size += {GetPrimitiveTypeSize(underlyingType)};");
                        codeBuilder.AppendLine("            }");
                        codeBuilder.AppendLine();
                    }
                }
            }

            // Add 1 byte for terminator
            codeBuilder.AppendLine("            // 1 byte for terminator");
            codeBuilder.AppendLine("            size += 1;");
            codeBuilder.AppendLine();

            codeBuilder.AppendLine("            return size;");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Generates the ToBytes convenience method
        /// </summary>
        private static void GenerateToBytesMethod(StringBuilder codeBuilder, INamedTypeSymbol typeSymbol)
        {
            codeBuilder.AppendLine($"        /// <summary>");
            codeBuilder.AppendLine($"        /// Convenience method for array-based serialization");
            codeBuilder.AppendLine($"        /// </summary>");
            codeBuilder.AppendLine($"        /// <param name=\"buffer\">The destination buffer</param>");
            codeBuilder.AppendLine($"        /// <returns>The number of bytes written</returns>");
            codeBuilder.AppendLine($"        public int ToBytes(byte[] buffer)");
            codeBuilder.AppendLine("        {");
            codeBuilder.AppendLine("            return Write(buffer);");
            codeBuilder.AppendLine("        }");
            codeBuilder.AppendLine();
        }

        /// <summary>
        /// Gets serializable properties from a type
        /// </summary>
        private static IEnumerable<IPropertySymbol> GetSerializableProperties(INamedTypeSymbol typeSymbol)
        {
            return typeSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.GetMethod != null && p.SetMethod != null && !p.IsStatic && p.DeclaredAccessibility == Accessibility.Public);
        }

        /// <summary>
        /// Checks if a type is a primitive type
        /// </summary>
        private static bool IsPrimitiveType(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => true,
                SpecialType.System_Byte => true,
                SpecialType.System_SByte => true,
                SpecialType.System_Int16 => true,
                SpecialType.System_UInt16 => true,
                SpecialType.System_Int32 => true,
                SpecialType.System_UInt32 => true,
                SpecialType.System_Int64 => true,
                SpecialType.System_UInt64 => true,
                SpecialType.System_Single => true,
                SpecialType.System_Double => true,
                _ => false
            };
        }

        /// <summary>
        /// Gets the write method name for a primitive type
        /// </summary>
        private static string GetPrimitiveWriteMethod(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => "WriteBoolean",
                SpecialType.System_Byte => "WriteByte",
                SpecialType.System_SByte => "WriteSByte",
                SpecialType.System_Int16 => "WriteInt16",
                SpecialType.System_UInt16 => "WriteUInt16",
                SpecialType.System_Int32 => "WriteInt32",
                SpecialType.System_UInt32 => "WriteUInt32",
                SpecialType.System_Int64 => "WriteInt64",
                SpecialType.System_UInt64 => "WriteUInt64",
                SpecialType.System_Single => "WriteSingle",
                SpecialType.System_Double => "WriteDouble",
                _ => "WriteByte" // fallback
            };
        }

        /// <summary>
        /// Gets the read method name for a primitive type
        /// </summary>
        private static string GetPrimitiveReadMethod(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => "ReadBoolean",
                SpecialType.System_Byte => "ReadByte",
                SpecialType.System_SByte => "ReadSByte",
                SpecialType.System_Int16 => "ReadInt16",
                SpecialType.System_UInt16 => "ReadUInt16",
                SpecialType.System_Int32 => "ReadInt32",
                SpecialType.System_UInt32 => "ReadUInt32",
                SpecialType.System_Int64 => "ReadInt64",
                SpecialType.System_UInt64 => "ReadUInt64",
                SpecialType.System_Single => "ReadSingle",
                SpecialType.System_Double => "ReadDouble",
                _ => "ReadByte" // fallback
            };
        }

        /// <summary>
        /// Gets the size in bytes for a primitive type
        /// </summary>
        private static int GetPrimitiveTypeSize(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => 1,
                SpecialType.System_Byte => 1,
                SpecialType.System_SByte => 1,
                SpecialType.System_Int16 => 2,
                SpecialType.System_UInt16 => 2,
                SpecialType.System_Int32 => 4,
                SpecialType.System_UInt32 => 4,
                SpecialType.System_Int64 => 8,
                SpecialType.System_UInt64 => 8,
                SpecialType.System_Single => 4,
                SpecialType.System_Double => 8,
                _ => 1 // fallback
            };
        }

        /// <summary>
        /// Generates a packet type ID based on the type name
        /// </summary>
        private static int GetPacketTypeId(string typeName)
        {
            // Simple hash-based ID generation for now
            var hash = 0;
            foreach (var c in typeName)
            {
                hash = ((hash << 5) + hash) + c;
            }
            return Math.Abs(hash) % 10000 + 1000; // Ensure 4-digit positive number
        }

        /// <summary>
        /// Gets the default value for a primitive type
        /// </summary>
        private static string GetDefaultValue(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => "false",
                SpecialType.System_Byte => "(byte)0",
                SpecialType.System_SByte => "(sbyte)0",
                SpecialType.System_Int16 => "(short)0",
                SpecialType.System_UInt16 => "(ushort)0",
                SpecialType.System_Int32 => "0",
                SpecialType.System_UInt32 => "0U",
                SpecialType.System_Int64 => "0L",
                SpecialType.System_UInt64 => "0UL",
                SpecialType.System_Single => "0f",
                SpecialType.System_Double => "0d",
                _ => "0" // fallback to int 0
            };
        }

        /// <summary>
        /// Gets the property key for a property
        /// </summary>
        private static int GetPropertyKey(IPropertySymbol property, IEnumerable<IPropertySymbol> allProperties)
        {
            // Simple index-based key generation
            var index = 0;
            foreach (var p in allProperties)
            {
                if (SymbolEqualityComparer.Default.Equals(p, property))
                {
                    return index;
                }
                index++;
            }
            return -1; // Should not happen if properties are unique
        }
    }
}
