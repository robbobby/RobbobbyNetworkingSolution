using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serializer.Generator.Templates
{
    public class ArrayTemplate
    {
        public static void Read<T>(ref int consumed, ReadOnlySpan<byte> buffer, HereForCompileReasonsPacket PACKET_NAME) where T : IRnsPacketField
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

            PACKET_NAME.PROPERTY_KEY = ArrayFieldList.ToArray(); // This gets replaced during code generation
        }

        public static void Write<T>(ref int used, Span<byte> buffer, T[] PROPERTY_VALUE, ushort key) where T : IRnsPacketField
        {
            if (PROPERTY_VALUE != null && PROPERTY_VALUE.Length > 0)
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);

                var ArrayFieldTotalLength = 2;
                foreach (var item in PROPERTY_VALUE)
                {
                    var itemBuffer = new byte[1024];
                    if (item.Write(itemBuffer, out int itemLength))
                    {
                        ArrayFieldTotalLength += 2 + itemLength;
                    }
                }

                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)ArrayFieldTotalLength);
                used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)PROPERTY_VALUE.Length);

                foreach (var item in PROPERTY_VALUE)
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

        public static string GenerateReadCode(string propertyName, string packetName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<ArrayTemplate>(compilation, nameof(Read));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<ArrayTemplate>(nameof(Read));
            }

            return methodBody
                .Replace("PROPERTY_KEY", propertyName)
                .Replace("PACKET_NAME", packetName);
        }

        public static string GenerateWriteCode(string propertyName, Compilation compilation = null)
        {
            // Try Roslyn analysis first
            var methodBody = Helpers.ExtractMethodBody<ArrayTemplate>(compilation, nameof(Write));

            // If that fails, use the fallback approach
            if (string.IsNullOrEmpty(methodBody))
            {
                methodBody = Helpers.ExtractMethodBodyFromSource<ArrayTemplate>(nameof(Write));
            }

            return methodBody
                .Replace("PROPERTY_VALUE", propertyName)
                .Replace("key", $"Keys.{propertyName}");
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
