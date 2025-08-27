using System;

namespace Serializer.Generator.Templates
{
    public static class Int32Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out int readPacket)
        {
            consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var Int32FieldValue);
            readPacket = Int32FieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, int writePacket, ushort key)
        {
            if (!(writePacket == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), writePacket);
            }
        }
    }
}
