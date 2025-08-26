using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Interface for packet keys of EnumTestPacket
    /// </summary>
    public interface IEnumTestPacketKeys
    {
        int PacketTypeId { get; }
        string PacketName { get; }
        int PacketVersion { get; }
        Type PacketType { get; }
    }

    /// <summary>
    /// Packet keys for EnumTestPacket
    /// </summary>
    public class EnumTestPacketKeys : IEnumTestPacketKeys
    {
        public int PacketTypeId => 8965;
        public string PacketName => "EnumTestPacket";
        public int PacketVersion => 1;

        /// <summary>
        /// Runtime-accessible metadata for EnumTestPacket
        /// </summary>
        public Type PacketType => typeof(EnumTestPacket);

        /// <summary>
        /// Singleton instance for RnsKeys property access
        /// </summary>
        public static readonly IEnumTestPacketKeys Instance = new EnumTestPacketKeys();
    }

    /// <summary>
    /// Generated partial class for EnumTestPacket with serialization support
    /// </summary>
    public partial class EnumTestPacket
    {
        /// <summary>
        /// Static access to packet keys and metadata
        /// </summary>
        public static IEnumTestPacketKeys RnsKeys => EnumTestPacketKeys.Instance;

        /// <summary>
        /// Alternative access to packet keys (for polymorphic scenarios)
        /// </summary>
        public static IEnumTestPacketKeys Keys => EnumTestPacketKeys.Instance;

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
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), Id);

            // Write Type key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), (Int32)Type);

            // Write UserStatus key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), (Int32)UserStatus);

            // Write Username key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 3);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), Username);

            // Write Timestamp key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), Timestamp);

            return offset;
        }

        /// <summary>
        /// Attempts to read a EnumTestPacket instance from the specified buffer
        /// </summary>
        /// <param name="source">The source buffer</param>
        /// <param name="value">The read value</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> source, out EnumTestPacket value, out int bytesRead)
        {
            value = default!;
            bytesRead = 0;

            try
            {
                value = new EnumTestPacket();
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
                                offset += Serializer.Runtime.BinarySerializer.ReadByte(source.Slice(offset), out var IdValue);
                                value.Id = IdValue;
                            }
                            break;
                        }
                        case 1: // Type
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var TypeValue);
                                value.Type = (PacketType)TypeValue;
                            }
                            break;
                        }
                        case 2: // UserStatus
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var UserStatusValue);
                                value.UserStatus = (Status)UserStatusValue;
                            }
                            break;
                        }
                        case 3: // Username
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var UsernameValue);
                                value.Username = UsernameValue;
                            }
                            break;
                        }
                        case 4: // Timestamp
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var TimestampValue);
                                value.Timestamp = TimestampValue;
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
            size += 1 + 1;

            // Type: 1 byte for key + value size
            size += 1 + 4;

            // UserStatus: 1 byte for key + value size
            size += 1 + 4;

            // Username: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(Username);

            // Timestamp: 1 byte for key + value size
            size += 1 + 4;

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
