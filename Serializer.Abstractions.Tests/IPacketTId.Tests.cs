using System;
using Xunit;
using Serializer.Abstractions;

namespace Serializer.Abstractions.Tests
{
    public sealed class IPacketTIdTests
    {
        [Fact]
        public void IPacketTIdCanBeImplementedWithIntId()
        {
            // Arrange & Act
            var packet = new TestPacketWithIntId();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
            Assert.IsAssignableFrom<IPacket<int>>(packet);
            Assert.Equal(42, packet.Id);
        }

        [Fact]
        public void IPacketTIdCanBeImplementedWithByteId()
        {
            // Arrange & Act
            var packet = new TestPacketWithByteId();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
            Assert.IsAssignableFrom<IPacket<byte>>(packet);
            Assert.Equal(255, packet.Id);
        }

        [Fact]
        public void IPacketTIdCanBeImplementedWithGuidId()
        {
            // Arrange & Act
            var expectedId = Guid.NewGuid();
            var packet = new TestPacketWithGuidId(expectedId);

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
            Assert.IsAssignableFrom<IPacket<Guid>>(packet);
            Assert.Equal(expectedId, packet.Id);
        }

        [Fact]
        public void IPacketTIdCanBeImplementedWithShortId()
        {
            // Arrange & Act
            var packet = new TestPacketWithShortId();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
            Assert.IsAssignableFrom<IPacket<short>>(packet);
            Assert.Equal(12345, packet.Id);
        }

        [Fact]
        public void IPacketTIdInheritsFromIPacket()
        {
            // Arrange
            var packet = new TestPacketWithIntId();

            // Assert
            // This test verifies that IPacket<TId> properly extends IPacket
            Assert.IsAssignableFrom<IPacket>(packet);
            Assert.IsAssignableFrom<IPacket<int>>(packet);
        }

        #region Test Implementations

        // IPacket<TId> implementations with different ID types
        private sealed class TestPacketWithIntId : IPacket<int>
        {
            public int Id => 42;
        }

        private sealed class TestPacketWithByteId : IPacket<byte>
        {
            public byte Id => 255;
        }

        private sealed class TestPacketWithGuidId : IPacket<Guid>
        {
            public Guid Id { get; }

            public TestPacketWithGuidId(Guid id)
            {
                Id = id;
            }
        }

        private sealed class TestPacketWithShortId : IPacket<short>
        {
            public short Id => 12345;
        }

        #endregion
    }
}
