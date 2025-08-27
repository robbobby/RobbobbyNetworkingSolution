using System;

namespace Serializer.Generator.Templates
{
    public static class UInt64Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out ulong readPacket)
        {
            consumed += RndCodec.ReadUInt64(buffer.Slice(consumed), out var UInt64FieldValue);
            readPacket = UInt64FieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, ulong writePacket, ushort key)
        {
            if (!(writePacket == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt64(buffer.Slice(used), writePacket);
            }
        }
    }
}
