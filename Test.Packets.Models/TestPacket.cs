using Serializer.Abstractions;

namespace Test.Packets.Models
{
    /// <summary>
    /// Test packet for testing the source generator - following problem statement specification
    /// </summary>
    public partial class TestPacket : IRnsPacket<PacketType>
    {
        public PacketType Id => PacketType.Test;
        public string PlayerName { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public int Health { get; set; }
        public bool IsAlive { get; set; }
        public TestFieldPacket? FieldPacket { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Test packet for generator")]
        public TestFieldPacket[] FieldPacketList { get; set; } = Array.Empty<TestFieldPacket>();
        
        // Additional properties for all template types
        public byte ByteValue { get; set; }
        public sbyte SByteValue { get; set; }
        public short Int16Value { get; set; }
        public ushort UInt16Value { get; set; }
        public uint UInt32Value { get; set; }
        public long Int64Value { get; set; }
        public ulong UInt64Value { get; set; }
        public double DoubleValue { get; set; }
        public Guid GuidValue { get; set; }
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
}
