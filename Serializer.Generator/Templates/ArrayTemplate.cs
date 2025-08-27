using System;
using System.Collections.Generic;

namespace Serializer.Generator.Templates
{
    public static class ArrayTemplate
    {
        public static void Read<T>(ref int consumed, ReadOnlySpan<byte> buffer, out T[] readPacket) where T : IRnsPacketField
        {
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var ArrayFieldTotalLen);
            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var count);

            var ArrayFieldList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var itemLen);
                if (PlaceHolderReadable.TryRead<T>(buffer.Slice(consumed, itemLen), ref consumed, out var item))
                {
                    ArrayFieldList.Add(item);
                }
                consumed += itemLen;
            }

            readPacket = ArrayFieldList.ToArray();
        }

        public static void Write<T>(ref int used, Span<byte> buffer, T[] writePacket, ushort key) where T : IRnsPacketField
        {
            if (writePacket != null && writePacket.Length > 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);

                var ArrayFieldTotalLength = 2;
                foreach (var item in writePacket)
                {
                    var itemBuffer = new byte[1024];
                    if (item.Write(itemBuffer, out int itemLength))
                    {
                        ArrayFieldTotalLength += 2 + itemLength;
                    }
                }

                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)ArrayFieldTotalLength);
                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)writePacket.Length);

                foreach (var item in writePacket)
                {
                    var itemBuffer = new byte[1024];
                    if (item.Write(itemBuffer, out int itemLength))
                    {
                        used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)itemLength);
                        itemBuffer.AsSpan(0, itemLength).CopyTo(buffer.Slice(used));
                        used += itemLength;
                    }
                }
            }
        }
    }

    public static class PlaceHolderReadable
    {
        public static bool TryRead<T>(ReadOnlySpan<byte> buffer, ref int consumed, out T readPacket) where T : IRnsPacketField
        {
            readPacket = Activator.CreateInstance<T>();
            return true;
        }
    }
}
