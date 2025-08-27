using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serializer.Generator.Templates
{
    public class Int16Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, HereForCompileReasonsPacket PACKET_NAME)
        {
            consumed += RndCodec.ReadInt16(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE; // This gets replaced during code generation
        }

        public static void Write(ref int used, Span<byte> buffer, short PROPERTY_VALUE, ushort key)
        {
            if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt16(buffer.Slice(used), PROPERTY_VALUE);
            }
        }

        public static string GenerateReadCode(string propertyName, string packetName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<Int16Template>(compilation, nameof(Read));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<Int16Template>(nameof(Read));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", $"{propertyName}Value")
                .Replace("PROPERTY_KEY", propertyName)
                .Replace("PACKET_NAME", packetName);
        }

        public static string GenerateWriteCode(string propertyName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<Int16Template>(compilation, nameof(Write));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<Int16Template>(nameof(Write));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", propertyName)
                .Replace("key", $"Keys.{propertyName}");
        }
    }
}
