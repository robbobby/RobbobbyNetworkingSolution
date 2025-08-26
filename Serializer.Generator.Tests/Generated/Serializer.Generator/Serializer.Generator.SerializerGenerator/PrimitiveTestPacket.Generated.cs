using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Generated partial class for PrimitiveTestPacket with RNS Binary Spec v1 serialization support
    /// </summary>
    public partial class PrimitiveTestPacket
    {
        /// <summary>
        /// Compile-time field number constants for fast lookup
        /// </summary>
        public static class Keys
        {
            public const int Id = 1;
            public const int ByteValue = 2;
            public const int SByteValue = 3;
            public const int ShortValue = 4;
            public const int UShortValue = 5;
            public const int IntValue = 6;
            public const int UIntValue = 7;
            public const int LongValue = 8;
            public const int ULongValue = 9;
            public const int FloatValue = 10;
            public const int DoubleValue = 11;
            public const int BoolValue = 12;
            public const int StringValue = 13;
        }

        /// <summary>
        /// Writes the current instance to the specified buffer using RNS Binary Spec v1
        /// </summary>
        /// <param name="buffer">The destination buffer</param>
        /// <param name="bytesWritten">The number of bytes written</param>
        /// <returns>True if successful, false if buffer is too small</returns>
        public bool Write(System.Span<byte> buffer, out int bytesWritten)
        {
            bytesWritten = 0;
            var dst = buffer;

            // IntValue (field 6, WT=0 via ZigZag)
            if (IntValue != 0)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.IntValue, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, Serializer.Runtime.RnsCodec.ZigZag32(IntValue), out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // FloatValue (field 10, WT=5)
            if (FloatValue != 0f)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.FloatValue, 5), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteFloat32(dst, FloatValue, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // BoolValue (field 12, WT=0)
            if (BoolValue)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.BoolValue, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, 1u, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // StringValue (field 13, WT=2)
            if (!string.IsNullOrEmpty(StringValue))
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.StringValue, 2), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, StringValue, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read a PrimitiveTestPacket instance from the specified buffer using RNS Binary Spec v1
        /// </summary>
        /// <param name="buffer">The source buffer</param>
        /// <param name="readPacket">The read packet</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> buffer, out PrimitiveTestPacket readPacket, out int bytesRead)
        {
            readPacket = new PrimitiveTestPacket();
            bytesRead = 0;
            var src = buffer;

            while (!src.IsEmpty)
            {
                if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var tag, out var tr)) return false;
                src = src[tr..]; bytesRead += tr;

                var field = (int)(tag >> 3);
                var wt = (int)(tag & 7);

                switch (field)
                {
                    case Keys.IntValue when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var zz, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.IntValue = Serializer.Runtime.RnsCodec.UnZigZag32(zz);
                        break;
                    }
                    case Keys.FloatValue when wt == 5:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadFloat32(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.FloatValue = v;
                        break;
                    }
                    case Keys.BoolValue when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.BoolValue = v != 0;
                        break;
                    }
                    case Keys.StringValue when wt == 2:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.StringValue = s;
                        break;
                    }
                    default:
                        // Skip unknown
                        if (!SkipUnknown(ref src, ref bytesRead, wt)) return false;
                        break;
                }
            }

            return true;
        }

        private static bool SkipUnknown(ref System.ReadOnlySpan<byte> src, ref int consumed, int wt)
        {
            switch (wt)
            {
                case 0:
                    if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out _, out var r0)) return false;
                    src = src[r0..]; consumed += r0; return true;
                case 1:
                    if (src.Length < 8) return false;
                    src = src[8..]; consumed += 8; return true;
                case 2:
                    if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var len, out var r2)) return false;
                    src = src[r2..];
                    if (src.Length < len) return false;
                    src = src[(int)len..]; consumed += r2 + (int)len; return true;
                case 5:
                    if (src.Length < 4) return false;
                    src = src[4..]; consumed += 4; return true;
                default: return false;
            }
        }

        private static uint MakeTag(int f, int wt) => (uint)((f << 3) | wt);

    }
}
