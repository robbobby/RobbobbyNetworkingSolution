using Serializer.Abstractions;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Test packet for testing the source generator
    /// </summary>
    [RnsSerializable]
    public partial class TestPacket : IRnsPacket<PacketType>
    {
        public static class Keys
        {
            public const int PlayerName = 1;
            public const int X = 2;
            public const int Y = 3;
            public const int Health = 4;
            public const int IsAlive = 5;
            public const int FieldPacket = 6;
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
            // id does not have a key
            // write method writing to the buffer using the keys, if anything is default value, empty string or null or empty length it doesn't write the key or the value.
            throw new System.NotImplementedException();
        }

        public bool TryRead(ReadOnlySpan<byte> buffer, out TestPacket readPacket, out int bytesRead)
        {
            // we have already read the key
            // reads from the buffer and creates a new TestPacket instance with the values present in the buffer
            // if it has any fields in it, it calls the Field.TryRead method on the field to read the field
            throw new System.NotImplementedException();
        }
    }

    public partial class TestFieldPacket : IRnsPacketField
    {
        public string PlayerName { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public int Health { get; set; }
        public TestFieldPacket2? FieldPacket2 { get; set; }
    }

    public partial class TestFieldPacket2 : IRnsPacketField
    {
        public string PlayerName { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public int Health { get; set; }
    }

    public enum PacketType
    {
        Unknown = 0,
        Login = 1,
        Logout = 2,
        Chat = 3,
        Movement = 4,
        Test = 5
    }

    [RnsSerializable]
    public partial class PrimitiveTestPacket : IRnsPacket<long>
    {
        public long Id { get; set; }
        public byte ByteValue { get; set; }
        public sbyte SByteValue { get; set; }
        public short ShortValue { get; set; }
        public ushort UShortValue { get; set; }
        public int IntValue { get; set; }
        public uint UIntValue { get; set; }
        public long LongValue { get; set; }
        public ulong ULongValue { get; set; }
        public float FloatValue { get; set; }
        public double DoubleValue { get; set; }
        public bool BoolValue { get; set; }
        public string StringValue { get; set; } = string.Empty;
    }

    /// <summary>
    /// Test packet with edge case values
    /// </summary>
    [RnsSerializable]
    public partial class EdgeCaseTestPacket : IRnsPacket<string>
    {
        public string Id { get; set; } = string.Empty;
        public int MaxInt { get; set; }
        public int MinInt { get; set; }
        public float MaxFloat { get; set; }
        public float MinFloat { get; set; }
        public double MaxDouble { get; set; }
        public double MinDouble { get; set; }
        public string EmptyString { get; set; } = string.Empty;
        public string LongString { get; set; } = string.Empty;
        public bool TrueValue { get; set; }
        public bool FalseValue { get; set; }
    }

    /// <summary>
    /// Test packet with nullable reference types
    /// </summary>
    [RnsSerializable]
    public partial class NullableTestPacket : IRnsPacket<int>
    {
        public int Id { get; set; }
        public string? NullableString { get; set; }
        public string NonNullableString { get; set; } = string.Empty;
        public int? NullableInt { get; set; }
        public int NonNullableInt { get; set; }
    }
}
