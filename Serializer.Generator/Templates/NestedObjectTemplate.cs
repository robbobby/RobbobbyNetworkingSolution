using System;

namespace Serializer.Generator.Templates
{
    public static class NestedObjectTemplate
    {
        public static void Read<T>(ref int consumed, ReadOnlySpan<byte> buffer, out T readPacket) where T : IRnsPacketField
        {
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var NestedFieldLen);
            if (T.TryRead<T>(buffer.Slice(consumed, NestedFieldLen), ref consumed, out var NestedFieldValue))
            {
                readPacket = NestedFieldValue;
            }
            else
            {
                readPacket = default!;
            }
            consumed += NestedFieldLen;
        }

        public static void Write<T>(ref int used, Span<byte> buffer, T writePacket, ushort key) where T : IRnsPacketField
        {
            if (writePacket != null)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                var nestedBuffer = new byte[1024];
                if (writePacket.Write(nestedBuffer, out int nestedLength))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)nestedLength);
                    nestedBuffer.AsSpan(0, nestedLength).CopyTo(buffer.Slice(used));
                    used += nestedLength;
                }
            }
        }
    }
}
