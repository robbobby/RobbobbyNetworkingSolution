using System;
using Xunit;
using Serializer.Abstractions;

namespace Serializer.Abstractions.Tests
{
    internal sealed class InterfaceTests
    {
        [Fact]
        public void IPacketCanBeImplemented()
        {
            // Arrange & Act
            var packet = new TestPacket();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
        }

        [Fact]
        public void IPacketTIdCanBeImplemented()
        {
            // Arrange & Act
            var packet = new TestPacketWithId();

            // Assert
            Assert.IsAssignableFrom<IPacket>(packet);
            Assert.IsAssignableFrom<IPacket<int>>(packet);
            Assert.Equal(42, packet.Id);
        }

        [Fact]
        public void IBinaryWritableCanBeImplemented()
        {
            // Arrange & Act
            var writable = new TestBinaryWritable();

            // Assert
            Assert.IsAssignableFrom<IBinaryWritable>(writable);
            Assert.Equal(8, writable.GetSerializedSize());
        }

        [Fact]
        public void IBinaryWritableWriteMethodWorksCorrectly()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            var buffer = new byte[16];

            // Act
            var bytesWritten = writable.Write(buffer);

            // Assert
            Assert.Equal(8, bytesWritten);
            Assert.Equal(0x12, buffer[0]); // First byte
            Assert.Equal(0x34, buffer[1]); // Second byte
        }

        [Fact]
        public void BinarySerializableAttributeCanBeApplied()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializablePacket).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
        }

        [Fact]
        public void BinarySerializableAttributeCanBeAppliedToStruct()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializableStruct).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
        }

        // Test implementations
        private sealed class TestPacket : IPacket
        {
            // Marker interface implementation
        }

        private sealed class TestPacketWithId : IPacket<int>
        {
            public int Id => 42;
        }

        private sealed class TestBinaryWritable : IBinaryWritable
        {
            public int Write(Span<byte> destination)
            {
                if (destination.Length < 8)
                    throw new ArgumentException("Buffer too small");

                destination[0] = 0x12;
                destination[1] = 0x34;
                destination[2] = 0x56;
                destination[3] = 0x78;
                destination[4] = 0x9A;
                destination[5] = 0xBC;
                destination[6] = 0xDE;
                destination[7] = 0xF0;

                return 8;
            }

            public int GetSerializedSize() => 8;
        }

        [BinarySerializable]
        private sealed class TestSerializablePacket : IPacket
        {
            // Test class with attribute
        }

        [BinarySerializable]
        private struct TestSerializableStruct : IPacket
        {
            // Test struct with attribute
        }
    }
}
