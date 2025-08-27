using Serializer.Abstractions;

namespace Serializer.Generator.Tests
{
    [RnsSerializable]
    public partial class SimpleTestPacket : IRnsPacket<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
