using System;

namespace Serializer.Generator.Templates
{
    public static class SByteTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out sbyte readPacket)
        {
            consumed += RndCodec.ReadSByte(buffer.Slice(consumed), out var SByteFieldValue);
            readPacket = SByteFieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, sbyte writePacket, ushort key)
        {
            if (!(writePacket == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteSByte(buffer.Slice(used), writePacket);
            }
        }
    }
}
