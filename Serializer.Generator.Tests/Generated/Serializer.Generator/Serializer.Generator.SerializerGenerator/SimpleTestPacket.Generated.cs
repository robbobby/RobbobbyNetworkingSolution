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

            // Write Id key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), Id);

            // Write Name key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), Name);

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
                        case 1: // Name
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var NameValue);
                                value.Name = NameValue;
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

            // Name: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(Name);

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
