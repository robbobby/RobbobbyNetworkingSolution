using Serializer.Abstractions;

namespace Test.RnsPackets
{
    /// <summary>
    /// Sample packet for testing the RNS Binary Spec v1 generator
    /// </summary>
    [RnsSerializable]
    public partial class SamplePacket : IRnsPacket<int>
    {
        public int Id => 1;

        public string PlayerName { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public int Health { get; set; }
        public bool IsAlive { get; set; }
    }

    /// <summary>
    /// Another sample packet for testing nested objects
    /// </summary>
    [RnsSerializable]
    public partial class SampleFieldPacket : IRnsPacketField
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public float Score { get; set; }
    }
}