using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Interface for packet keys of EdgeCaseTestPacket
    /// </summary>
    public interface IEdgeCaseTestPacketKeys
    {
        int PacketTypeId { get; }
        string PacketName { get; }
        int PacketVersion { get; }
        Type PacketType { get; }
    }

    /// <summary>
    /// Packet keys for EdgeCaseTestPacket
    /// </summary>
    public class EdgeCaseTestPacketKeys : IEdgeCaseTestPacketKeys
    {
        public int PacketTypeId => 7905;
        public string PacketName => "EdgeCaseTestPacket";
        public int PacketVersion => 1;

        /// <summary>
        /// Runtime-accessible metadata for EdgeCaseTestPacket
        /// </summary>
        public Type PacketType => typeof(EdgeCaseTestPacket);

        /// <summary>
        /// Singleton instance for RnsKeys property access
        /// </summary>
        public static readonly IEdgeCaseTestPacketKeys Instance = new EdgeCaseTestPacketKeys();
    }

    /// <summary>
    /// Generated partial class for EdgeCaseTestPacket with serialization support
    /// </summary>
    public partial class EdgeCaseTestPacket
    {
        /// <summary>
        /// Static access to packet keys and metadata
        /// </summary>
        public static IEdgeCaseTestPacketKeys RnsKeys => EdgeCaseTestPacketKeys.Instance;

        /// <summary>
        /// Alternative access to packet keys (for polymorphic scenarios)
        /// </summary>
        public static IEdgeCaseTestPacketKeys Keys => EdgeCaseTestPacketKeys.Instance;

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
            var hasIdValue = !string.IsNullOrEmpty(Id);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasIdValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasIdValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), Id);
            }

            // Write MaxInt key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            // Write flag indicating if value is default
            var hasMaxIntValue = MaxInt != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasMaxIntValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasMaxIntValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), MaxInt);
            }

            // Write MinInt key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            // Write flag indicating if value is default
            var hasMinIntValue = MinInt != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasMinIntValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasMinIntValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), MinInt);
            }

            // Write MaxFloat key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 3);
            // Write flag indicating if value is default
            var hasMaxFloatValue = MaxFloat != 0f;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasMaxFloatValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasMaxFloatValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), MaxFloat);
            }

            // Write MinFloat key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            // Write flag indicating if value is default
            var hasMinFloatValue = MinFloat != 0f;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasMinFloatValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasMinFloatValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), MinFloat);
            }

            // Write MaxDouble key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 5);
            // Write flag indicating if value is default
            var hasMaxDoubleValue = MaxDouble != 0d;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasMaxDoubleValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasMaxDoubleValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteDouble(destination.Slice(offset), MaxDouble);
            }

            // Write MinDouble key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 6);
            // Write flag indicating if value is default
            var hasMinDoubleValue = MinDouble != 0d;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasMinDoubleValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasMinDoubleValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteDouble(destination.Slice(offset), MinDouble);
            }

            // Write EmptyString key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 7);
            // Write flag indicating if value is default
            var hasEmptyStringValue = !string.IsNullOrEmpty(EmptyString);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasEmptyStringValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasEmptyStringValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), EmptyString);
            }

            // Write LongString key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 8);
            // Write flag indicating if value is default
            var hasLongStringValue = !string.IsNullOrEmpty(LongString);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasLongStringValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasLongStringValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), LongString);
            }

            // Write TrueValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 9);
            // Write flag indicating if value is default
            var hasTrueValueValue = TrueValue != false;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasTrueValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasTrueValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteBoolean(destination.Slice(offset), TrueValue);
            }

            // Write FalseValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 10);
            // Write flag indicating if value is default
            var hasFalseValueValue = FalseValue != false;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasFalseValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasFalseValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteBoolean(destination.Slice(offset), FalseValue);
            }

            // Write terminator byte
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0xFF);

            return offset;
        }

        /// <summary>
        /// Attempts to read a EdgeCaseTestPacket instance from the specified buffer
        /// </summary>
        /// <param name="source">The source buffer</param>
        /// <param name="value">The read value</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> source, out EdgeCaseTestPacket value, out int bytesRead)
        {
            value = default!;
            bytesRead = 0;

            try
            {
                value = new EdgeCaseTestPacket();
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
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var IdValue);
                                    value.Id = IdValue;
                                }
                            }
                            // If no value, keep default (null/empty)
                            break;
                        }
                        case 1: // MaxInt
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var MaxIntValue);
                                    value.MaxInt = MaxIntValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 2: // MinInt
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var MinIntValue);
                                    value.MinInt = MinIntValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 3: // MaxFloat
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var MaxFloatValue);
                                    value.MaxFloat = MaxFloatValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 4: // MinFloat
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var MinFloatValue);
                                    value.MinFloat = MinFloatValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 5: // MaxDouble
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 8 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadDouble(source.Slice(offset), out var MaxDoubleValue);
                                    value.MaxDouble = MaxDoubleValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 6: // MinDouble
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 8 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadDouble(source.Slice(offset), out var MinDoubleValue);
                                    value.MinDouble = MinDoubleValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 7: // EmptyString
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var EmptyStringValue);
                                    value.EmptyString = EmptyStringValue;
                                }
                            }
                            // If no value, keep default (null/empty)
                            break;
                        }
                        case 8: // LongString
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var LongStringValue);
                                    value.LongString = LongStringValue;
                                }
                            }
                            // If no value, keep default (null/empty)
                            break;
                        }
                        case 9: // TrueValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 1 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadBoolean(source.Slice(offset), out var TrueValueValue);
                                    value.TrueValue = TrueValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 10: // FalseValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 1 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadBoolean(source.Slice(offset), out var FalseValueValue);
                                    value.FalseValue = FalseValueValue;
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

            // Id: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(Id))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(Id ?? "");
            }

            // MaxInt: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (MaxInt != 0)
            {
                size += 4;
            }

            // MinInt: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (MinInt != 0)
            {
                size += 4;
            }

            // MaxFloat: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (MaxFloat != 0f)
            {
                size += 4;
            }

            // MinFloat: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (MinFloat != 0f)
            {
                size += 4;
            }

            // MaxDouble: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (MaxDouble != 0d)
            {
                size += 8;
            }

            // MinDouble: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (MinDouble != 0d)
            {
                size += 8;
            }

            // EmptyString: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(EmptyString))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(EmptyString ?? "");
            }

            // LongString: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(LongString))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(LongString ?? "");
            }

            // TrueValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (TrueValue != false)
            {
                size += 1;
            }

            // FalseValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (FalseValue != false)
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
