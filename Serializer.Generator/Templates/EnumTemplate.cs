using System;

namespace Serializer.Generator.Templates
{
    public static class EnumTemplate
    {
        public static void Read<T>(ref int consumed, ReadOnlySpan<byte> buffer, out T readPacket) where T : struct, Enum
        {
            consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var EnumFieldValue);
            readPacket = (T)(object)EnumFieldValue;
        }

        public static void Write<T>(ref int used, Span<byte> buffer, T writePacket, ushort key) where T : struct, Enum
        {
            if (!writePacket.Equals(default(T)))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), (int)(object)writePacket);
            }
        }
    }
}
