using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Interface for packet keys of SimpleTestPacket
    /// </summary>
    public interface ISimpleTestPacketKeys
    {
        int PacketTypeId { get; }
        string PacketName { get; }
        int PacketVersion { get; }
        Type PacketType { get; }
    }

    /// <summary>
    /// Packet keys for SimpleTestPacket
    /// </summary>
    public class SimpleTestPacketKeys : ISimpleTestPacketKeys
    {
        public int PacketTypeId => 5178;
        public string PacketName => "SimpleTestPacket";
        public int PacketVersion => 1;

        /// <summary>
        /// Runtime-accessible metadata for SimpleTestPacket
        /// </summary>
        public Type PacketType => typeof(SimpleTestPacket);

        /// <summary>
        /// Singleton instance for RnsKeys property access
        /// </summary>
        public static readonly ISimpleTestPacketKeys Instance = new SimpleTestPacketKeys();
    }

    /// <summary>
    /// Generated partial class for SimpleTestPacket with serialization support
    /// </summary>
    public partial class SimpleTestPacket
    {
        /// <summary>
        /// Static access to packet keys and metadata
        /// </summary>
        public static ISimpleTestPacketKeys RnsKeys => SimpleTestPacketKeys.Instance;

        /// <summary>
        /// Alternative access to packet keys (for polymorphic scenarios)
        /// </summary>
        public static ISimpleTestPacketKeys Keys => SimpleTestPacketKeys.Instance;

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

            // Write Name key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            // Write flag indicating if value is default
            var hasNameValue = !string.IsNullOrEmpty(Name);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasNameValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasNameValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), Name);
            }

            // Write terminator byte
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0xFF);

            return offset;
        }

        /// <summary>
        /// Attempts to read a SimpleTestPacket instance from the specified buffer
        /// </summary>
        /// <param name="source">The source buffer</param>
        /// <param name="value">The read value</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> source, out SimpleTestPacket value, out int bytesRead)
        {
            value = default!;
            bytesRead = 0;

            try
            {
                value = new SimpleTestPacket();
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
                        case 1: // Name
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var NameValue);
                                    value.Name = NameValue;
                                }
                            }
                            // If no value, keep default (null/empty)
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

            // Name: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(Name))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(Name ?? "");
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
