using System;

namespace Serializer.Generator.Templates
{
    public static class Int64Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out long readPacket)
        {
            consumed += RndCodec.ReadInt64(buffer.Slice(consumed), out var Int64FieldValue);
            readPacket = Int64FieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, long writePacket, ushort key)
        {
            if (!(writePacket == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt64(buffer.Slice(used), writePacket);
            }
        }
    }
}
