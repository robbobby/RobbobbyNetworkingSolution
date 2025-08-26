using Serializer.Abstractions;

namespace Sample.Packets.Packets
{
    /// <summary>
    /// Sample packet for player movement
    /// </summary>
    [RnsSerializable]
    public partial class PlayerMovePacket : IRnsPacket<int>
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool IsRunning { get; set; }
        public int Timestamp { get; set; }
    }
}
