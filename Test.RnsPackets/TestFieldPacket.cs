using System;
using System.Collections.Generic;
using Serializer.Abstractions;
using Serializer.Runtime;

namespace Test.RnsPackets
{
    /// <summary>
    /// Test field packet following RNS Binary Spec v1
    /// </summary>
    public partial class TestFieldPacket : IRnsPacketField
    {
        public static class Keys
        {
            public const int PlayerName = 1;
            public const int X          = 2;
            public const int Y          = 3;
            public const int Health     = 4;
            public const int FieldPacket2 = 5;
        }

        static TestFieldPacket()
        {
            RnsKeyRegistry.Register<TestFieldPacket>(new Dictionary<int, string>
            {
                { Keys.PlayerName, nameof(PlayerName) },
                { Keys.X, nameof(X) },
                { Keys.Y, nameof(Y) },
                { Keys.Health, nameof(Health) },
                { Keys.FieldPacket2, nameof(FieldPacket2) },
            });
        }

        public string PlayerName { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public int Health { get; set; }
        public TestFieldPacket2? FieldPacket2 { get; set; }

        public bool Write(Span<byte> buffer, out int bytesWritten)
        {
            bytesWritten = 0;
            var dst = buffer;

            // PlayerName (field 1, WT=2)
            if (!string.IsNullOrEmpty(PlayerName))
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.PlayerName, 2), out var w1)) return false;
                dst = dst[w1..]; bytesWritten += w1;
                if (!RnsCodec.TryWriteString(dst, PlayerName, out var w2)) return false;
                dst = dst[w2..]; bytesWritten += w2;
            }

            // X (field 2, WT=5)
            if (X != 0f)
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.X, 5), out var w1)) return false;
                dst = dst[w1..]; bytesWritten += w1;
                if (!RnsCodec.TryWriteFloat32(dst, X, out var w2)) return false;
                dst = dst[w2..]; bytesWritten += w2;
            }

            // Y (field 3, WT=5)
            if (Y != 0f)
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Y, 5), out var w1)) return false;
                dst = dst[w1..]; bytesWritten += w1;
                if (!RnsCodec.TryWriteFloat32(dst, Y, out var w2)) return false;
                dst = dst[w2..]; bytesWritten += w2;
            }

            // Health (field 4, WT=0 via ZigZag)
            if (Health != 0)
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Health, 0), out var w1)) return false;
                dst = dst[w1..]; bytesWritten += w1;
                if (!RnsCodec.TryWriteVarUInt(dst, RnsCodec.ZigZag32(Health), out var w2)) return false;
                dst = dst[w2..]; bytesWritten += w2;
            }

            // FieldPacket2 (field 5, WT=2)
            if (FieldPacket2 is not null)
            {
                // write nested to temp
                Span<byte> tmp = stackalloc byte[4096]; // or use ArrayPool
                if (!FieldPacket2.Write(tmp, out var nestedLen)) return false;
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.FieldPacket2, 2), out var w1)) return false;
                dst = dst[w1..]; bytesWritten += w1;
                if (!RnsCodec.TryWriteBlock(dst, tmp[..nestedLen], out var w2)) return false;
                dst = dst[w2..]; bytesWritten += w2;
            }

            return true;
        }

        public static bool TryRead(ReadOnlySpan<byte> buffer, out TestFieldPacket readPacket, out int bytesRead)
        {
            readPacket = new TestFieldPacket();
            bytesRead = 0;
            var src = buffer;

            while (!src.IsEmpty)
            {
                if (!RnsCodec.TryReadVarUInt(src, out var tag, out var tr)) return false;
                src = src[tr..]; bytesRead += tr;

                var field = (int)(tag >> 3);
                var wt = (int)(tag & 7);

                switch (field)
                {
                    case Keys.PlayerName when wt == 2:
                    {
                        if (!RnsCodec.TryReadString(src, out var s, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.PlayerName = s;
                        break;
                    }
                    case Keys.X when wt == 5:
                    {
                        if (!RnsCodec.TryReadFloat32(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.X = v;
                        break;
                    }
                    case Keys.Y when wt == 5:
                    {
                        if (!RnsCodec.TryReadFloat32(src, out var v, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.Y = v;
                        break;
                    }
                    case Keys.Health when wt == 0:
                    {
                        if (!RnsCodec.TryReadVarUInt(src, out var zz, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.Health = RnsCodec.UnZigZag32(zz);
                        break;
                    }
                    case Keys.FieldPacket2 when wt == 2:
                    {
                        if (!RnsCodec.TryReadBlock(src, out var block, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        if (!TestFieldPacket2.TryRead(block, out var fp, out var _)) return false;
                        readPacket.FieldPacket2 = fp;
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

        private static bool SkipUnknown(ref ReadOnlySpan<byte> src, ref int consumed, int wt)
        {
            switch (wt)
            {
                case 0:
                    if (!RnsCodec.TryReadVarUInt(src, out _, out var r0)) return false;
                    src = src[r0..]; consumed += r0; return true;
                case 1:
                    if (src.Length < 8) return false;
                    src = src[8..]; consumed += 8; return true;
                case 2:
                    if (!RnsCodec.TryReadVarUInt(src, out var len, out var r2)) return false;
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