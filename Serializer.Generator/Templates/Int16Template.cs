using System;

namespace Serializer.Generator.Templates
{
    public static class Int16Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out short readPacket)
        {
            consumed += RndCodec.ReadInt16(buffer.Slice(consumed), out var Int16FieldValue);
            readPacket = Int16FieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, short writePacket, ushort key)
        {
            if (!(writePacket == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt16(buffer.Slice(used), writePacket);
            }
        }
    }
}
