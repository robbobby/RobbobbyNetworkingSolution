using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Generated partial class for SimpleTestPacket with RNS Binary Spec v1 serialization support
    /// </summary>
    public partial class SimpleTestPacket
    {
        /// <summary>
        /// Compile-time field number constants for fast lookup
        /// </summary>
        public static class Keys
        {
            public const int Id = 1;
            public const int Name = 2;
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

            // Name (field 2, WT=2)
            if (!string.IsNullOrEmpty(Name))
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Name, 2), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, Name, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read a SimpleTestPacket instance from the specified buffer using RNS Binary Spec v1
        /// </summary>
        /// <param name="buffer">The source buffer</param>
        /// <param name="readPacket">The read packet</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> buffer, out SimpleTestPacket readPacket, out int bytesRead)
        {
            readPacket = new SimpleTestPacket();
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
                    case Keys.Name when wt == 2:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.Name = s;
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
