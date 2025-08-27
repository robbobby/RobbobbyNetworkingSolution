using Xunit;

namespace Serializer.Abstractions.Tests
{
    public sealed class IBinaryWritableTests
    {
        [Fact]
        public void IRnsBinaryWritableCanBeImplemented()
        {
            // Arrange & Act
            var writable = new TestBinaryWritable();

            // Assert
            Assert.IsAssignableFrom<IRnsBinaryWritable>(writable);
            Assert.Equal(8, writable.GetSerializedSize());
        }

        [Fact]
        public void WriteMethodReturnsCorrectNumberOfBytes()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            var buffer = new byte[16];

            // Act
            var bytesWritten = writable.Write(buffer);

            // Assert
            Assert.Equal(writable.GetSerializedSize(), bytesWritten);
        }

        [Fact]
        public void WriteMethodActuallyWritesDataToBuffer()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            var buffer = new byte[16];

            // Act
            writable.Write(buffer);

            // Assert
            Assert.Equal(0x12, buffer[0]); // First byte
            Assert.Equal(0x34, buffer[1]); // Second byte
            Assert.Equal(0x56, buffer[2]); // Third byte
            Assert.Equal(0x78, buffer[3]); // Fourth byte
            // Ensure bytes after the written span are untouched
            for (int i = 8; i < buffer.Length; i++)
            {
                Assert.Equal(0, buffer[i]);
            }
        }

        [Fact]
        public void GetSerializedSizeReturnsCorrectSize()
        {
            // Arrange
            var writable = new TestBinaryWritable();

            // Act
            var size = writable.GetSerializedSize();

            // Assert
            Assert.Equal(8, size);
        }

        [Fact]
        public void WriteMethodThrowsWhenBufferTooSmall()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            var smallBuffer = new byte[4]; // Too small for 8 bytes

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => writable.Write(smallBuffer));
            Assert.Contains("Buffer too small", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void WriteMethodHandlesExactBufferSize()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            var exactBuffer = new byte[8]; // Exactly the right size

            // Act
            var bytesWritten = writable.Write(exactBuffer);

            // Assert
            Assert.Equal(8, bytesWritten);
            Assert.Equal(0x12, exactBuffer[0]);
            Assert.Equal(0xF0, exactBuffer[7]);
        }

        [Fact]
        public void WriteSupportsStackallocSpan()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            Span<byte> stackSpan = stackalloc byte[writable.GetSerializedSize()];

            // Act
            var bytesWritten = writable.Write(stackSpan);

            // Assert
            Assert.Equal(writable.GetSerializedSize(), bytesWritten);
            Assert.Equal(0x12, stackSpan[0]);
            Assert.Equal(0xF0, stackSpan[7]);
        }

        #region Test Implementations

        // IRnsBinaryWritable implementation
        private sealed class TestBinaryWritable : IRnsBinaryWritable
        {
            public int Write(Span<byte> destination)
            {
                if (destination.Length < 8)
                    throw new ArgumentException("Buffer too small", nameof(destination));

                // Write a predictable pattern for testing
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

        #endregion
    }
}
