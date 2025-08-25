using Xunit;
using Serializer.Abstractions;

namespace Serializer.Abstractions.Tests
{
    public sealed class IPacketTests
    {
        [Fact]
        public void IRnsPacketCanBeImplementedByClass()
        {
            // Arrange & Act
            var packet = new TestPacket();

            // Assert
            Assert.IsAssignableFrom<IRnsPacket>(packet);
        }

        [Fact]
        public void IRnsPacketCanBeImplementedByStruct()
        {
            // Arrange & Act
            var packet = new TestPacketStruct();

            // Assert
            Assert.IsAssignableFrom<IRnsPacket>(packet);
        }

        [Fact]
        public void IRnsPacketCanBeImplementedBySealedClass()
        {
            // Arrange & Act
            var packet = new TestSealedPacket();

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
