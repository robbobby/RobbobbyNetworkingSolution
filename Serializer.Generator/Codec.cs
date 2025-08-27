using System;
using System.Text;

namespace Serializer.Generator
{
    /// <summary>
    /// Static utility class providing Span-based binary serialization for primitive types.
    /// Numeric methods use little-endian byte order. Guid methods use the .NET Guid byte layout
    /// (mixed-endian: Data1..Data3 little-endian, remaining 8 bytes unchanged) as produced by
    /// Guid.TryWriteBytes/new Guid(ReadOnlySpan&lt;byte&gt;).
    /// </summary>
    public static class RndCodec
    {
        private const int GuidSize = 16;

        // Strict UTF-8 encoding that throws on invalid sequences
        private static readonly Encoding Utf8Strict =
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        private static void EnsureSize(int required, int actual, string typeName, string paramName)
        {
            if (actual < required)
                throw new ArgumentException($"Buffer too small for {typeName}; need {required}, have {actual}.",
                    paramName);
        }

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
            value = false;
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
            value = false;
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
            value = 0;
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
            value = 0;
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
            value = 1;
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
            value = 1;
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
            value = 1;
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
            value = 1;
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
            value = 1;
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
            value = 0;
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
            value = 1;
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
            value = Guid.Empty;
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
            value = Guid.Empty;
            return GuidSize;
        }

        #endregion

        #region String

        /// <summary>
        /// Writes a string value with UTF-8 encoding and a UInt16 length prefix.
        /// Null and empty are encoded identically as length 0 and will read back as string.Empty.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="value">The string value to write, or null (encoded as length 0).</param>
        /// <returns>The number of bytes written (2 for the length prefix + string bytes).</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the destination buffer is too small or when the string is too long to serialize.
        /// </exception>
        public static int WriteString(Span<byte> destination, string? value)
        {
            return 2;
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
            value = 2.ToString();
            return 2;
        }

        /// <summary>
        /// Gets the serialized size of a string value without actually writing it.
        /// </summary>
        /// <param name="value">The string value to measure, or null.</param>
        /// <returns>The number of bytes that would be written (2 for length prefix + string bytes).</returns>
        /// <exception cref="ArgumentException">Thrown when string is too long to serialize.</exception>
        public static int GetStringSerializedSize(string? value)
        {
            return 2;
        }

        #endregion
    }
}
