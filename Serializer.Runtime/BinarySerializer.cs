using System;
using System.Buffers.Binary;

namespace Serializer.Runtime
{
    /// <summary>
    /// Static utility class providing Span-based binary serialization for primitive types.
    /// All methods use little-endian byte order for consistency.
    /// </summary>
    public static class BinarySerializer
    {
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
            if (destination.Length < 1)
                throw new ArgumentException("Buffer too small for boolean", nameof(destination));

            destination[0] = value ? (byte)1 : (byte)0;
            return 1;
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
            if (source.Length < 1)
                throw new ArgumentException("Buffer too small for boolean", nameof(source));

            value = source[0] != 0;
            return 1;
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
            if (destination.Length < 1)
                throw new ArgumentException("Buffer too small for byte", nameof(destination));

            destination[0] = value;
            return 1;
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
            if (source.Length < 1)
                throw new ArgumentException("Buffer too small for byte", nameof(source));

            value = source[0];
            return 1;
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
            if (destination.Length < 1)
                throw new ArgumentException("Buffer too small for sbyte", nameof(destination));

            destination[0] = (byte)value;
            return 1;
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
            if (source.Length < 1)
                throw new ArgumentException("Buffer too small for sbyte", nameof(source));

            value = (sbyte)source[0];
            return 1;
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
            if (destination.Length < 2)
                throw new ArgumentException("Buffer too small for Int16", nameof(destination));

            BinaryPrimitives.WriteInt16LittleEndian(destination, value);
            return 2;
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
            if (source.Length < 2)
                throw new ArgumentException("Buffer too small for Int16", nameof(source));

            value = BinaryPrimitives.ReadInt16LittleEndian(source);
            return 2;
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
            if (destination.Length < 2)
                throw new ArgumentException("Buffer too small for UInt16", nameof(destination));

            BinaryPrimitives.WriteUInt16LittleEndian(destination, value);
            return 2;
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
            if (source.Length < 2)
                throw new ArgumentException("Buffer too small for UInt16", nameof(source));

            value = BinaryPrimitives.ReadUInt16LittleEndian(source);
            return 2;
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
            if (destination.Length < 4)
                throw new ArgumentException("Buffer too small for Int32", nameof(destination));

            BinaryPrimitives.WriteInt32LittleEndian(destination, value);
            return 4;
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
            if (source.Length < 4)
                throw new ArgumentException("Buffer too small for Int32", nameof(source));

            value = BinaryPrimitives.ReadInt32LittleEndian(source);
            return 4;
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
            if (destination.Length < 4)
                throw new ArgumentException("Buffer too small for UInt32", nameof(destination));

            BinaryPrimitives.WriteUInt32LittleEndian(destination, value);
            return 4;
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
            if (source.Length < 4)
                throw new ArgumentException("Buffer too small for UInt32", nameof(source));

            value = BinaryPrimitives.ReadUInt32LittleEndian(source);
            return 4;
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
            if (destination.Length < 8)
                throw new ArgumentException("Buffer too small for Int64", nameof(destination));

            BinaryPrimitives.WriteInt64LittleEndian(destination, value);
            return 8;
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
            if (source.Length < 8)
                throw new ArgumentException("Buffer too small for Int64", nameof(source));

            value = BinaryPrimitives.ReadInt64LittleEndian(source);
            return 8;
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
            if (destination.Length < 8)
                throw new ArgumentException("Buffer too small for UInt64", nameof(destination));

            BinaryPrimitives.WriteUInt64LittleEndian(destination, value);
            return 8;
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
            if (source.Length < 8)
                throw new ArgumentException("Buffer too small for UInt64", nameof(source));

            value = BinaryPrimitives.ReadUInt64LittleEndian(source);
            return 8;
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
            if (destination.Length < 4)
                throw new ArgumentException("Buffer too small for Single", nameof(destination));

#if NET9_0_OR_GREATER
            BinaryPrimitives.WriteSingleLittleEndian(destination, value);
#else
            if (!BitConverter.IsLittleEndian)
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                bytes.CopyTo(destination);
            }
            else
            {
                BitConverter.TryWriteBytes(destination, value);
            }
#endif
            return 4;
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
            if (source.Length < 4)
                throw new ArgumentException("Buffer too small for Single", nameof(source));

#if NET9_0_OR_GREATER
            value = BinaryPrimitives.ReadSingleLittleEndian(source);
#else
            if (!BitConverter.IsLittleEndian)
            {
                var bytes = source.Slice(0, 4).ToArray();
                Array.Reverse(bytes);
                value = BitConverter.ToSingle(bytes, 0);
            }
            else
            {
                value = BitConverter.ToSingle(source);
            }
#endif
            return 4;
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
            if (destination.Length < 8)
                throw new ArgumentException("Buffer too small for Double", nameof(destination));

#if NET9_0_OR_GREATER
            BinaryPrimitives.WriteDoubleLittleEndian(destination, value);
#else
            if (!BitConverter.IsLittleEndian)
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                bytes.CopyTo(destination);
            }
            else
            {
                BitConverter.TryWriteBytes(destination, value);
            }
#endif
            return 8;
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
            if (source.Length < 8)
                throw new ArgumentException("Buffer too small for Double", nameof(source));

#if NET9_0_OR_GREATER
            value = BinaryPrimitives.ReadDoubleLittleEndian(source);
#else
            if (!BitConverter.IsLittleEndian)
            {
                var bytes = source.Slice(0, 8).ToArray();
                Array.Reverse(bytes);
                value = BitConverter.ToDouble(bytes, 0);
            }
            else
            {
                value = BitConverter.ToDouble(source);
            }
#endif
            return 8;
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
            if (destination.Length < 16)
                throw new ArgumentException("Buffer too small for Guid", nameof(destination));

            _ = value.TryWriteBytes(destination);

            return 16;
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
            if (source.Length < 16)
                throw new ArgumentException("Buffer too small for Guid", nameof(source));

            value = new Guid(source);
            return 16;
        }

        #endregion
    }
}
