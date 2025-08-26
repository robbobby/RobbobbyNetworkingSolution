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

            // Write Id key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), Id);

            // Write MaxInt key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), MaxInt);

            // Write MinInt key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), MinInt);

            // Write MaxFloat key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 3);
            offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), MaxFloat);

            // Write MinFloat key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), MinFloat);

            // Write MaxDouble key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 5);
            offset += Serializer.Runtime.BinarySerializer.WriteDouble(destination.Slice(offset), MaxDouble);

            // Write MinDouble key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 6);
            offset += Serializer.Runtime.BinarySerializer.WriteDouble(destination.Slice(offset), MinDouble);

            // Write EmptyString key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 7);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), EmptyString);

            // Write LongString key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 8);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), LongString);

            // Write TrueValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 9);
            offset += Serializer.Runtime.BinarySerializer.WriteBoolean(destination.Slice(offset), TrueValue);

            // Write FalseValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 10);
            offset += Serializer.Runtime.BinarySerializer.WriteBoolean(destination.Slice(offset), FalseValue);

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
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var IdValue);
                                value.Id = IdValue;
                            }
                            break;
                        }
                        case 1: // MaxInt
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var MaxIntValue);
                                value.MaxInt = MaxIntValue;
                            }
                            break;
                        }
                        case 2: // MinInt
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var MinIntValue);
                                value.MinInt = MinIntValue;
                            }
                            break;
                        }
                        case 3: // MaxFloat
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var MaxFloatValue);
                                value.MaxFloat = MaxFloatValue;
                            }
                            break;
                        }
                        case 4: // MinFloat
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var MinFloatValue);
                                value.MinFloat = MinFloatValue;
                            }
                            break;
                        }
                        case 5: // MaxDouble
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadDouble(source.Slice(offset), out var MaxDoubleValue);
                                value.MaxDouble = MaxDoubleValue;
                            }
                            break;
                        }
                        case 6: // MinDouble
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadDouble(source.Slice(offset), out var MinDoubleValue);
                                value.MinDouble = MinDoubleValue;
                            }
                            break;
                        }
                        case 7: // EmptyString
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var EmptyStringValue);
                                value.EmptyString = EmptyStringValue;
                            }
                            break;
                        }
                        case 8: // LongString
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var LongStringValue);
                                value.LongString = LongStringValue;
                            }
                            break;
                        }
                        case 9: // TrueValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadBoolean(source.Slice(offset), out var TrueValueValue);
                                value.TrueValue = TrueValueValue;
                            }
                            break;
                        }
                        case 10: // FalseValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadBoolean(source.Slice(offset), out var FalseValueValue);
                                value.FalseValue = FalseValueValue;
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

            // Id: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(Id);

            // MaxInt: 1 byte for key + value size
            size += 1 + 4;

            // MinInt: 1 byte for key + value size
            size += 1 + 4;

            // MaxFloat: 1 byte for key + value size
            size += 1 + 4;

            // MinFloat: 1 byte for key + value size
            size += 1 + 4;

            // MaxDouble: 1 byte for key + value size
            size += 1 + 8;

            // MinDouble: 1 byte for key + value size
            size += 1 + 8;

            // EmptyString: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(EmptyString);

            // LongString: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(LongString);

            // TrueValue: 1 byte for key + value size
            size += 1 + 1;

            // FalseValue: 1 byte for key + value size
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
