using System;
using Microsoft.CodeAnalysis;

namespace Serializer.Generator.Templates.ComplexTypes
{
    public class NestedObjectTemplate
    {
        public static void Read<T>(ref int consumed, ReadOnlySpan<byte> buffer, HereForCompileReasonsPacket PACKET_NAME) where T : IRnsPacketField
        {
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var NestedFieldLen);
            if (PlaceHolderReadable.TryRead<T>(buffer.Slice(consumed, NestedFieldLen), ref consumed, out var NestedFieldValue))
            {
                PACKET_NAME.PROPERTY_KEY = NestedFieldValue; // This gets replaced during code generation
            }
            else
            {
                PACKET_NAME.PROPERTY_KEY = default(T); // This gets replaced during code generation
            }
            consumed += NestedFieldLen;
        }

        public static void Write<T>(ref int used, Span<byte> buffer, T PROPERTY_VALUE, ushort key) where T : IRnsPacketField
        {
            if (PROPERTY_VALUE != null)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                var nestedBuffer = new byte[1024];
                if (PROPERTY_VALUE.Write(nestedBuffer, out int nestedLength))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)nestedLength);
                    nestedBuffer.AsSpan(0, nestedLength).CopyTo(buffer.Slice(used));
                    used += nestedLength;
                }
            }
        }

        public static string GenerateReadCode(string propertyName, string packetName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<NestedObjectTemplate>(compilation, nameof(Read));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<NestedObjectTemplate>(nameof(Read));
            }

            return methodBody
                .Replace("PROPERTY_KEY", propertyName)
                .Replace("PACKET_NAME", packetName);
        }

        public static string GenerateWriteCode(string propertyName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<NestedObjectTemplate>(compilation, nameof(Write));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<NestedObjectTemplate>(nameof(Write));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", propertyName)
                .Replace("key", $"Keys.{propertyName}");
        }
    }
}
