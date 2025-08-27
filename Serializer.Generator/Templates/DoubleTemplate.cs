using System;

namespace Serializer.Generator.Templates
{
    public static class DoubleTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out double readPacket)
        {
            consumed += RndCodec.ReadDouble(buffer.Slice(consumed), out var DoubleFieldValue);
            readPacket = DoubleFieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, double writePacket, ushort key)
        {
            if (!(writePacket == 0.0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteDouble(buffer.Slice(used), writePacket);
            }
        }
    }
}
