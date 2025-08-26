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

            // Write Id key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0);
            // Write flag indicating if value is default
            var hasIdValue = Id != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasIdValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasIdValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), Id);
            }

            // Write PlayerName key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            // Write flag indicating if value is default
            var hasPlayerNameValue = !string.IsNullOrEmpty(PlayerName);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasPlayerNameValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasPlayerNameValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), PlayerName);
            }

            // Write X key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            // Write flag indicating if value is default
            var hasXValue = X != 0f;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasXValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasXValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), X);
            }

            // Write Y key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 3);
            // Write flag indicating if value is default
            var hasYValue = Y != 0f;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasYValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasYValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), Y);
            }

            // Write Health key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            // Write flag indicating if value is default
            var hasHealthValue = Health != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasHealthValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasHealthValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), Health);
            }

            // Write IsAlive key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 5);
            // Write flag indicating if value is default
            var hasIsAliveValue = IsAlive != false;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasIsAliveValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasIsAliveValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteBoolean(destination.Slice(offset), IsAlive);
            }

            // Write terminator byte
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0xFF);

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

                // Read properties in key-flag-value format until terminator
                while (offset < source.Length)
                {
                    // Check if we have enough data for key and flag
                    if (offset + 2 > source.Length) break;

                    // Read property key and flag
                    var propertyKey = source[offset];
                    var hasValue = source[offset + 1] != 0;
                    offset += 2;

                    // Check for terminator
                    if (propertyKey == 0xFF) break;

                    // Process property based on key
                    switch (propertyKey)
                    {
                        case 0: // Id
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var IdValue);
                                    value.Id = IdValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 1: // PlayerName
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var PlayerNameValue);
                                    value.PlayerName = PlayerNameValue;
                                }
                            }
                            // If no value, keep default (null/empty)
                            break;
                        }
                        case 2: // X
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var XValue);
                                    value.X = XValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 3: // Y
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var YValue);
                                    value.Y = YValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 4: // Health
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var HealthValue);
                                    value.Health = HealthValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 5: // IsAlive
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 1 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadBoolean(source.Slice(offset), out var IsAliveValue);
                                    value.IsAlive = IsAliveValue;
                                }
                            }
                            // If no value, keep default
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

            // Id: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (Id != 0)
            {
                size += 4;
            }

            // PlayerName: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(PlayerName))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(PlayerName ?? "");
            }

            // X: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (X != 0f)
            {
                size += 4;
            }

            // Y: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (Y != 0f)
            {
                size += 4;
            }

            // Health: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (Health != 0)
            {
                size += 4;
            }

            // IsAlive: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (IsAlive != false)
            {
                size += 1;
            }

            // 1 byte for terminator
            size += 1;

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
