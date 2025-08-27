using System;

namespace Serializer.Generator.Templates
{
    public static class UInt16Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out ushort readPacket)
        {
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var UInt16FieldValue);
            readPacket = UInt16FieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, ushort writePacket, ushort key)
        {
            if (!(writePacket == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt16(buffer.Slice(used), writePacket);
            }
        }
    }
}
