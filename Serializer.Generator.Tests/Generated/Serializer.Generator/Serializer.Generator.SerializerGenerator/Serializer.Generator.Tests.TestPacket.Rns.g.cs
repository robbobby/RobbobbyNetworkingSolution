using System;
using System.Collections.Generic;
using Serializer.Runtime;
using Serializer.Generator.Runtime;

namespace Serializer.Generator.Tests
{
    public partial class TestPacket
    {
        public static class Keys
        {
            public const ushort PlayerName = 1;
            public const ushort X = 2;
            public const ushort Y = 3;
            public const ushort Health = 4;
            public const ushort IsAlive = 5;
            public const ushort FieldPacket = 6;
            public const ushort FieldPacketList = 7;
        }

        static TestPacket()
        {
            RnsKeyRegistry.Register<TestPacket>(new Dictionary<ushort, string>
            {
                { Keys.PlayerName, nameof(PlayerName) },
                { Keys.X, nameof(X) },
                { Keys.Y, nameof(Y) },
                { Keys.Health, nameof(Health) },
                { Keys.IsAlive, nameof(IsAlive) },
                { Keys.FieldPacket, nameof(FieldPacket) },
                { Keys.FieldPacketList, nameof(FieldPacketList) },
            });
        }

        public bool Write(Span<byte> buffer, out int bytesWritten)
        {
            bytesWritten = 0;
            var used = 0;

            try
            {
                // Write PlayerName
                if (!(string.IsNullOrEmpty(PlayerName)))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.PlayerName);
                    used += RndCodec.WriteString(buffer.Slice(used), PlayerName);
                }

                // Write X
                if (!(X == 0f))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.X);
                    used += RndCodec.WriteUInt16(buffer.Slice(used), 4); // Length
                    used += RndCodec.WriteSingle(buffer.Slice(used), X);
                }

                // Write Y
                if (!(Y == 0f))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.Y);
                    used += RndCodec.WriteUInt16(buffer.Slice(used), 4); // Length
                    used += RndCodec.WriteSingle(buffer.Slice(used), Y);
                }

                // Write Health
                if (!(Health == 0))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.Health);
                    used += RndCodec.WriteUInt16(buffer.Slice(used), 4); // Length
                    used += RndCodec.WriteInt32(buffer.Slice(used), Health);
                }

                // Write IsAlive
                if (!(IsAlive == false))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.IsAlive);
                    used += RndCodec.WriteUInt16(buffer.Slice(used), 1); // Length
                    used += RndCodec.WriteBoolean(buffer.Slice(used), IsAlive);
                }

                // Write FieldPacket
                if (!(FieldPacket == null))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.FieldPacket);
                    // Write nested object as length-prefixed payload
                    var nestedBuffer = new byte[1024]; // Temporary buffer
                    if (FieldPacket!.Write(nestedBuffer, out var nestedLength))
                    {
                        used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)nestedLength);
                        nestedBuffer.AsSpan(0, nestedLength).CopyTo(buffer.Slice(used));
                        used += nestedLength;
                    }
                }

                // Write FieldPacketList
                if (!(FieldPacketList == null || FieldPacketList.Length == 0))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.FieldPacketList);
                    // Write array count
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)FieldPacketList.Length);
                    foreach (var item in FieldPacketList)
                    {
                        var itemBuffer = new byte[1024]; // Temporary buffer
                        if (item.Write(itemBuffer, out var itemLength))
                        {
                            used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)itemLength);
                            itemBuffer.AsSpan(0, itemLength).CopyTo(buffer.Slice(used));
                            used += itemLength;
                        }
                    }
                }

                bytesWritten = used;
                return true;
            }
            catch
            {
                bytesWritten = 0;
                return false;
            }
        }

        public static bool TryRead(ReadOnlySpan<byte> buffer, out TestPacket readPacket, out int bytesRead)
        {
            readPacket = new TestPacket();
            bytesRead = 0;

            try
            {
                var consumed = 0;

                while (consumed < buffer.Length)
                {
                    if (consumed + 4 > buffer.Length) break; // Need at least key + length

                    consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var key);
                    consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var len);

                    if (consumed + len > buffer.Length) return false; // Malformed

                    switch (key)
                    {
                        case Keys.PlayerName:
                            consumed += RndCodec.ReadString(buffer.Slice(consumed), out readPacket.PlayerName);
                            break;
                        case Keys.X:
                            if (len != 4) return false;
                            consumed += RndCodec.ReadSingle(buffer.Slice(consumed), out readPacket.X);
                            break;
                        case Keys.Y:
                            if (len != 4) return false;
                            consumed += RndCodec.ReadSingle(buffer.Slice(consumed), out readPacket.Y);
                            break;
                        case Keys.Health:
                            if (len != 4) return false;
                            consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out readPacket.Health);
                            break;
                        case Keys.IsAlive:
                            if (len != 1) return false;
                            consumed += RndCodec.ReadBooleanStrict(buffer.Slice(consumed), out readPacket.IsAlive);
                            break;
                        case Keys.FieldPacket:
                            if (Serializer.Generator.Tests.TestFieldPacket?.TryRead(buffer.Slice(consumed, len), out var FieldPacketValue, out var FieldPacketRead))
                            {
                                readPacket.FieldPacket = FieldPacketValue;
                            }
                            consumed += len;
                            break;
                        case Keys.FieldPacketList:
                            var arrayStart = consumed;
                            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var count);
                            var FieldPacketListList = new List<Serializer.Generator.Tests.TestFieldPacket>();
                            for (int i = 0; i < count; i++)
                            {
                                consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var itemLen);
                                if (Serializer.Generator.Tests.TestFieldPacket.TryRead(buffer.Slice(consumed, itemLen), out var item, out var itemRead))
                                {
                                    FieldPacketListList.Add(item);
                                }
                                consumed += itemLen;
                            }
                            readPacket.FieldPacketList = FieldPacketListList.ToArray();
                            break;
                        default:
                            // Unknown key - skip len bytes
                            consumed += len;
                            break;
                    }
                }

                bytesRead = consumed;
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
