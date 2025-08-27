using System;

namespace Serializer.Generator.Templates
{
    public static class ByteTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out byte readPacket)
        {
            consumed += RndCodec.ReadByte(buffer.Slice(consumed), out var ByteFieldValue);
            readPacket = ByteFieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, byte writePacket, ushort key)
        {
            if (!(writePacket == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteByte(buffer.Slice(used), writePacket);
            }
        }
    }
}
