using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Interface for packet keys of TestPacket
    /// </summary>
    public interface ITestPacketKeys
    {
        int PacketTypeId { get; }
        string PacketName { get; }
        int PacketVersion { get; }
        Type PacketType { get; }
    }

    /// <summary>
    /// Packet keys for TestPacket
    /// </summary>
    public class TestPacketKeys : ITestPacketKeys
    {
        public int PacketTypeId => 3984;
        public string PacketName => "TestPacket";
        public int PacketVersion => 1;

        /// <summary>
        /// Runtime-accessible metadata for TestPacket
        /// </summary>
        public Type PacketType => typeof(TestPacket);

        /// <summary>
        /// Singleton instance for RnsKeys property access
        /// </summary>
        public static readonly ITestPacketKeys Instance = new TestPacketKeys();
    }

    /// <summary>
    /// Generated partial class for TestPacket with serialization support
    /// </summary>
    public partial class TestPacket
    {
        /// <summary>
        /// Static access to packet keys and metadata
        /// </summary>
        public static ITestPacketKeys RnsKeys => TestPacketKeys.Instance;

        /// <summary>
        /// Alternative access to packet keys (for polymorphic scenarios)
        /// </summary>
        public static ITestPacketKeys Keys => TestPacketKeys.Instance;

        /// <summary>
        /// Writes the current instance to the specified buffer
        /// </summary>
        /// <param name="destination">The destination buffer</param>
        /// <returns>The number of bytes written</returns>
        public int Write(System.Span<byte> destination)
        {
            var offset = 0;

            // Write Id key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), Id);

            // Write PlayerName key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), PlayerName);

            // Write X key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), X);

            // Write Y key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 3);
            offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), Y);

            // Write Health key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), Health);

            // Write IsAlive key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 5);
            offset += Serializer.Runtime.BinarySerializer.WriteBoolean(destination.Slice(offset), IsAlive);

            return offset;
        }

        /// <summary>
        /// Attempts to read a TestPacket instance from the specified buffer
        /// </summary>
        /// <param name="source">The source buffer</param>
        /// <param name="value">The read value</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> source, out TestPacket value, out int bytesRead)
        {
            value = default!;
            bytesRead = 0;

            try
            {
                value = new TestPacket();
                var offset = 0;

                // Read properties in key-value format
                while (offset < source.Length)
                {
                    // Read property key
                    if (offset >= source.Length) break;
                    var propertyKey = source[offset];
                    offset++;

                    // Process property based on key
                    switch (propertyKey)
                    {
                        case 0: // Id
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var IdValue);
                                value.Id = IdValue;
                            }
                            break;
                        }
                        case 1: // PlayerName
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var PlayerNameValue);
                                value.PlayerName = PlayerNameValue;
                            }
                            break;
                        }
                        case 2: // X
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var XValue);
                                value.X = XValue;
                            }
                            break;
                        }
                        case 3: // Y
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var YValue);
                                value.Y = YValue;
                            }
                            break;
                        }
                        case 4: // Health
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var HealthValue);
                                value.Health = HealthValue;
                            }
                            break;
                        }
                        case 5: // IsAlive
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadBoolean(source.Slice(offset), out var IsAliveValue);
                                value.IsAlive = IsAliveValue;
                            }
                            break;
                        }
                        default:
                            // Unknown property key, skip to next
                            break;
                    }
                }
                bytesRead = offset;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the total number of bytes required to serialize this instance
        /// </summary>
        /// <returns>The serialized size in bytes</returns>
        public int GetSerializedSize()
        {
            var size = 0;

            // Id: 1 byte for key + value size
            size += 1 + 4;

            // PlayerName: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(PlayerName);

            // X: 1 byte for key + value size
            size += 1 + 4;

            // Y: 1 byte for key + value size
            size += 1 + 4;

            // Health: 1 byte for key + value size
            size += 1 + 4;

            // IsAlive: 1 byte for key + value size
            size += 1 + 1;

            return size;
        }

        /// <summary>
        /// Convenience method for array-based serialization
        /// </summary>
        /// <param name="buffer">The destination buffer</param>
        /// <returns>The number of bytes written</returns>
        public int ToBytes(byte[] buffer)
        {
            return Write(buffer);
        }

    }
}
