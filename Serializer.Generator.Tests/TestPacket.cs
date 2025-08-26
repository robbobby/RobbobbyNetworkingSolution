using Serializer.Abstractions;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Test packet for testing the source generator
    /// </summary>
    [RnsSerializable]
    public partial class TestPacket : IRnsPacket<int>
    {
        /// <summary>
        /// Gets or sets the packet identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player name
        /// </summary>
        public string PlayerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the X coordinate
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the player health
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets whether the player is alive
        /// </summary>
        public bool IsAlive { get; set; }
    }

    /// <summary>
    /// Test packet with enum values
    /// </summary>
    [RnsSerializable]
    public partial class EnumTestPacket : IRnsPacket<byte>
    {
        public enum PacketType
        {
            Unknown = 0,
            Login = 1,
            Logout = 2,
            Chat = 3,
            Movement = 4
        }

        public enum Status
        {
            Offline = 0,
            Online = 1,
            Away = 2,
            Busy = 3
        }

        public byte Id { get; set; }
        public PacketType Type { get; set; }
        public Status UserStatus { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Timestamp { get; set; }
    }

    /// <summary>
    /// Test packet with all primitive types
    /// </summary>
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
