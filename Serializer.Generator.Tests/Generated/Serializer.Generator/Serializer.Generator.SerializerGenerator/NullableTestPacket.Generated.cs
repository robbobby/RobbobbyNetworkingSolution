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

            // Write NullableString key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            // Write flag indicating if value is default
            var hasNullableStringValue = !string.IsNullOrEmpty(NullableString);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasNullableStringValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasNullableStringValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), NullableString);
            }

            // Write NonNullableString key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            // Write flag indicating if value is default
            var hasNonNullableStringValue = !string.IsNullOrEmpty(NonNullableString);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasNonNullableStringValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasNonNullableStringValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), NonNullableString);
            }

            // Write NonNullableInt key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            // Write flag indicating if value is default
            var hasNonNullableIntValue = NonNullableInt != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasNonNullableIntValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasNonNullableIntValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), NonNullableInt);
            }

            // Write terminator byte
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0xFF);

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
                        case 1: // NullableString
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var NullableStringValue);
                                    value.NullableString = NullableStringValue;
                                }
                            }
                            // If no value, keep default (null/empty)
                            break;
                        }
                        case 2: // NonNullableString
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var NonNullableStringValue);
                                    value.NonNullableString = NonNullableStringValue;
                                }
                            }
                            // If no value, keep default (null/empty)
                            break;
                        }
                        case 3: // NullableInt
                        case 4: // NonNullableInt
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var NonNullableIntValue);
                                    value.NonNullableInt = NonNullableIntValue;
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

            // NullableString: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(NullableString))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(NullableString ?? "");
            }

            // NonNullableString: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(NonNullableString))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(NonNullableString ?? "");
            }

            // NonNullableInt: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (NonNullableInt != 0)
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
