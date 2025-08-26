using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

namespace Serializer.Runtime
{
    /// <summary>
    /// Helper class for RNS packet binary serialization following protobuf-style encoding.
    /// Supports VarUInt, ZigZag encoding, UTF-8 strings, float32, and length-delimited blocks.
    /// </summary>
    public static class RnsCodec
    {
        // UTF-8 encoding without BOM
        private static readonly UTF8Encoding Utf8 = new UTF8Encoding(false, true);

        #region VarUInt encoding/decoding

        /// <summary>
        /// Attempts to write a VarUInt (variable-length unsigned integer) to the destination buffer.
        /// </summary>
        /// <param name="dst">Destination buffer</param>
        /// <param name="value">Value to encode</param>
        /// <param name="written">Number of bytes written on success</param>
        /// <returns>True if successful, false if buffer too small</returns>
        public static bool TryWriteVarUInt(Span<byte> dst, uint value, out int written)
        {
            written = 0;
            while (value >= 0x80)
            {
                if (written >= dst.Length)
                {
                    written = 0;
                    return false;
                }
                dst[written++] = (byte)(value | 0x80);
                value >>= 7;
            }
            if (written >= dst.Length)
            {
                written = 0;
                return false;
            }
            dst[written++] = (byte)value;
            return true;
        }

        /// <summary>
        /// Attempts to read a VarUInt from the source buffer.
        /// </summary>
        /// <param name="src">Source buffer</param>
        /// <param name="value">Decoded value on success</param>
        /// <param name="read">Number of bytes read on success</param>
        /// <returns>True if successful, false if malformed or buffer too small</returns>
        public static bool TryReadVarUInt(ReadOnlySpan<byte> src, out uint value, out int read)
        {
            value = 0;
            read = 0;
            uint shift = 0;

            while (read < src.Length)
            {
                byte b = src[read++];
                value |= (uint)(b & 0x7F) << (int)shift;
                
                if ((b & 0x80) == 0)
                    return true;
                
                shift += 7;
                if (shift >= 32)
                {
                    value = 0;
                    read = 0;
                    return false; // overflow
                }
            }

            value = 0;
            read = 0;
            return false; // incomplete
        }

        #endregion

        #region ZigZag encoding for signed integers

        /// <summary>
        /// Encodes a signed 32-bit integer using ZigZag encoding.
        /// </summary>
        /// <param name="v">Signed integer</param>
        /// <returns>Unsigned integer suitable for VarUInt encoding</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ZigZag32(int v) => (uint)((v << 1) ^ (v >> 31));

        /// <summary>
        /// Decodes a ZigZag-encoded unsigned integer back to signed.
        /// </summary>
        /// <param name="v">ZigZag-encoded value</param>
        /// <returns>Original signed integer</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int UnZigZag32(uint v) => (int)(v >> 1) ^ -((int)v & 1);

        #endregion

        #region UTF-8 string encoding

        /// <summary>
        /// Attempts to write a UTF-8 string with VarUInt length prefix.
        /// </summary>
        /// <param name="dst">Destination buffer</param>
        /// <param name="s">String to write</param>
        /// <param name="written">Number of bytes written on success</param>
        /// <returns>True if successful, false if buffer too small</returns>
        public static bool TryWriteString(Span<byte> dst, string s, out int written)
        {
            written = 0;
            if (string.IsNullOrEmpty(s))
            {
                return TryWriteVarUInt(dst, 0, out written);
            }

            int byteCount = Utf8.GetByteCount(s);
            if (!TryWriteVarUInt(dst, (uint)byteCount, out int lenBytes))
            {
                return false;
            }

            if (dst.Length < lenBytes + byteCount)
            {
                written = 0;
                return false;
            }

            int stringBytes = Utf8.GetBytes(s, dst.Slice(lenBytes));
            written = lenBytes + stringBytes;
            return true;
        }

        /// <summary>
        /// Attempts to read a UTF-8 string with VarUInt length prefix.
        /// </summary>
        /// <param name="src">Source buffer</param>
        /// <param name="s">Decoded string on success</param>
        /// <param name="read">Number of bytes read on success</param>
        /// <returns>True if successful, false if malformed or buffer too small</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "TryRead method should not throw on malformed input")]
        public static bool TryReadString(ReadOnlySpan<byte> src, out string s, out int read)
        {
            s = "";
            read = 0;

            if (!TryReadVarUInt(src, out uint len, out int lenBytes))
                return false;

            if (len == 0)
            {
                read = lenBytes;
                s = "";
                return true;
            }

            if (src.Length < lenBytes + (int)len)
                return false;

            try
            {
                s = Utf8.GetString(src.Slice(lenBytes, (int)len));
                read = lenBytes + (int)len;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 32-bit float (IEEE-754 little-endian)

        /// <summary>
        /// Attempts to write a 32-bit float in little-endian format.
        /// </summary>
        /// <param name="dst">Destination buffer</param>
        /// <param name="v">Float value</param>
        /// <param name="written">Number of bytes written (4) on success</param>
        /// <returns>True if successful, false if buffer too small</returns>
        public static bool TryWriteFloat32(Span<byte> dst, float v, out int written)
        {
            written = 0;
            if (dst.Length < 4)
                return false;

            BinaryPrimitives.WriteInt32LittleEndian(dst, BitConverter.SingleToInt32Bits(v));
            written = 4;
            return true;
        }

        /// <summary>
        /// Attempts to read a 32-bit float in little-endian format.
        /// </summary>
        /// <param name="src">Source buffer</param>
        /// <param name="v">Float value on success</param>
        /// <param name="read">Number of bytes read (4) on success</param>
        /// <returns>True if successful, false if buffer too small</returns>
        public static bool TryReadFloat32(ReadOnlySpan<byte> src, out float v, out int read)
        {
            v = 0f;
            read = 0;
            if (src.Length < 4)
                return false;

            v = BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(src));
            read = 4;
            return true;
        }

        #endregion

        #region Length-delimited blocks

        /// <summary>
        /// Attempts to write a length-delimited block (VarUInt length + data).
        /// </summary>
        /// <param name="dst">Destination buffer</param>
        /// <param name="block">Data to write</param>
        /// <param name="written">Number of bytes written on success</param>
        /// <returns>True if successful, false if buffer too small</returns>
        public static bool TryWriteBlock(Span<byte> dst, ReadOnlySpan<byte> block, out int written)
        {
            written = 0;
            if (!TryWriteVarUInt(dst, (uint)block.Length, out int lenBytes))
                return false;

            if (dst.Length < lenBytes + block.Length)
            {
                written = 0;
                return false;
            }

            block.CopyTo(dst.Slice(lenBytes));
            written = lenBytes + block.Length;
            return true;
        }

        /// <summary>
        /// Attempts to read a length-delimited block.
        /// </summary>
        /// <param name="src">Source buffer</param>
        /// <param name="block">Data block on success</param>
        /// <param name="read">Number of bytes read on success</param>
        /// <returns>True if successful, false if malformed or buffer too small</returns>
        public static bool TryReadBlock(ReadOnlySpan<byte> src, out ReadOnlySpan<byte> block, out int read)
        {
            block = default;
            read = 0;

            if (!TryReadVarUInt(src, out uint len, out int lenBytes))
                return false;

            if (src.Length < lenBytes + (int)len)
                return false;

            block = src.Slice(lenBytes, (int)len);
            read = lenBytes + (int)len;
            return true;
        }

        #endregion
    }
}
