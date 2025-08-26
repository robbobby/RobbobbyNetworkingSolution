using System;
using System.Collections.Generic;
using Serializer.Abstractions;
using Serializer.Runtime;

namespace Test.RnsPackets
{
    /// <summary>
    /// Packet type enumeration for testing
    /// </summary>
    public enum PacketType
    {
        Test = 1,
        TestField = 2,
        TestField2 = 3
    }

    /// <summary>
    /// Test packet following RNS Binary Spec v1
    /// </summary>
    public partial class TestPacket : IRnsPacket<PacketType>
    {
        public static class Keys
        {
            public const int PlayerName      = 1;
            public const int X               = 2;
            public const int Y               = 3;
            public const int Health          = 4;
            public const int IsAlive         = 5;
            public const int FieldPacket     = 6;
            public const int FieldPacketList = 7;
        }

        static TestPacket()
        {
            RnsKeyRegistry.Register<TestPacket>(new Dictionary<int, string>
            {
                { Keys.PlayerName, nameof(PlayerName) },
                { Keys.X, nameof(X) },
                { Keys.Y, nameof(Y) },
                { Keys.Health, nameof(Health) },
                { Keys.IsAlive, nameof(IsAlive) },
                { Keys.FieldPacket, nameof(FieldPacket) },
                { Keys.FieldPacketList, nameof(FieldPacketList) },
            });
        }

        public PacketType Id => PacketType.Test;

        public string PlayerName { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public int Health { get; set; }
        public bool IsAlive { get; set; }
        public TestFieldPacket? FieldPacket { get; set; }
        public TestFieldPacket[] FieldPacketList { get; set; } = [];

        public bool Write(Span<byte> buffer, out int bytesWritten)
        {
            bytesWritten = 0;
            int used = 0;
            var dst = buffer;

            // PlayerName (field 1, WT=2)
            if (!string.IsNullOrEmpty(PlayerName))
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.PlayerName, 2), out var w1)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w1..]; used += w1;
                if (!RnsCodec.TryWriteString(dst, PlayerName, out var w2)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w2..]; used += w2;
            }

            // X (field 2, WT=5)
            if (X != 0f)
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.X, 5), out var w1)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w1..]; used += w1;
                if (!RnsCodec.TryWriteFloat32(dst, X, out var w2)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w2..]; used += w2;
            }

            // Y (field 3, WT=5)
            if (Y != 0f)
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Y, 5), out var w1)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w1..]; used += w1;
                if (!RnsCodec.TryWriteFloat32(dst, Y, out var w2)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w2..]; used += w2;
            }

            // Health (field 4, WT=0 via ZigZag)
            if (Health != 0)
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.Health, 0), out var w1)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w1..]; used += w1;
                if (!RnsCodec.TryWriteVarUInt(dst, RnsCodec.ZigZag32(Health), out var w2)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w2..]; used += w2;
            }

            // IsAlive (field 5, WT=0)
            if (IsAlive)
            {
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.IsAlive, 0), out var w1)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w1..]; used += w1;
                if (!RnsCodec.TryWriteVarUInt(dst, 1u, out var w2)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w2..]; used += w2;
            }

            // FieldPacket (field 6, WT=2)
            if (FieldPacket is not null)
            {
                // write nested to temp
                Span<byte> tmp = stackalloc byte[4096]; // or use ArrayPool
                if (!FieldPacket.Write(tmp, out var nestedLen)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.FieldPacket, 2), out var w1)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w1..]; used += w1;
                if (!RnsCodec.TryWriteBlock(dst, tmp[..nestedLen], out var w2)) 
                { 
                    bytesWritten = 0; 
                    return false; 
                }
                dst = dst[w2..]; used += w2;
            }

            // FieldPacketList (field 7, WT=2) – packed objects
            if (FieldPacketList is { Length: > 0 })
            {
                // Build array payload
                Span<byte> arr = stackalloc byte[8192]; // or ArrayPool
                var arrUsed = 0;
                foreach (var el in FieldPacketList)
                {
                    if (el is null) continue;
                    Span<byte> elem = stackalloc byte[4096];
                    if (!el.Write(elem, out var elLen)) 
                    { 
                        bytesWritten = 0; 
                        return false; 
                    }

                    // length-delimit each element inside the array payload
                    if (!RnsCodec.TryWriteVarUInt(arr[arrUsed..], (uint)elLen, out var lw)) 
                    { 
                        bytesWritten = 0; 
                        return false; 
                    }
                    arrUsed += lw;
                    elem[..elLen].CopyTo(arr[arrUsed..]);
                    arrUsed += elLen;
                }

                if (arrUsed > 0)
                {
                    if (!RnsCodec.TryWriteVarUInt(dst, MakeTag(Keys.FieldPacketList, 2), out var w1)) 
                    { 
                        bytesWritten = 0; 
                        return false; 
                    }
                    dst = dst[w1..]; used += w1;
                    if (!RnsCodec.TryWriteVarUInt(dst, (uint)arrUsed, out var lw)) 
                    { 
                        bytesWritten = 0; 
                        return false; 
                    }
                    dst = dst[lw..]; used += lw;
                    if (dst.Length < arrUsed)
                    {
                        bytesWritten = 0; 
                        return false; 
                    }
                    arr[..arrUsed].CopyTo(dst);
                    dst = dst[arrUsed..]; used += arrUsed;
                }
            }

            bytesWritten = used;
            return true;
        }

        public static bool TryRead(ReadOnlySpan<byte> buffer, out TestPacket readPacket, out int bytesRead)
        {
            readPacket = new TestPacket();
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
                    case Keys.IsAlive when wt == 0:
                    {
                        if (!RnsCodec.TryReadVarUInt(src, out var b, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        readPacket.IsAlive = b != 0;
                        break;
                    }
                    case Keys.FieldPacket when wt == 2:
                    {
                        if (!RnsCodec.TryReadBlock(src, out var block, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        if (!TestFieldPacket.TryRead(block, out var fp, out var _)) return false;
                        readPacket.FieldPacket = fp;
                        break;
                    }
                    case Keys.FieldPacketList when wt == 2:
                    {
                        if (!RnsCodec.TryReadBlock(src, out var block, out var r)) return false;
                        src = src[r..]; bytesRead += r;
                        // parse sequence of length-delimited elements
                        var list = new List<TestFieldPacket>(4);
                        var inner = block;
                        while (!inner.IsEmpty)
                        {
                            if (!RnsCodec.TryReadVarUInt(inner, out var len, out var lr)) return false;
                            inner = inner[lr..];
                            if (len > (uint)inner.Length) return false;
                            var elem = inner.Slice(0, (int)len);
                            inner = inner[(int)len..];

                            if (!TestFieldPacket.TryRead(elem, out var obj, out var _)) return false;
                            list.Add(obj);
                        }
                        readPacket.FieldPacketList = list.ToArray();
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