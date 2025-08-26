// Generated serialization code
#nullable enable
using System;
using System.Collections.Generic;
using Serializer.Runtime;
using Serializer.Generator.Runtime;

namespace Serializer.Generator.Tests
{
    public partial class TestFieldPacket2
    {
        public static class Keys
        {
            public const ushort PlayerName = 1;
            public const ushort X = 2;
            public const ushort Y = 3;
            public const ushort Health = 4;
        }

        static TestFieldPacket2()
        {
            RnsKeyRegistry.Register<TestFieldPacket2>(new Dictionary<ushort, string>
            {
                { Keys.PlayerName, nameof(PlayerName) },
                { Keys.X, nameof(X) },
                { Keys.Y, nameof(Y) },
                { Keys.Health, nameof(Health) },
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

                bytesWritten = used;
                return true;
            }
            catch
            {
                bytesWritten = 0;
                return false;
            }
        }

        public static bool TryRead(ReadOnlySpan<byte> buffer, out TestFieldPacket2 readPacket, out int bytesRead)
        {
            readPacket = new TestFieldPacket2();
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
