using System;
using System.Buffers.Binary;
using System.Text;

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

        // Strict UTF-8 encoding that throws on invalid sequences
        private static readonly Encoding Utf8Strict = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        private static void EnsureSize(int required, int actual, string typeName, string paramName)
        {
            if (actual < required)
                throw new ArgumentException($"Buffer too small for {typeName}; need {required}, have {actual}.", paramName);
        }

        private static void Reverse4(Span<byte> s)
        {
            (s[0], s[3]) = (s[3], s[0]);
            (s[1], s[2]) = (s[2], s[1]);
        }

        private static void Reverse2(Span<byte> s) => (s[0], s[1]) = (s[1], s[0]);

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
            EnsureSize(sizeof(bool), destination.Length, "boolean", nameof(destination));

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
            EnsureSize(sizeof(bool), source.Length, "boolean", nameof(source));

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
            EnsureSize(sizeof(bool), source.Length, "boolean", nameof(source));

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
            EnsureSize(sizeof(byte), destination.Length, "byte", nameof(destination));

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
            EnsureSize(sizeof(byte), source.Length, "byte", nameof(source));

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
            EnsureSize(sizeof(sbyte), destination.Length, "sbyte", nameof(destination));

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
            EnsureSize(sizeof(sbyte), source.Length, "sbyte", nameof(source));

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
            EnsureSize(sizeof(short), destination.Length, "Int16", nameof(destination));

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
            EnsureSize(sizeof(short), source.Length, "Int16", nameof(source));

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
            EnsureSize(sizeof(ushort), destination.Length, "UInt16", nameof(destination));

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
            EnsureSize(sizeof(ushort), source.Length, "UInt16", nameof(source));

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
            EnsureSize(sizeof(int), destination.Length, "Int32", nameof(destination));

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
            EnsureSize(sizeof(int), source.Length, "Int32", nameof(source));

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
            EnsureSize(sizeof(uint), destination.Length, "UInt32", nameof(destination));

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
            EnsureSize(sizeof(uint), source.Length, "UInt32", nameof(source));

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
            EnsureSize(sizeof(long), destination.Length, "Int64", nameof(destination));

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
            EnsureSize(sizeof(long), source.Length, "Int64", nameof(source));

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
            EnsureSize(sizeof(ulong), destination.Length, "UInt64", nameof(destination));

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
            EnsureSize(sizeof(ulong), source.Length, "UInt64", nameof(source));

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
            EnsureSize(sizeof(float), destination.Length, "Single", nameof(destination));

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
            EnsureSize(sizeof(float), source.Length, "Single", nameof(source));

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
            EnsureSize(sizeof(double), destination.Length, "Double", nameof(destination));

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
            EnsureSize(sizeof(double), source.Length, "Double", nameof(source));

            value = BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(source));
            return sizeof(double);
        }

        #endregion

        #region Guid

        /// <summary>
        /// Writes a Guid value to the specified buffer.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The Guid value to write.</param>
        /// <returns>The number of bytes written (16).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteGuid(Span<byte> destination, Guid value)
        {
            EnsureSize(GuidSize, destination.Length, "Guid", nameof(destination));

            System.Diagnostics.Debug.Assert(value.TryWriteBytes(destination), "TryWriteBytes should succeed with a 16-byte buffer.");

            return GuidSize;
        }

        /// <summary>
        /// Reads a Guid value from the specified buffer.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read Guid value.</param>
        /// <returns>The number of bytes read (16).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadGuid(ReadOnlySpan<byte> source, out Guid value)
        {
            EnsureSize(GuidSize, source.Length, "Guid", nameof(source));

            value = new Guid(source);
            return GuidSize;
        }

        /// <summary>
        /// Writes a Guid in RFC 4122 byte order (network/big-endian for all fields).
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The Guid value to write.</param>
        /// <returns>The number of bytes written (16).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteGuidRfc4122(Span<byte> destination, Guid value)
        {
            EnsureSize(GuidSize, destination.Length, "Guid", nameof(destination));

            // Write .NET mixed-endian layout then reverse the first 4/2/2 bytes in place to get RFC 4122 order
            System.Diagnostics.Debug.Assert(value.TryWriteBytes(destination), "TryWriteBytes should succeed with a 16-byte buffer.");
            Reverse4(destination.Slice(0, 4));
            Reverse2(destination.Slice(4, 2));
            Reverse2(destination.Slice(6, 2));

            return GuidSize;
        }

        /// <summary>
        /// Reads a Guid from RFC 4122 byte order (network/big-endian for all fields).
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read Guid value.</param>
        /// <returns>The number of bytes read (16).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadGuidRfc4122(ReadOnlySpan<byte> source, out Guid value)
        {
            EnsureSize(GuidSize, source.Length, "Guid", nameof(source));

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

        #region String

        /// <summary>
        /// Writes a string value to the specified buffer with UTF-8 encoding and UInt16 length prefix.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The string value to write, or null.</param>
        /// <returns>The number of bytes written (2 for length prefix + string bytes).</returns>
        /// <exception cref="ArgumentException">Thrown when destination buffer is too small.</exception>
        public static int WriteString(Span<byte> destination, string? value)
        {
            if (value == null)
            {
                EnsureSize(2, destination.Length, "string", nameof(destination));
                WriteUInt16(destination, 0);
                return 2;
            }

            // Fast-path for empty string (no encoding needed)
            if (value.Length == 0)
            {
                EnsureSize(2, destination.Length, "string", nameof(destination));
                WriteUInt16(destination, 0);
                return 2;
            }

            EnsureSize(2, destination.Length, "string", nameof(destination));

            int byteCount = Utf8Strict.GetByteCount(value);
            if (byteCount > ushort.MaxValue)
                throw new ArgumentException($"String too long: {byteCount} bytes exceeds maximum {ushort.MaxValue}", nameof(value));

            EnsureSize(2 + byteCount, destination.Length, "string", nameof(destination));

            WriteUInt16(destination, (ushort)byteCount);
            int written = Utf8Strict.GetBytes(value.AsSpan(), destination.Slice(2));
            System.Diagnostics.Debug.Assert(written == byteCount);

            return 2 + byteCount;
        }

        /// <summary>
        /// Reads a string value from the specified buffer with UTF-8 encoding and UInt16 length prefix.
        /// </summary>
        /// <param name="source">The source buffer.</param>
        /// <param name="value">The read string value.</param>
        /// <returns>The number of bytes read (2 for length prefix + string bytes).</returns>
        /// <exception cref="ArgumentException">Thrown when source buffer is too small.</exception>
        public static int ReadString(ReadOnlySpan<byte> source, out string value)
        {
            EnsureSize(2, source.Length, "string", nameof(source));

            ReadUInt16(source, out var lengthValue);

            if (lengthValue == 0)
            {
                value = string.Empty;
                return 2;
            }

            EnsureSize(2 + lengthValue, source.Length, "string", nameof(source));

            value = Utf8Strict.GetString(source.Slice(2, lengthValue));
            return 2 + lengthValue;
        }

        /// <summary>
        /// Gets the serialized size of a string value without actually writing it.
        /// </summary>
        /// <param name="value">The string value to measure, or null.</param>
        /// <returns>The number of bytes that would be written (2 for length prefix + string bytes).</returns>
        /// <exception cref="ArgumentException">Thrown when string is too long to serialize.</exception>
        public static int GetStringSerializedSize(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return 2;

            int byteCount = Utf8Strict.GetByteCount(value);
            if (byteCount > ushort.MaxValue)
                throw new ArgumentException($"String too long: {byteCount} bytes exceeds maximum {ushort.MaxValue}", nameof(value));

            return 2 + byteCount;
        }

        #endregion
    }
}
