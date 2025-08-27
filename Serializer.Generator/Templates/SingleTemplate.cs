using System;

namespace Serializer.Generator.Templates
{
    public static class SingleTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out float readPacket)
        {
            consumed += RndCodec.ReadSingle(buffer.Slice(consumed), out var SingleFieldValue);
            readPacket = SingleFieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, float writePacket, ushort key)
        {
            if (!(writePacket == 0f))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteSingle(buffer.Slice(used), writePacket);
            }
        }
    }
}
