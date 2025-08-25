using Xunit;
using Serializer.Abstractions;

namespace Serializer.Abstractions.Tests
{
    internal sealed class IPacketTests
    {
        [Fact]
        public void IPacketCanBeImplementedByClass()
        {
            // Arrange & Act
            var packet = new TestPacket();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
        }

        [Fact]
        public void IPacketCanBeImplementedByStruct()
        {
            // Arrange & Act
            var packet = new TestPacketStruct();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
        }

        [Fact]
        public void IPacketCanBeImplementedBySealedClass()
        {
            // Arrange & Act
            var packet = new TestSealedPacket();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
        }

        #region Test Implementations

        // Basic IPacket implementations
        private sealed class TestPacket : IPacket
        {
            // Marker interface implementation - no members required
        }

        private struct TestPacketStruct : IPacket
        {
            // Marker interface implementation - no members required
        }

        private sealed class TestSealedPacket : IPacket
        {
            // Sealed class implementation - no members required
        }

        #endregion
    }
}
