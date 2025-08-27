using Xunit;

namespace Serializer.Abstractions.Tests
{
    public sealed class IRnsPacketTIdTests
    {
        [Fact]
        public void IRnsPacketTIdCanBeImplementedWithIntId()
        {
            // Arrange & Act
            var packet = new TestPacketWithIntId();

            // Assert
            Assert.IsAssignableFrom<IRnsPacket>(packet);
            Assert.IsAssignableFrom<IRnsPacket<int>>(packet);
            Assert.Equal(42, packet.Id);
        }

        [Fact]
        public void IRnsPacketTIdCanBeImplementedWithByteId()
        {
            // Arrange & Act
            var packet = new TestPacketWithByteId();

            // Assert
            Assert.IsAssignableFrom<IRnsPacket>(packet);
            Assert.IsAssignableFrom<IRnsPacket<byte>>(packet);
            Assert.IsType<byte>(packet.Id);
            Assert.Equal((byte)255, packet.Id);
        }

        [Fact]
        public void IRnsPacketTIdCanBeImplementedWithGuidId()
        {
            // Arrange & Act
            var expectedId = Guid.NewGuid();
            var packet = new TestPacketWithGuidId(expectedId);

            // Assert
            Assert.IsAssignableFrom<IRnsPacket>(packet);
            Assert.IsAssignableFrom<IRnsPacket<Guid>>(packet);
            Assert.Equal(expectedId, packet.Id);
        }

        [Fact]
        public void IRnsPacketTIdCanBeImplementedWithShortId()
        {
            // Arrange & Act
            var packet = new TestPacketWithShortId();

            // Assert
            Assert.IsAssignableFrom<IRnsPacket>(packet);
            Assert.IsAssignableFrom<IRnsPacket<short>>(packet);
            Assert.IsType<short>(packet.Id);
            Assert.Equal((short)12345, packet.Id);
        }

        [Fact]
        public void IRnsPacketTIdInheritsFromIRnsPacket()
        {
            // Arrange
            var packet = new TestPacketWithIntId();

            // Assert
            // This test verifies that IRnsPacket<TId> properly extends IRnsPacket
            Assert.IsAssignableFrom<IRnsPacket>(packet);
            Assert.IsAssignableFrom<IRnsPacket<int>>(packet);
        }

        #region Test Implementations

        // IRnsPacket<TId> implementations with different ID types
        private sealed class TestPacketWithIntId : IRnsPacket<int>
        {
            public int Id => 42;
        }

        private sealed class TestPacketWithByteId : IRnsPacket<byte>
        {
            public byte Id => 255;
        }

        private sealed class TestPacketWithGuidId : IRnsPacket<Guid>
        {
            public Guid Id { get; }

            public TestPacketWithGuidId(Guid id)
            {
                Id = id;
            }
        }

        private sealed class TestPacketWithShortId : IRnsPacket<short>
        {
            public short Id => 12345;
        }

        #endregion
    }
}
