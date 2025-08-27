using System;
using Microsoft.CodeAnalysis;

namespace Serializer.Generator.Templates.PrimitiveTypes
{
    public class GuidTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, HereForCompileReasonsPacket PACKET_NAME)
        {
            consumed += RndCodec.ReadGuid(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE; // This gets replaced during code generation
        }

        public static void Write(ref int used, Span<byte> buffer, Guid PROPERTY_VALUE, ushort key)
        {
            if (PROPERTY_VALUE != Guid.Empty)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt16(buffer.Slice(used), 16);
                used += RndCodec.WriteGuid(buffer.Slice(used), PROPERTY_VALUE);
            }
        }

        public static string GenerateReadCode(string propertyName, string packetName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<GuidTemplate>(compilation, nameof(Read));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<GuidTemplate>(nameof(Read));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", $"{propertyName}Value")
                .Replace("PROPERTY_KEY", propertyName)
                .Replace("PACKET_NAME", packetName);
        }

        public static string GenerateWriteCode(string propertyName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<GuidTemplate>(compilation, nameof(Write));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<GuidTemplate>(nameof(Write));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", propertyName)
                .Replace("key", $"Keys.{propertyName}");
        }
    }
}
