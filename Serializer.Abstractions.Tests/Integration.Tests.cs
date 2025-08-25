using Xunit;
using Serializer.Abstractions;

namespace Serializer.Abstractions.Tests
{
    public sealed class IntegrationTests
    {
        [Fact]
        public void AllInterfacesCanBeImplementedTogether()
        {
            // Arrange & Act
            var packet = new TestFullPacket();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
            Assert.IsAssignableFrom<IPacket<int>>(packet);
            Assert.IsAssignableFrom<IBinaryWritable>(packet);
            Assert.Equal(123, packet.Id);
            Assert.Equal(16, packet.GetSerializedSize());
        }

        [Fact]
        public void PacketWithBinaryWritableCanSerialize()
        {
            // Arrange
            var packet = new TestFullPacket();
            var buffer = new byte[32];

            // Act
            var bytesWritten = packet.Write(buffer);

            // Assert
            Assert.Equal(16, bytesWritten);
            Assert.Equal(123, packet.Id);
            Assert.Equal(16, packet.GetSerializedSize());
        }

        [Fact]
        public void MultiplePacketTypesCanShareSameIdType()
        {
            // Arrange
            var packet1 = new TestPacketWithIntId(42);
            var packet2 = new TestPacketWithIntId(99);

            // Assert
            Assert.IsAssignableFrom<IPacket<int>>(packet1);
            Assert.IsAssignableFrom<IPacket<int>>(packet2);
            Assert.Equal(42, packet1.Id);
            Assert.Equal(99, packet2.Id);
        }

        [Fact]
        public void AttributeCanBeAppliedToComplexInterfaceImplementation()
        {
            // Arrange & Act
            var attributes = typeof(TestFullPacket).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
        }

        #region Test Implementations

        // Full implementation combining all interfaces
        [BinarySerializable]
        private sealed class TestFullPacket : IPacket<int>, IBinaryWritable
        {
            public int Id => 123;

            public int Write(System.Span<byte> destination)
            {
                if (destination.Length < 16)
                    throw new System.ArgumentException("Buffer too small", nameof(destination));

                // Write ID (4 bytes) + some data (12 bytes)
                System.BitConverter.TryWriteBytes(destination, Id);
                for (int i = 4; i < 16; i++)
                {
                    destination[i] = (byte)(i * 2);
                }

                return 16;
            }

            public int GetSerializedSize() => 16;
        }

        // Multiple packets with same ID type
        private sealed class TestPacketWithIntId : IPacket<int>
        {
            public int Id { get; }

            public TestPacketWithIntId(int id)
            {
                Id = id;
            }
        }

        #endregion
    }
}
