// Generated serialization code
#nullable enable
using System;
using System.Collections.Generic;
using Serializer.Runtime;
using Serializer.Generator.Runtime;

namespace Serializer.Generator.Tests
{
    public partial class TestFieldPacket
    {
        public static class Keys
        {
            public const ushort PlayerName = 1;
            public const ushort X = 2;
            public const ushort Y = 3;
            public const ushort Health = 4;
            public const ushort FieldPacket2 = 5;
        }

        static TestFieldPacket()
        {
            RnsKeyRegistry.Register<TestFieldPacket>(new Dictionary<ushort, string>
            {
                { Keys.PlayerName, nameof(PlayerName) },
                { Keys.X, nameof(X) },
                { Keys.Y, nameof(Y) },
                { Keys.Health, nameof(Health) },
                { Keys.FieldPacket2, nameof(FieldPacket2) },
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

                // Write FieldPacket2
                if (!(FieldPacket2 == null))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.FieldPacket2);
                    // Write nested object as length-prefixed payload
                    var nestedBuffer = new byte[1024]; // Temporary buffer
                    if (FieldPacket2!.Write(nestedBuffer, out var nestedLength))
                    {
                        used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)nestedLength);
                        nestedBuffer.AsSpan(0, nestedLength).CopyTo(buffer.Slice(used));
                        used += nestedLength;
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

        public static bool TryRead(ReadOnlySpan<byte> buffer, out TestFieldPacket readPacket, out int bytesRead)
        {
            readPacket = new TestFieldPacket();
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
                            consumed += RndCodec.ReadString(buffer.Slice(consumed), out var PlayerNameValue);
                            readPacket.PlayerName = PlayerNameValue;
                            break;
                        case Keys.X:
                            if (len != 4) return false;
                            consumed += RndCodec.ReadSingle(buffer.Slice(consumed), out var XValue);
                            readPacket.X = XValue;
                            break;
                        case Keys.Y:
                            if (len != 4) return false;
                            consumed += RndCodec.ReadSingle(buffer.Slice(consumed), out var YValue);
                            readPacket.Y = YValue;
                            break;
                        case Keys.Health:
                            if (len != 4) return false;
                            consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var HealthValue);
                            readPacket.Health = HealthValue;
                            break;
                        case Keys.FieldPacket2:
                            if (Serializer.Generator.Tests.TestFieldPacket2?.TryRead(buffer.Slice(consumed, len), out var FieldPacket2Value, out var FieldPacket2Read))
                            {
                                readPacket.FieldPacket2 = FieldPacket2Value;
                            }
                            consumed += len;
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
