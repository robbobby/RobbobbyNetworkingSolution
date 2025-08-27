using Xunit;

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
            Assert.IsAssignableFrom<IRnsPacket<int>>(packet);
            Assert.IsAssignableFrom<IRnsPacket<int>>(packet);
            Assert.IsAssignableFrom<IRnsBinaryWritable>(packet);
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
            var expectedSize = packet.GetSerializedSize();
            var bytesWritten = packet.Write(buffer);

            // Assert
            Assert.Equal(expectedSize, bytesWritten);
            Assert.Equal(123, packet.Id);
            Assert.Equal(16, packet.GetSerializedSize());
            // Verify buffer content: first 4 bytes encode Id, remaining pattern
            var idFromBuffer = System.BitConverter.ToInt32(buffer, 0);
            Assert.Equal(packet.Id, idFromBuffer);
            for (int i = 4; i < expectedSize; i++)
            {
                Assert.Equal((byte)(i * 2), buffer[i]);
            }
        }

        [Fact]
        public void MultiplePacketTypesCanShareSameIdType()
        {
            // Arrange
            var packet1 = new TestPacketWithIntId(42);
            var packet2 = new TestPacketWithIntId(99);

            // Assert
            Assert.IsAssignableFrom<IRnsPacket<int>>(packet1);
            Assert.IsAssignableFrom<IRnsPacket<int>>(packet2);
            Assert.Equal(42, packet1.Id);
            Assert.Equal(99, packet2.Id);
        }

        [Fact]
        public void AttributeCanBeAppliedToComplexInterfaceImplementation()
        {
            // Arrange & Act
            var attributes = typeof(TestFullPacket).GetCustomAttributes(typeof(RnsSerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<RnsSerializableAttribute>(attributes[0]);
        }

        #region Test Implementations

        // Full implementation combining all interfaces
        [RnsSerializable]
        private sealed class TestFullPacket : IRnsPacket<int>, IRnsBinaryWritable
        {
            public int Id => 123;

            public int Write(System.Span<byte> destination)
            {
                if (destination.Length < 16)
                    throw new System.ArgumentException("Buffer too small", nameof(destination));

                // Write ID (4 bytes, little-endian) + some data (12 bytes)
                System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(destination, Id);
                for (int i = 4; i < 16; i++)
                {
                    destination[i] = (byte)(i * 2);
                }

                return 16;
            }

            public int GetSerializedSize() => 16;
        }

        // Multiple packets with same ID type
        private sealed class TestPacketWithIntId : IRnsPacket<int>
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
