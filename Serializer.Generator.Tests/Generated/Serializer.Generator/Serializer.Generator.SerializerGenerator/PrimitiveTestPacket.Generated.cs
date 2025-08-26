using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Interface for packet keys of PrimitiveTestPacket
    /// </summary>
    public interface IPrimitiveTestPacketKeys
    {
        int PacketTypeId { get; }
        string PacketName { get; }
        int PacketVersion { get; }
        Type PacketType { get; }
    }

    /// <summary>
    /// Packet keys for PrimitiveTestPacket
    /// </summary>
    public class PrimitiveTestPacketKeys : IPrimitiveTestPacketKeys
    {
        public int PacketTypeId => 5017;
        public string PacketName => "PrimitiveTestPacket";
        public int PacketVersion => 1;

        /// <summary>
        /// Runtime-accessible metadata for PrimitiveTestPacket
        /// </summary>
        public Type PacketType => typeof(PrimitiveTestPacket);

        /// <summary>
        /// Singleton instance for RnsKeys property access
        /// </summary>
        public static readonly IPrimitiveTestPacketKeys Instance = new PrimitiveTestPacketKeys();
    }

    /// <summary>
    /// Generated partial class for PrimitiveTestPacket with serialization support
    /// </summary>
    public partial class PrimitiveTestPacket
    {
        /// <summary>
        /// Static access to packet keys and metadata
        /// </summary>
        public static IPrimitiveTestPacketKeys RnsKeys => PrimitiveTestPacketKeys.Instance;

        /// <summary>
        /// Alternative access to packet keys (for polymorphic scenarios)
        /// </summary>
        public static IPrimitiveTestPacketKeys Keys => PrimitiveTestPacketKeys.Instance;

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
            offset += Serializer.Runtime.BinarySerializer.WriteInt64(destination.Slice(offset), Id);

            // Write ByteValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), ByteValue);

            // Write SByteValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            offset += Serializer.Runtime.BinarySerializer.WriteSByte(destination.Slice(offset), SByteValue);

            // Write ShortValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 3);
            offset += Serializer.Runtime.BinarySerializer.WriteInt16(destination.Slice(offset), ShortValue);

            // Write UShortValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            offset += Serializer.Runtime.BinarySerializer.WriteUInt16(destination.Slice(offset), UShortValue);

            // Write IntValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 5);
            offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), IntValue);

            // Write UIntValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 6);
            offset += Serializer.Runtime.BinarySerializer.WriteUInt32(destination.Slice(offset), UIntValue);

            // Write LongValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 7);
            offset += Serializer.Runtime.BinarySerializer.WriteInt64(destination.Slice(offset), LongValue);

            // Write ULongValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 8);
            offset += Serializer.Runtime.BinarySerializer.WriteUInt64(destination.Slice(offset), ULongValue);

            // Write FloatValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 9);
            offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), FloatValue);

            // Write DoubleValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 10);
            offset += Serializer.Runtime.BinarySerializer.WriteDouble(destination.Slice(offset), DoubleValue);

            // Write BoolValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 11);
            offset += Serializer.Runtime.BinarySerializer.WriteBoolean(destination.Slice(offset), BoolValue);

            // Write StringValue key and value
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 12);
            offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), StringValue);

            return offset;
        }

        /// <summary>
        /// Attempts to read a PrimitiveTestPacket instance from the specified buffer
        /// </summary>
        /// <param name="source">The source buffer</param>
        /// <param name="value">The read value</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> source, out PrimitiveTestPacket value, out int bytesRead)
        {
            value = default!;
            bytesRead = 0;

            try
            {
                value = new PrimitiveTestPacket();
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
                                offset += Serializer.Runtime.BinarySerializer.ReadInt64(source.Slice(offset), out var IdValue);
                                value.Id = IdValue;
                            }
                            break;
                        }
                        case 1: // ByteValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadByte(source.Slice(offset), out var ByteValueValue);
                                value.ByteValue = ByteValueValue;
                            }
                            break;
                        }
                        case 2: // SByteValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadSByte(source.Slice(offset), out var SByteValueValue);
                                value.SByteValue = SByteValueValue;
                            }
                            break;
                        }
                        case 3: // ShortValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt16(source.Slice(offset), out var ShortValueValue);
                                value.ShortValue = ShortValueValue;
                            }
                            break;
                        }
                        case 4: // UShortValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadUInt16(source.Slice(offset), out var UShortValueValue);
                                value.UShortValue = UShortValueValue;
                            }
                            break;
                        }
                        case 5: // IntValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var IntValueValue);
                                value.IntValue = IntValueValue;
                            }
                            break;
                        }
                        case 6: // UIntValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadUInt32(source.Slice(offset), out var UIntValueValue);
                                value.UIntValue = UIntValueValue;
                            }
                            break;
                        }
                        case 7: // LongValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadInt64(source.Slice(offset), out var LongValueValue);
                                value.LongValue = LongValueValue;
                            }
                            break;
                        }
                        case 8: // ULongValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadUInt64(source.Slice(offset), out var ULongValueValue);
                                value.ULongValue = ULongValueValue;
                            }
                            break;
                        }
                        case 9: // FloatValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var FloatValueValue);
                                value.FloatValue = FloatValueValue;
                            }
                            break;
                        }
                        case 10: // DoubleValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadDouble(source.Slice(offset), out var DoubleValueValue);
                                value.DoubleValue = DoubleValueValue;
                            }
                            break;
                        }
                        case 11: // BoolValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadBoolean(source.Slice(offset), out var BoolValueValue);
                                value.BoolValue = BoolValueValue;
                            }
                            break;
                        }
                        case 12: // StringValue
                        {
                            if (offset < source.Length)
                            {
                                offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var StringValueValue);
                                value.StringValue = StringValueValue;
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
            size += 1 + 8;

            // ByteValue: 1 byte for key + value size
            size += 1 + 1;

            // SByteValue: 1 byte for key + value size
            size += 1 + 1;

            // ShortValue: 1 byte for key + value size
            size += 1 + 2;

            // UShortValue: 1 byte for key + value size
            size += 1 + 2;

            // IntValue: 1 byte for key + value size
            size += 1 + 4;

            // UIntValue: 1 byte for key + value size
            size += 1 + 4;

            // LongValue: 1 byte for key + value size
            size += 1 + 8;

            // ULongValue: 1 byte for key + value size
            size += 1 + 8;

            // FloatValue: 1 byte for key + value size
            size += 1 + 4;

            // DoubleValue: 1 byte for key + value size
            size += 1 + 8;

            // BoolValue: 1 byte for key + value size
            size += 1 + 1;

            // StringValue: 1 byte for key + 2 bytes for length + string content
            size += 1 + 2 + System.Text.Encoding.UTF8.GetByteCount(StringValue);

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
