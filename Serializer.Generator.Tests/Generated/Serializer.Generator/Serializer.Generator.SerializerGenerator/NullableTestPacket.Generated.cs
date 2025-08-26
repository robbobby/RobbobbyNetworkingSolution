using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Generated partial class for NullableTestPacket with RNS Binary Spec v1 serialization support
    /// </summary>
    public partial class NullableTestPacket
    {
        /// <summary>
        /// Compile-time field number constants for fast lookup
        /// </summary>
        public static class Keys
        {
            public const int Id = 1;
            public const int NullableString = 2;
            public const int NonNullableString = 3;
            public const int NullableInt = 4;
            public const int NonNullableInt = 5;
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

            // Id (field 1, WT=0 via ZigZag)
            if (Id != 0)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Id, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, Serializer.Runtime.RnsCodec.ZigZag32(Id), out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // NullableString (field 2, WT=2)
            if (!string.IsNullOrEmpty(NullableString))
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.NullableString, 2), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, NullableString, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // NonNullableString (field 3, WT=2)
            if (!string.IsNullOrEmpty(NonNullableString))
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.NonNullableString, 2), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, NonNullableString, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // NonNullableInt (field 5, WT=0 via ZigZag)
            if (NonNullableInt != 0)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.NonNullableInt, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, Serializer.Runtime.RnsCodec.ZigZag32(NonNullableInt), out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read a NullableTestPacket instance from the specified buffer using RNS Binary Spec v1
        /// </summary>
        /// <param name="buffer">The source buffer</param>
        /// <param name="readPacket">The read packet</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> buffer, out NullableTestPacket readPacket, out int bytesRead)
        {
            readPacket = new NullableTestPacket();
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
                    case Keys.Id when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var zz, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.Id = Serializer.Runtime.RnsCodec.UnZigZag32(zz);
                        break;
                    }
                    case Keys.NullableString when wt == 2:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.NullableString = s;
                        break;
                    }
                    case Keys.NonNullableString when wt == 2:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.NonNullableString = s;
                        break;
                    }
                    case Keys.NonNullableInt when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var zz, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.NonNullableInt = Serializer.Runtime.RnsCodec.UnZigZag32(zz);
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
