using System;

namespace Serializer.Generator.Templates
{
    public static class UInt32Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out uint readPacket)
        {
            consumed += RndCodec.ReadUInt32(buffer.Slice(consumed), out var UInt32FieldValue);
            readPacket = UInt32FieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, uint writePacket, ushort key)
        {
            if (!(writePacket == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt32(buffer.Slice(used), writePacket);
            }
        }
    }
}
