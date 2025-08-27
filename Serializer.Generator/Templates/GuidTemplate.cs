using System;

namespace Serializer.Generator.Templates
{
    public static class GuidTemplate
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, out Guid readPacket)
        {
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var GuidFieldLen);
            consumed += RndCodec.ReadGuid(buffer.Slice(consumed), out var GuidFieldValue);
            readPacket = GuidFieldValue;
        }

        public static void Write(ref int used, Span<byte> buffer, Guid writePacket, ushort key)
        {
            if (writePacket != default)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteUInt16(buffer.Slice(used), 16);
                used += RndCodec.WriteGuid(buffer.Slice(used), writePacket);
            }
        }
    }
}
