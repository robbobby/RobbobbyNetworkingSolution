using Serializer.Abstractions;

namespace Sample.Packets.Packets
{
    /// <summary>
    /// Sample packet for chat messages
    /// </summary>
    [RnsSerializable]
    public partial class ChatMessagePacket : IRnsPacket<long>
    {
        public long Id { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public int MessageType { get; set; } // 0=Global, 1=Local, 2=Private
        public long Timestamp { get; set; }
    }
}
