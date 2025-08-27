using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serializer.Generator.Templates
{
    public class EnumTemplate
    {
        public static void Read<T>(ref int consumed, ReadOnlySpan<byte> buffer, HereForCompileReasonsPacket PACKET_NAME) where T : struct, Enum
        {
            consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = (T)(object)PROPERTY_VALUE; // This gets replaced during code generation
        }

        public static void Write<T>(ref int used, Span<byte> buffer, T PROPERTY_VALUE, ushort key) where T : struct, Enum
        {
            if (!PROPERTY_VALUE.Equals(default(T)))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), (int)(object)PROPERTY_VALUE);
            }
        }

        public static string GenerateReadCode(string propertyName, string packetName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<EnumTemplate>(compilation, nameof(Read));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<EnumTemplate>(nameof(Read));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", $"{propertyName}Value")
                .Replace("PROPERTY_KEY", propertyName)
                .Replace("PACKET_NAME", packetName);
        }

        public static string GenerateWriteCode(string propertyName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<EnumTemplate>(compilation, nameof(Write));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<EnumTemplate>(nameof(Write));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", propertyName)
                .Replace("key", $"Keys.{propertyName}");
        }
    }
}
