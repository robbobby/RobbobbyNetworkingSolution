using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Generated partial class for EdgeCaseTestPacket with RNS Binary Spec v1 serialization support
    /// </summary>
    public partial class EdgeCaseTestPacket
    {
        /// <summary>
        /// Compile-time field number constants for fast lookup
        /// </summary>
        public static class Keys
        {
            public const int Id = 1;
            public const int MaxInt = 2;
            public const int MinInt = 3;
            public const int MaxFloat = 4;
            public const int MinFloat = 5;
            public const int MaxDouble = 6;
            public const int MinDouble = 7;
            public const int EmptyString = 8;
            public const int LongString = 9;
            public const int TrueValue = 10;
            public const int FalseValue = 11;
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

            // Id (field 1, WT=2)
            if (!string.IsNullOrEmpty(Id))
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Id, 2), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, Id, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // MaxInt (field 2, WT=0 via ZigZag)
            if (MaxInt != 0)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.MaxInt, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, Serializer.Runtime.RnsCodec.ZigZag32(MaxInt), out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // MinInt (field 3, WT=0 via ZigZag)
            if (MinInt != 0)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.MinInt, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, Serializer.Runtime.RnsCodec.ZigZag32(MinInt), out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // MaxFloat (field 4, WT=5)
            if (MaxFloat != 0f)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.MaxFloat, 5), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteFloat32(dst, MaxFloat, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // MinFloat (field 5, WT=5)
            if (MinFloat != 0f)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.MinFloat, 5), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteFloat32(dst, MinFloat, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // EmptyString (field 8, WT=2)
            if (!string.IsNullOrEmpty(EmptyString))
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.EmptyString, 2), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, EmptyString, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // LongString (field 9, WT=2)
            if (!string.IsNullOrEmpty(LongString))
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.LongString, 2), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, LongString, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // TrueValue (field 10, WT=0)
            if (TrueValue)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.TrueValue, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, 1u, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // FalseValue (field 11, WT=0)
            if (FalseValue)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.FalseValue, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, 1u, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read a EdgeCaseTestPacket instance from the specified buffer using RNS Binary Spec v1
        /// </summary>
        /// <param name="buffer">The source buffer</param>
        /// <param name="readPacket">The read packet</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> buffer, out EdgeCaseTestPacket readPacket, out int bytesRead)
        {
            readPacket = new EdgeCaseTestPacket();
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
                    case Keys.Id when wt == 2:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.Id = s;
                        break;
                    }
                    case Keys.MaxInt when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var zz, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.MaxInt = Serializer.Runtime.RnsCodec.UnZigZag32(zz);
                        break;
                    }
                    case Keys.MinInt when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var zz, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.MinInt = Serializer.Runtime.RnsCodec.UnZigZag32(zz);
                        break;
                    }
                    case Keys.MaxFloat when wt == 5:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadFloat32(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.MaxFloat = v;
                        break;
                    }
                    case Keys.MinFloat when wt == 5:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadFloat32(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.MinFloat = v;
                        break;
                    }
                    case Keys.EmptyString when wt == 2:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.EmptyString = s;
                        break;
                    }
                    case Keys.LongString when wt == 2:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.LongString = s;
                        break;
                    }
                    case Keys.TrueValue when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.TrueValue = v != 0;
                        break;
                    }
                    case Keys.FalseValue when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.FalseValue = v != 0;
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
