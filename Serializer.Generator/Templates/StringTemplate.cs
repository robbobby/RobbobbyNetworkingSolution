using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serializer.Generator.Templates
{
    public class StringTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, HereForCompileReasonsPacket PACKET_NAME)
        {
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE; // This gets replaced during code generation
        }

        public static void Write(ref int used, Span<byte> buffer, string value, ushort key)
        {
            if (!string.IsNullOrEmpty(value))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                var StringFieldBytes = System.Text.Encoding.UTF8.GetByteCount(value);
                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)(2 + StringFieldBytes));
                used += RndCodec.WriteString(buffer.Slice(used), value);
            }
        }

        public static string GenerateReadCode(string propertyName, string packetName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<StringTemplate>(compilation, nameof(Read));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<StringTemplate>(nameof(Read));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", $"{propertyName}Value")
                .Replace("PROPERTY_KEY", propertyName)
                .Replace("PACKET_NAME", packetName);
        }

        public static string GenerateWriteCode(string propertyName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<StringTemplate>(compilation, nameof(Write));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<StringTemplate>(nameof(Write));
            }

            return methodBody
                .Replace("value", propertyName)
                .Replace("key", $"Keys.{propertyName}");
        }
    }
}
