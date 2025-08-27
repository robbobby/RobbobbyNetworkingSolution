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
            Assert.IsAssignableFrom<IRnsPacket>(packet);
        }

        #region Test Implementations

        // Basic IRnsPacket implementations
        private sealed class TestPacket : IRnsPacket
        {
            // Marker interface implementation - no members required
        }

        private struct TestPacketStruct : IRnsPacket
        {
            // Marker interface implementation - no members required
        }

        private sealed class TestSealedPacket : IRnsPacket
        {
            // Sealed class implementation - no members required
        }

        #endregion
    }
}
