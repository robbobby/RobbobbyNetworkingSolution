using Serializer.Abstractions;

namespace Test.Packets.Models
{
    [RnsSerializable]
    public partial class SimpleTestPacket : IRnsPacket<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
