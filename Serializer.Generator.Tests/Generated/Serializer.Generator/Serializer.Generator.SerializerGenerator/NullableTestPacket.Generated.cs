using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Interface for packet keys of NullableTestPacket
    /// </summary>
    public interface INullableTestPacketKeys
    {
        int PacketTypeId { get; }
        string PacketName { get; }
        int PacketVersion { get; }
        Type PacketType { get; }
    }

    /// <summary>
    /// Packet keys for NullableTestPacket
    /// </summary>
    public class NullableTestPacketKeys : INullableTestPacketKeys
    {
        public int PacketTypeId => 4449;
        public string PacketName => "NullableTestPacket";
        public int PacketVersion => 1;

        /// <summary>
        /// Runtime-accessible metadata for NullableTestPacket
        /// </summary>
        public Type PacketType => typeof(NullableTestPacket);

        /// <summary>
        /// Singleton instance for RnsKeys property access
        /// </summary>
        public static readonly INullableTestPacketKeys Instance = new NullableTestPacketKeys();
    }

    /// <summary>
    /// Generated partial class for NullableTestPacket with serialization support
    /// </summary>
    public partial class NullableTestPacket
    {
        /// <summary>
        /// Static access to packet keys and metadata
        /// </summary>
        public static INullableTestPacketKeys RnsKeys => NullableTestPacketKeys.Instance;

        /// <summary>
        /// Alternative access to packet keys (for polymorphic scenarios)
        /// </summary>
        public static INullableTestPacketKeys Keys => NullableTestPacketKeys.Instance;

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

            // Write NullableString key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), NullableString);

            // Write NonNullableString key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), NonNullableString);

            // Write NonNullableInt key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), NonNullableInt);

            return offset;
        }

        /// <summary>
        /// Attempts to read a NullableTestPacket instance from the specified buffer
        /// </summary>
        /// <param name="source">The source buffer</param>
        /// <param name="value">The read value</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> source, out NullableTestPacket value, out int bytesRead)
        {
            value = default!;
            bytesRead = 0;

            try
            {
                value = new NullableTestPacket();
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
                        case 1: // NullableString
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var NullableStringValue);
                                value.NullableString = NullableStringValue;
                            }
                            break;
                        }
                        case 2: // NonNullableString
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var NonNullableStringValue);
                                value.NonNullableString = NonNullableStringValue;
                            }
                            break;
                        }
                        case 3: // NullableInt
                        case 4: // NonNullableInt
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var NonNullableIntValue);
                                value.NonNullableInt = NonNullableIntValue;
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

            // NullableString: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(NullableString);

            // NonNullableString: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(NonNullableString);

            // NonNullableInt: 1 byte for key + value size
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
