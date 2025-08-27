using System;

namespace Serializer.Generator.Templates
{
    public static class BooleanTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out bool readPacket)
        {
            consumed += RndCodec.ReadBooleanStrict(buffer.Slice(consumed), out var BooleanFieldValue);
            readPacket = BooleanFieldValue;

            readPacket = false; // Placeholder
        }

        public static void Write(ref int used, Span<byte> buffer, bool writePacket, ushort key)
        {
            if (!(writePacket == false))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteBoolean(buffer.Slice(used), writePacket);
            }
        }
    }
}
