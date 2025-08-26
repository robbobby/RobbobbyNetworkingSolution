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

            // Write Id key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0);
            // Write flag indicating if value is default
            var hasIdValue = Id != (byte)0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasIdValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasIdValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), Id);
            }

            // Write Type key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            // Write flag indicating if value is default
            var hasTypeValue = (Int32)Type != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasTypeValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasTypeValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), (Int32)Type);
            }

            // Write UserStatus key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            // Write flag indicating if value is default
            var hasUserStatusValue = (Int32)UserStatus != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasUserStatusValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasUserStatusValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), (Int32)UserStatus);
            }

            // Write Username key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 3);
            // Write flag indicating if value is default
            var hasUsernameValue = !string.IsNullOrEmpty(Username);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasUsernameValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasUsernameValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), Username);
            }

            // Write Timestamp key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            // Write flag indicating if value is default
            var hasTimestampValue = Timestamp != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasTimestampValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasTimestampValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), Timestamp);
            }

            // Write terminator byte
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0xFF);

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
                                if (offset + 1 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadByte(source.Slice(offset), out var IdValue);
                                    value.Id = IdValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 1: // Type
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var TypeValue);
                                    value.Type = (PacketType)TypeValue;
                            }
                            // If no value, keep default
                            break;
                        }
                        case 2: // UserStatus
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var UserStatusValue);
                                    value.UserStatus = (Status)UserStatusValue;
                            }
                            // If no value, keep default
                            break;
                        }
                        case 3: // Username
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var UsernameValue);
                                    value.Username = UsernameValue;
                                }
                            }
                            // If no value, keep default (null/empty)
                            break;
                        }
                        case 4: // Timestamp
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var TimestampValue);
                                    value.Timestamp = TimestampValue;
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
            if (Id != (byte)0)
            {
                size += 1;
            }

            // Type: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if ((Int32)Type != 0)
            {
                size += 4;
            }

            // UserStatus: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if ((Int32)UserStatus != 0)
            {
                size += 4;
            }

            // Username: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(Username))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(Username ?? "");
            }

            // Timestamp: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (Timestamp != 0)
            {
                size += 4;
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
