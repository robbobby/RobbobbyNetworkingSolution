using System;
using Microsoft.CodeAnalysis;
namespace Serializer.Generator.Templates.PrimitiveTypes
{
    public class UInt64Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, HereForCompileReasonsPacket PACKET_NAME)
        {
            consumed += RndCodec.ReadUInt64(buffer.Slice(consumed), out var PROPERTY_VALUE);
            PACKET_NAME.PROPERTY_KEY = PROPERTY_VALUE; // This gets replaced during code generation
        }
        public static void Write(ref int used, Span<byte> buffer, ulong PROPERTY_VALUE, ushort key)
        {
            if (PROPERTY_VALUE != 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt64(buffer.Slice(used), PROPERTY_VALUE);
            }
        }
        public static string GenerateReadCode(string propertyName, string packetName, Compilation compilation = null)
        {
            // Use Roslyn analysis to extract the method body
            var methodBody = Helpers.ExtractMethodBody<UInt64Template>(compilation, nameof(Read));
            return methodBody
                .Replace("PROPERTY_VALUE", $"{propertyName}Value")
                .Replace("PROPERTY_KEY", propertyName)
                .Replace("PACKET_NAME", packetName);
        }
        public static string GenerateWriteCode(string propertyName, Compilation compilation = null)
        {
            // Use Roslyn analysis to extract the method body
            var methodBody = Helpers.ExtractMethodBody<UInt64Template>(compilation, nameof(Write));
            return methodBody
                .Replace("PROPERTY_VALUE", propertyName)
                .Replace("key", $"Keys.{propertyName}");
        }
    }
}
