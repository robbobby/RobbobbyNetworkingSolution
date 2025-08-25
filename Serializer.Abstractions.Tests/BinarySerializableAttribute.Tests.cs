using Xunit;
using Serializer.Abstractions;

namespace Serializer.Abstractions.Tests
{
    public sealed class BinarySerializableAttributeTests
    {
        [Fact]
        public void AttributeCanBeAppliedToClass()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializableClass).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
        }

        [Fact]
        public void AttributeCanBeAppliedToStruct()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializableStruct).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
        }

        [Fact]
        public void AttributeCanBeAppliedToSealedClass()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializableSealedClass).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
        }

        [Fact]
        public void AttributeCanBeAppliedToClassImplementingIPacket()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializablePacket).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
            Assert.IsAssignableFrom<IPacket>(new TestSerializablePacket());
        }

        [Fact]
        public void AttributeCanBeAppliedToClassImplementingMultipleInterfaces()
        {
            // Arrange & Act
            var attributes = typeof(TestFullPacket).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
            Assert.IsAssignableFrom<IPacket>(new TestFullPacket());
            Assert.IsAssignableFrom<IPacket<int>>(new TestFullPacket());
            Assert.IsAssignableFrom<IBinaryWritable>(new TestFullPacket());
        }

        [Fact]
        public void AttributeCanBeAppliedToEmptyClass()
        {
            // Arrange & Act
            var attributes = typeof(TestEmptySerializableClass).GetCustomAttributes(typeof(BinarySerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<BinarySerializableAttribute>(attributes[0]);
        }

        #region Test Implementations

        // BinarySerializableAttribute test classes
        [BinarySerializable]
        private sealed class TestSerializableClass
        {
            // Test class with attribute
        }

        [BinarySerializable]
        private struct TestSerializableStruct
        {
            // Test struct with attribute
        }

        [BinarySerializable]
        private sealed class TestSerializableSealedClass
        {
            // Test sealed class with attribute
        }

        [BinarySerializable]
        private sealed class TestSerializablePacket : IPacket
        {
            // Test class with attribute implementing IPacket
        }

        [BinarySerializable]
        private sealed class TestEmptySerializableClass
        {
            // Empty class with attribute
        }

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

        #endregion
    }
}
