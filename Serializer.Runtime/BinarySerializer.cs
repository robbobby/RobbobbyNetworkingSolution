using System;
using System.Buffers.Binary;

namespace Serializer.Runtime
{
    /// <summary>
    /// Static utility class providing Span-based binary serialization for primitive types.
    /// Numeric methods use little-endian byte order. Guid methods use the .NET Guid byte layout
    /// (mixed-endian: Data1..Data3 little-endian, remaining 8 bytes unchanged) as produced by
    /// Guid.TryWriteBytes/new Guid(ReadOnlySpan&lt;byte&gt;).
    /// </summary>
    public static class BinarySerializer
    {
        private const int GuidSize = 16;

        #region Boolean

        /// <summary>
        /// Writes a boolean value to the specified buffer.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The boolean value to write.</param>
        /// <returns>The number of bytes written (1).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteBoolean(Span<byte> destination, bool value)
        {
            if (destination.Length < sizeof(bool))
                throw new ArgumentException($"Buffer too small for boolean; need {sizeof(bool)}, have {destination.Length}.", nameof(destination));

            destination[0] = value ? (byte)1 : (byte)0;
            return sizeof(bool);
        }

        /// <summary>
        /// Reads a boolean value from the specified buffer.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read boolean value.</param>
        /// <returns>The number of bytes read (1).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadBoolean(ReadOnlySpan<byte> source, out bool value)
        {
            if (source.Length < sizeof(bool))
                throw new ArgumentException($"Buffer too small for boolean; need {sizeof(bool)}, have {source.Length}.", nameof(source));

            value = source[0] != 0;
            return sizeof(bool);
        }

        /// <summary>
        /// Reads a boolean that must be encoded as 0 or 1.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read boolean value.</param>
        /// <returns>The number of bytes read (1).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        /// <exception cref="FormatException">Thrown when the encoded value is not 0 or 1.</exception>
        public static int ReadBooleanStrict(ReadOnlySpan<byte> source, out bool value)
        {
            if (source.Length < sizeof(bool))
                throw new ArgumentException($"Buffer too small for boolean; need {sizeof(bool)}, have {source.Length}.", nameof(source));

            byte b = source[0];
            if (b != 0 && b != 1)
                throw new FormatException($"Invalid boolean encoding: expected 0 or 1, got {b}.");

            value = b == 1;
            return sizeof(bool);
        }

        #endregion

        #region Byte

        /// <summary>
        /// Writes a byte value to the specified buffer.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The byte value to write.</param>
        /// <returns>The number of bytes written (1).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteByte(Span<byte> destination, byte value)
        {
            if (destination.Length < sizeof(byte))
                throw new ArgumentException($"Buffer too small for byte; need {sizeof(byte)}, have {destination.Length}.", nameof(destination));

            destination[0] = value;
            return sizeof(byte);
        }

        /// <summary>
        /// Reads a byte value from the specified buffer.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read byte value.</param>
        /// <returns>The number of bytes read (1).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadByte(ReadOnlySpan<byte> source, out byte value)
        {
            if (source.Length < sizeof(byte))
                throw new ArgumentException($"Buffer too small for byte; need {sizeof(byte)}, have {source.Length}.", nameof(source));

            value = source[0];
            return sizeof(byte);
        }

        #endregion

        #region SByte

        /// <summary>
        /// Writes a signed byte value to the specified buffer.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The signed byte value to write.</param>
        /// <returns>The number of bytes written (1).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteSByte(Span<byte> destination, sbyte value)
        {
            if (destination.Length < sizeof(sbyte))
                throw new ArgumentException($"Buffer too small for sbyte; need {sizeof(sbyte)}, have {destination.Length}.", nameof(destination));

            destination[0] = (byte)value;
            return sizeof(sbyte);
        }

        /// <summary>
        /// Reads a signed byte value from the specified buffer.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read signed byte value.</param>
        /// <returns>The number of bytes read (1).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadSByte(ReadOnlySpan<byte> source, out sbyte value)
        {
            if (source.Length < sizeof(sbyte))
                throw new ArgumentException($"Buffer too small for sbyte; need {sizeof(sbyte)}, have {source.Length}.", nameof(source));

            value = (sbyte)source[0];
            return sizeof(sbyte);
        }

        #endregion

        #region Int16

        /// <summary>
        /// Writes a 16-bit signed integer to the specified buffer in little-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The 16-bit signed integer to write.</param>
        /// <returns>The number of bytes written (2).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteInt16(Span<byte> destination, short value)
        {
            if (destination.Length < sizeof(short))
                throw new ArgumentException($"Buffer too small for Int16; need {sizeof(short)}, have {destination.Length}.", nameof(destination));

            BinaryPrimitives.WriteInt16LittleEndian(destination, value);
            return sizeof(short);
        }

        /// <summary>
        /// Reads a 16-bit signed integer from the specified buffer in little-endian format.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read 16-bit signed integer.</param>
        /// <returns>The number of bytes read (2).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadInt16(ReadOnlySpan<byte> source, out short value)
        {
            if (source.Length < sizeof(short))
                throw new ArgumentException($"Buffer too small for Int16; need {sizeof(short)}, have {source.Length}.", nameof(source));

            value = BinaryPrimitives.ReadInt16LittleEndian(source);
            return sizeof(short);
        }

        #endregion

        #region UInt16

        /// <summary>
        /// Writes a 16-bit unsigned integer to the specified buffer in little-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The 16-bit unsigned integer to write.</param>
        /// <returns>The number of bytes written (2).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteUInt16(Span<byte> destination, ushort value)
        {
            if (destination.Length < sizeof(ushort))
                throw new ArgumentException($"Buffer too small for UInt16; need {sizeof(ushort)}, have {destination.Length}.", nameof(destination));

            BinaryPrimitives.WriteUInt16LittleEndian(destination, value);
            return sizeof(ushort);
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer from the specified buffer in little-endian format.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read 16-bit unsigned integer.</param>
        /// <returns>The number of bytes read (2).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadUInt16(ReadOnlySpan<byte> source, out ushort value)
        {
            if (source.Length < sizeof(ushort))
                throw new ArgumentException($"Buffer too small for UInt16; need {sizeof(ushort)}, have {source.Length}.", nameof(source));

            value = BinaryPrimitives.ReadUInt16LittleEndian(source);
            return sizeof(ushort);
        }

        #endregion

        #region Int32

        /// <summary>
        /// Writes a 32-bit signed integer to the specified buffer in little-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The 32-bit signed integer to write.</param>
        /// <returns>The number of bytes written (4).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteInt32(Span<byte> destination, int value)
        {
            if (destination.Length < sizeof(int))
                throw new ArgumentException($"Buffer too small for Int32; need {sizeof(int)}, have {destination.Length}.", nameof(destination));

            BinaryPrimitives.WriteInt32LittleEndian(destination, value);
            return sizeof(int);
        }

        /// <summary>
        /// Reads a 32-bit signed integer from the specified buffer in little-endian format.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read 32-bit signed integer.</param>
        /// <returns>The number of bytes read (4).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadInt32(ReadOnlySpan<byte> source, out int value)
        {
            if (source.Length < sizeof(int))
                throw new ArgumentException($"Buffer too small for Int32; need {sizeof(int)}, have {source.Length}.", nameof(source));

            value = BinaryPrimitives.ReadInt32LittleEndian(source);
            return sizeof(int);
        }

        #endregion

        #region UInt32

        /// <summary>
        /// Writes a 32-bit unsigned integer to the specified buffer in little-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The 32-bit unsigned integer to write.</param>
        /// <returns>The number of bytes written (4).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteUInt32(Span<byte> destination, uint value)
        {
            if (destination.Length < sizeof(uint))
                throw new ArgumentException($"Buffer too small for UInt32; need {sizeof(uint)}, have {destination.Length}.", nameof(destination));

            BinaryPrimitives.WriteUInt32LittleEndian(destination, value);
            return sizeof(uint);
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer from the specified buffer in little-endian format.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read 32-bit unsigned integer.</param>
        /// <returns>The number of bytes read (4).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadUInt32(ReadOnlySpan<byte> source, out uint value)
        {
            if (source.Length < sizeof(uint))
                throw new ArgumentException($"Buffer too small for UInt32; need {sizeof(uint)}, have {source.Length}.", nameof(source));

            value = BinaryPrimitives.ReadUInt32LittleEndian(source);
            return sizeof(uint);
        }

        #endregion

        #region Int64

        /// <summary>
        /// Writes a 64-bit signed integer to the specified buffer in little-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The 64-bit signed integer to write.</param>
        /// <returns>The number of bytes written (8).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteInt64(Span<byte> destination, long value)
        {
            if (destination.Length < sizeof(long))
                throw new ArgumentException($"Buffer too small for Int64; need {sizeof(long)}, have {destination.Length}.", nameof(destination));

            BinaryPrimitives.WriteInt64LittleEndian(destination, value);
            return sizeof(long);
        }

        /// <summary>
        /// Reads a 64-bit signed integer from the specified buffer in little-endian format.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read 64-bit signed integer.</param>
        /// <returns>The number of bytes read (8).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadInt64(ReadOnlySpan<byte> source, out long value)
        {
            if (source.Length < sizeof(long))
                throw new ArgumentException($"Buffer too small for Int64; need {sizeof(long)}, have {source.Length}.", nameof(source));

            value = BinaryPrimitives.ReadInt64LittleEndian(source);
            return sizeof(long);
        }

        #endregion

        #region UInt64

        /// <summary>
        /// Writes a 64-bit unsigned integer to the specified buffer in little-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The 64-bit unsigned integer to write.</param>
        /// <returns>The number of bytes written (8).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteUInt64(Span<byte> destination, ulong value)
        {
            if (destination.Length < sizeof(ulong))
                throw new ArgumentException($"Buffer too small for UInt64; need {sizeof(ulong)}, have {destination.Length}.", nameof(destination));

            BinaryPrimitives.WriteUInt64LittleEndian(destination, value);
            return sizeof(ulong);
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer from the specified buffer in little-endian format.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read 64-bit unsigned integer.</param>
        /// <returns>The number of bytes read (8).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadUInt64(ReadOnlySpan<byte> source, out ulong value)
        {
            if (source.Length < sizeof(ulong))
                throw new ArgumentException($"Buffer too small for UInt64; need {sizeof(ulong)}, have {source.Length}.", nameof(source));

            value = BinaryPrimitives.ReadUInt64LittleEndian(source);
            return sizeof(ulong);
        }

        #endregion

        #region Single

        /// <summary>
        /// Writes a 32-bit floating-point value to the specified buffer in little-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The 32-bit floating-point value to write.</param>
        /// <returns>The number of bytes written (4).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteSingle(Span<byte> destination, float value)
        {
            if (destination.Length < sizeof(float))
                throw new ArgumentException($"Buffer too small for Single; need {sizeof(float)}, have {destination.Length}.", nameof(destination));

            BinaryPrimitives.WriteInt32LittleEndian(destination, BitConverter.SingleToInt32Bits(value));
            return sizeof(float);
        }

        /// <summary>
        /// Reads a 32-bit floating-point value from the specified buffer in little-endian format.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read 32-bit floating-point value.</param>
        /// <returns>The number of bytes read (4).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadSingle(ReadOnlySpan<byte> source, out float value)
        {
            if (source.Length < sizeof(float))
                throw new ArgumentException($"Buffer too small for Single; need {sizeof(float)}, have {source.Length}.", nameof(source));

            value = BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(source));
            return sizeof(float);
        }

        #endregion

        #region Double

        /// <summary>
        /// Writes a 64-bit floating-point value to the specified buffer in little-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The 64-bit floating-point value to write.</param>
        /// <returns>The number of bytes written (8).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteDouble(Span<byte> destination, double value)
        {
            if (destination.Length < sizeof(double))
                throw new ArgumentException($"Buffer too small for Double; need {sizeof(double)}, have {destination.Length}.", nameof(destination));

            BinaryPrimitives.WriteInt64LittleEndian(destination, BitConverter.DoubleToInt64Bits(value));
            return sizeof(double);
        }

        /// <summary>
        /// Reads a 64-bit floating-point value from the specified buffer in little-endian format.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read 64-bit floating-point value.</param>
        /// <returns>The number of bytes read (8).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadDouble(ReadOnlySpan<byte> source, out double value)
        {
            if (source.Length < sizeof(double))
                throw new ArgumentException($"Buffer too small for Double; need {sizeof(double)}, have {source.Length}.", nameof(source));

            value = BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(source));
            return sizeof(double);
        }

        #endregion

        #region Guid

        /// <summary>
        /// Writes a GUID value to the specified buffer.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The GUID value to write.</param>
        /// <returns>The number of bytes written (16).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteGuid(Span<byte> destination, Guid value)
        {
            if (destination.Length < GuidSize)
                throw new ArgumentException($"Buffer too small for Guid; need {GuidSize}, have {destination.Length}.", nameof(destination));

            _ = value.TryWriteBytes(destination);

            return GuidSize;
        }

        /// <summary>
        /// Reads a GUID value from the specified buffer.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read GUID value.</param>
        /// <returns>The number of bytes read (16).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadGuid(ReadOnlySpan<byte> source, out Guid value)
        {
            if (source.Length < GuidSize)
                throw new ArgumentException($"Buffer too small for Guid; need {GuidSize}, have {source.Length}.", nameof(source));

            value = new Guid(source);
            return GuidSize;
        }

        /// <summary>
        /// Writes a Guid in RFC 4122 byte order (network/big-endian for all fields).
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The GUID value to write.</param>
        /// <returns>The number of bytes written (16).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteGuidRfc4122(Span<byte> destination, Guid value)
        {
            if (destination.Length < GuidSize)
                throw new ArgumentException($"Buffer too small for Guid; need {GuidSize}, have {destination.Length}.", nameof(destination));

            Span<byte> tmp = stackalloc byte[GuidSize];
            value.TryWriteBytes(tmp); // .NET mixed-endian layout

            // Convert to RFC 4122 layout by reversing the first 4, next 2, next 2 bytes
            destination[0] = tmp[3]; destination[1] = tmp[2]; destination[2] = tmp[1]; destination[3] = tmp[0];
            destination[4] = tmp[5]; destination[5] = tmp[4];
            destination[6] = tmp[7]; destination[7] = tmp[6];
            tmp.Slice(8).CopyTo(destination.Slice(8));

            return GuidSize;
        }

        /// <summary>
        /// Reads a Guid from RFC 4122 byte order (network/big-endian for all fields).
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read GUID value.</param>
        /// <returns>The number of bytes read (16).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadGuidRfc4122(ReadOnlySpan<byte> source, out Guid value)
        {
            if (source.Length < GuidSize)
                throw new ArgumentException($"Buffer too small for Guid; need {GuidSize}, have {source.Length}.", nameof(source));

            Span<byte> tmp = stackalloc byte[GuidSize];
            // Reverse RFC 4122 -> .NET layout
            tmp[0] = source[3]; tmp[1] = source[2]; tmp[2] = source[1]; tmp[3] = source[0];
            tmp[4] = source[5]; tmp[5] = source[4];
            tmp[6] = source[7]; tmp[7] = source[6];
            source.Slice(8).CopyTo(tmp.Slice(8));

            value = new Guid(tmp);
            return GuidSize;
        }

        #endregion
    }
}
