// Generated serialization code
#nullable enable
using System;
using System.Collections.Generic;
using Serializer.Runtime;
using Serializer.Generator.Runtime;

namespace Serializer.Generator.Tests
{
    public partial class SimpleTestPacket
    {
        public static class Keys
        {
            public const ushort Name = 1;
        }

        static SimpleTestPacket()
        {
            RnsKeyRegistry.Register<SimpleTestPacket>(new Dictionary<ushort, string>
            {
                { Keys.Name, nameof(Name) },
            });
        }

        public bool Write(Span<byte> buffer, out int bytesWritten)
        {
            bytesWritten = 0;
            var used = 0;

            try
            {
                // Write Name
                if (!(string.IsNullOrEmpty(Name)))
                {
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.Name);
                    used += RndCodec.WriteString(buffer.Slice(used), Name);
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

        public static bool TryRead(ReadOnlySpan<byte> buffer, out SimpleTestPacket readPacket, out int bytesRead)
        {
            readPacket = new SimpleTestPacket();
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
                        case Keys.Name:
                            consumed += RndCodec.ReadString(buffer.Slice(consumed), out var NameValue);
                            readPacket.Name = NameValue;
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
