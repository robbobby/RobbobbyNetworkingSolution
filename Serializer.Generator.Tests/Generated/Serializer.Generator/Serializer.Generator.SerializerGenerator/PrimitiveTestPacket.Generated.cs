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

            // Write Id key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0);
            // Write flag indicating if value is default
            var hasIdValue = Id != 0L;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasIdValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasIdValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt64(destination.Slice(offset), Id);
            }

            // Write ByteValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 1);
            // Write flag indicating if value is default
            var hasByteValueValue = ByteValue != (byte)0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasByteValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasByteValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), ByteValue);
            }

            // Write SByteValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 2);
            // Write flag indicating if value is default
            var hasSByteValueValue = SByteValue != (sbyte)0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasSByteValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasSByteValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteSByte(destination.Slice(offset), SByteValue);
            }

            // Write ShortValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 3);
            // Write flag indicating if value is default
            var hasShortValueValue = ShortValue != (short)0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasShortValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasShortValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt16(destination.Slice(offset), ShortValue);
            }

            // Write UShortValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 4);
            // Write flag indicating if value is default
            var hasUShortValueValue = UShortValue != (ushort)0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasUShortValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasUShortValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteUInt16(destination.Slice(offset), UShortValue);
            }

            // Write IntValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 5);
            // Write flag indicating if value is default
            var hasIntValueValue = IntValue != 0;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasIntValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasIntValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt32(destination.Slice(offset), IntValue);
            }

            // Write UIntValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 6);
            // Write flag indicating if value is default
            var hasUIntValueValue = UIntValue != 0U;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasUIntValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasUIntValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteUInt32(destination.Slice(offset), UIntValue);
            }

            // Write LongValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 7);
            // Write flag indicating if value is default
            var hasLongValueValue = LongValue != 0L;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasLongValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasLongValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteInt64(destination.Slice(offset), LongValue);
            }

            // Write ULongValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 8);
            // Write flag indicating if value is default
            var hasULongValueValue = ULongValue != 0UL;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasULongValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasULongValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteUInt64(destination.Slice(offset), ULongValue);
            }

            // Write FloatValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 9);
            // Write flag indicating if value is default
            var hasFloatValueValue = FloatValue != 0f;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasFloatValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasFloatValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteSingle(destination.Slice(offset), FloatValue);
            }

            // Write DoubleValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 10);
            // Write flag indicating if value is default
            var hasDoubleValueValue = DoubleValue != 0d;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasDoubleValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasDoubleValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteDouble(destination.Slice(offset), DoubleValue);
            }

            // Write BoolValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 11);
            // Write flag indicating if value is default
            var hasBoolValueValue = BoolValue != false;
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasBoolValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasBoolValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteBoolean(destination.Slice(offset), BoolValue);
            }

            // Write StringValue key
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 12);
            // Write flag indicating if value is default
            var hasStringValueValue = !string.IsNullOrEmpty(StringValue);
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), hasStringValueValue ? (byte)1 : (byte)0);
            // Write value only if not default
            if (hasStringValueValue)
            {
                offset += Serializer.Runtime.BinarySerializer.WriteString(destination.Slice(offset), StringValue);
            }

            // Write terminator byte
            offset += Serializer.Runtime.BinarySerializer.WriteByte(destination.Slice(offset), 0xFF);

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
                                if (offset + 8 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt64(source.Slice(offset), out var IdValue);
                                    value.Id = IdValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 1: // ByteValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 1 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadByte(source.Slice(offset), out var ByteValueValue);
                                    value.ByteValue = ByteValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 2: // SByteValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 1 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadSByte(source.Slice(offset), out var SByteValueValue);
                                    value.SByteValue = SByteValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 3: // ShortValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt16(source.Slice(offset), out var ShortValueValue);
                                    value.ShortValue = ShortValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 4: // UShortValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadUInt16(source.Slice(offset), out var UShortValueValue);
                                    value.UShortValue = UShortValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 5: // IntValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt32(source.Slice(offset), out var IntValueValue);
                                    value.IntValue = IntValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 6: // UIntValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadUInt32(source.Slice(offset), out var UIntValueValue);
                                    value.UIntValue = UIntValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 7: // LongValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 8 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadInt64(source.Slice(offset), out var LongValueValue);
                                    value.LongValue = LongValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 8: // ULongValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 8 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadUInt64(source.Slice(offset), out var ULongValueValue);
                                    value.ULongValue = ULongValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 9: // FloatValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 4 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadSingle(source.Slice(offset), out var FloatValueValue);
                                    value.FloatValue = FloatValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 10: // DoubleValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 8 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadDouble(source.Slice(offset), out var DoubleValueValue);
                                    value.DoubleValue = DoubleValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 11: // BoolValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a value
                                if (offset + 1 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadBoolean(source.Slice(offset), out var BoolValueValue);
                                    value.BoolValue = BoolValueValue;
                                }
                            }
                            // If no value, keep default
                            break;
                        }
                        case 12: // StringValue
                        {
                            if (hasValue)
                            {
                                // Check if there's enough data to read a string (at least 2 bytes for length)
                                if (offset + 2 <= source.Length)
                                {
                                    offset += Serializer.Runtime.BinarySerializer.ReadString(source.Slice(offset), out var StringValueValue);
                                    value.StringValue = StringValueValue;
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
            if (Id != 0L)
            {
                size += 8;
            }

            // ByteValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (ByteValue != (byte)0)
            {
                size += 1;
            }

            // SByteValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (SByteValue != (sbyte)0)
            {
                size += 1;
            }

            // ShortValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (ShortValue != (short)0)
            {
                size += 2;
            }

            // UShortValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (UShortValue != (ushort)0)
            {
                size += 2;
            }

            // IntValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (IntValue != 0)
            {
                size += 4;
            }

            // UIntValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (UIntValue != 0U)
            {
                size += 4;
            }

            // LongValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (LongValue != 0L)
            {
                size += 8;
            }

            // ULongValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (ULongValue != 0UL)
            {
                size += 8;
            }

            // FloatValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (FloatValue != 0f)
            {
                size += 4;
            }

            // DoubleValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (DoubleValue != 0d)
            {
                size += 8;
            }

            // BoolValue: 2 bytes (key + flag) + value size (if not default)
            size += 2; // Always count key and flag
            if (BoolValue != false)
            {
                size += 1;
            }

            // StringValue: 2 bytes (key + flag) + 2 bytes for length + string content (if not default)
            size += 2; // Always count key and flag
            if (!string.IsNullOrEmpty(StringValue))
            {
                size += 2 + System.Text.Encoding.UTF8.GetByteCount(StringValue ?? "");
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
