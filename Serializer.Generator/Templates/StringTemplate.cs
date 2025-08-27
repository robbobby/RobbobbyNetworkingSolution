using System;

namespace Serializer.Generator.Templates
{
    public static class StringTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out string readPacket)
        {
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var StringFieldLen);
            consumed += RndCodec.ReadString(buffer.Slice(consumed), out var StringFieldValue);
            readPacket = StringFieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, string writePacket, ushort key)
        {
            if (!string.IsNullOrEmpty(writePacket))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                var StringFieldBytes = System.Text.Encoding.UTF8.GetByteCount(writePacket);
                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)(2 + StringFieldBytes));
                used += RndCodec.WriteString(buffer.Slice(used), writePacket);
            }
        }
    }
}
