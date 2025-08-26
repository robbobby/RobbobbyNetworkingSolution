using System;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Generated partial class for TestPacket with RNS Binary Spec v1 serialization support
    /// </summary>
    public partial class TestPacket
    {
        /// <summary>
        /// Compile-time field number constants for fast lookup
        /// </summary>
        public static class Keys
        {
            public const int PlayerName = 1;
            public const int X = 2;
            public const int Y = 3;
            public const int Health = 4;
            public const int IsAlive = 5;
            public const int FieldPacket = 6;
            public const int FieldPacketList = 7;
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

            // PlayerName (field 1, WT=2)
            if (!string.IsNullOrEmpty(PlayerName))
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.PlayerName, 2), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteString(dst, PlayerName, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // X (field 2, WT=5)
            if (X != 0f)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.X, 5), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteFloat32(dst, X, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // Y (field 3, WT=5)
            if (Y != 0f)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Y, 5), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteFloat32(dst, Y, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // Health (field 4, WT=0 via ZigZag)
            if (Health != 0)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Health, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, Serializer.Runtime.RnsCodec.ZigZag32(Health), out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            // IsAlive (field 5, WT=0)
            if (IsAlive)
            {
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.IsAlive, 0), out var w1)) { bytesWritten = 0; return false; }
                dst = dst[w1..]; bytesWritten += w1;
                if (!Serializer.Runtime.RnsCodec.TryWriteVarUInt(dst, 1u, out var w2)) { bytesWritten = 0; return false; }
                dst = dst[w2..]; bytesWritten += w2;
            }

            return true;
        }

        /// <summary>
        /// Attempts to read a TestPacket instance from the specified buffer using RNS Binary Spec v1
        /// </summary>
        /// <param name="buffer">The source buffer</param>
        /// <param name="readPacket">The read packet</param>
        /// <param name="bytesRead">The number of bytes read</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryRead(System.ReadOnlySpan<byte> buffer, out TestPacket readPacket, out int bytesRead)
        {
            readPacket = new TestPacket();
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
                    case Keys.PlayerName when wt == 2:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.PlayerName = s;
                        break;
                    }
                    case Keys.X when wt == 5:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadFloat32(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.X = v;
                        break;
                    }
                    case Keys.Y when wt == 5:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadFloat32(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.Y = v;
                        break;
                    }
                    case Keys.Health when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var zz, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.Health = Serializer.Runtime.RnsCodec.UnZigZag32(zz);
                        break;
                    }
                    case Keys.IsAlive when wt == 0:
                    {
                        if (!Serializer.Runtime.RnsCodec.TryReadVarUInt(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.IsAlive = v != 0;
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
