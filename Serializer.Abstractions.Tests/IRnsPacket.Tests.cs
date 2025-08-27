using Xunit;

namespace Serializer.Abstractions.Tests
{
    public sealed class IRnsPacketTests
    {
        public static IEnumerable<object[]> PacketInstances()
        {
            yield return new object[] { new TestPacket() };
            yield return new object[] { new TestPacketStruct() };
            yield return new object[] { new TestSealedPacket() };
        }

        [Theory]
        [MemberData(nameof(PacketInstances))]
        public void IRnsPacketIsAssignable(object packet)
        {
            // Assert
            Assert.IsAssignableFrom<IRnsPacket<int>>(packet);
        }

        #region Test Implementations

        // Basic IRnsPacket implementations
        private sealed class TestPacket : IRnsPacket<int>
        {
            // Marker interface implementation - no members required
            public int Id { get; } = 1;
        }

        private struct TestPacketStruct : IRnsPacket<int>
        {
            public TestPacketStruct()
            { }

            public int Id { get; } = 2;
            // Marker interface implementation - no members required
        }

        private sealed class TestSealedPacket : IRnsPacket<int>
        {
            public int Id { get; } = 3;
            // Sealed class implementation - no members required
        }

        #endregion
    }
}
