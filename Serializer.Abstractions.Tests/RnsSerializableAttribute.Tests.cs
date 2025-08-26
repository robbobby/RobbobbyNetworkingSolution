using Xunit;
using Serializer.Abstractions;

namespace Serializer.Abstractions.Tests
{
    public sealed class RnsSerializableAttributeTests
    {
        [Fact]
        public void AttributeCanBeAppliedToClass()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializableClass).GetCustomAttributes(typeof(RnsSerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<RnsSerializableAttribute>(attributes[0]);
        }

        [Fact]
        public void AttributeCanBeAppliedToStruct()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializableStruct).GetCustomAttributes(typeof(RnsSerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<RnsSerializableAttribute>(attributes[0]);
        }

        [Fact]
        public void AttributeCanBeAppliedToSealedClass()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializableSealedClass).GetCustomAttributes(typeof(RnsSerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<RnsSerializableAttribute>(attributes[0]);
        }

        [Fact]
        public void AttributeCanBeAppliedToClassImplementingIRnsPacket()
        {
            // Arrange & Act
            var attributes = typeof(TestSerializablePacket).GetCustomAttributes(typeof(RnsSerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<RnsSerializableAttribute>(attributes[0]);
            Assert.IsAssignableFrom<IRnsPacket>(new TestSerializablePacket());
        }

        [Fact]
        public void AttributeCanBeAppliedToClassImplementingMultipleInterfaces()
        {
            // Arrange & Act
            var attributes = typeof(TestFullPacket).GetCustomAttributes(typeof(RnsSerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<RnsSerializableAttribute>(attributes[0]);
            var instance = new TestFullPacket();
            Assert.IsAssignableFrom<IRnsPacket>(instance);
            Assert.IsAssignableFrom<IRnsPacket<int>>(instance);
            Assert.IsAssignableFrom<IRnsBinaryWritable>(instance);
        }

        [Fact]
        public void AttributeCanBeAppliedToEmptyClass()
        {
            // Arrange & Act
            var attributes = typeof(TestEmptySerializableClass).GetCustomAttributes(typeof(RnsSerializableAttribute), false);

            // Assert
            Assert.Single(attributes);
            Assert.IsType<RnsSerializableAttribute>(attributes[0]);
        }

        #region Test Implementations

        // RnsSerializableAttribute test classes
        [RnsSerializable]
        private sealed class TestSerializableClass
        {
            // Test class with attribute
        }

        [RnsSerializable]
        private struct TestSerializableStruct
        {
            // Test struct with attribute
        }

        [RnsSerializable]
        private sealed class TestSerializableSealedClass
        {
            // Test sealed class with attribute
        }

        [RnsSerializable]
        private sealed class TestSerializablePacket : IRnsPacket
        {
            // Test class with attribute implementing IRnsPacket
        }

        [RnsSerializable]
        private sealed class TestEmptySerializableClass
        {
            // Empty class with attribute
        }

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

        #endregion
    }
}
