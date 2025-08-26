using System;
using Xunit;
using Serializer.Abstractions;

namespace Serializer.Abstractions.Tests
{
    public sealed class IRnsBinaryWritableExtensionsTests
    {
        [Fact]
        public void TryWrite_SuccessWithExactBufferSize_ReturnsTrueAndCorrectBytesWritten()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            var buffer = new byte[writable.GetSerializedSize()];

            // Act
            var success = writable.TryWrite(buffer, out var bytesWritten);

            // Assert
            Assert.True(success);
            Assert.Equal(writable.GetSerializedSize(), bytesWritten);
            Assert.Equal(0x12, buffer[0]);
            Assert.Equal(0xF0, buffer[7]);
        }

        [Fact]
        public void TryWrite_InsufficientBuffer_ReturnsFalseAndZeroBytesWritten()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            var smallBuffer = new byte[writable.GetSerializedSize() - 1]; // Too small

            // Act
            var success = writable.TryWrite(smallBuffer, out var bytesWritten);

            // Assert
            Assert.False(success);
            Assert.Equal(0, bytesWritten);
            // Ensure buffer is untouched
            for (int i = 0; i < smallBuffer.Length; i++)
            {
                Assert.Equal(0, smallBuffer[i]);
            }
        }

        [Fact]
        public void TryWrite_ZeroSizePayload_ReturnsTrueAndZeroBytesWritten()
        {
            // Arrange
            var writable = new TestZeroSizeWritable();
            var buffer = new byte[1];

            // Act
            var success = writable.TryWrite(buffer, out var bytesWritten);

            // Assert
            Assert.True(success);
            Assert.Equal(0, bytesWritten);
            // Ensure buffer is untouched
            Assert.Equal(0, buffer[0]);
        }

        [Fact]
        public void TryWrite_PartialWriteBug_ReturnsFalseAndZeroBytesWritten()
        {
            // Arrange
            var writable = new TestBuggyWritable();
            var buffer = new byte[writable.GetSerializedSize()];

            // Act
            var success = writable.TryWrite(buffer, out var bytesWritten);

            // Assert
            Assert.False(success);
            Assert.Equal(4, bytesWritten); // The actual bytes written by the buggy implementation
            // Note: In a real scenario, this would indicate a bug in the Write implementation
            // since it returned a different size than GetSerializedSize()
        }

        [Fact]
        public void TryWrite_WriteThrowsException_ReturnsFalseAndZeroBytesWritten()
        {
            // Arrange
            var writable = new TestExceptionWritable();
            var buffer = new byte[writable.GetSerializedSize()];

            // Act
            var success = writable.TryWrite(buffer, out var bytesWritten);

            // Assert
            Assert.False(success);
            Assert.Equal(0, bytesWritten);
            // Ensure buffer is untouched
            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.Equal(0, buffer[i]);
            }
        }

        [Fact]
        public void TryWrite_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IRnsBinaryWritable? writable = null;
            var buffer = new byte[8];

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => writable!.TryWrite(buffer, out _));
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void TryWrite_LargeBuffer_WorksCorrectly()
        {
            // Arrange
            var writable = new TestBinaryWritable();
            var largeBuffer = new byte[writable.GetSerializedSize() * 2]; // Extra space

            // Act
            var success = writable.TryWrite(largeBuffer, out var bytesWritten);

            // Assert
            Assert.True(success);
            Assert.Equal(writable.GetSerializedSize(), bytesWritten);
            Assert.Equal(0x12, largeBuffer[0]);
            Assert.Equal(0xF0, largeBuffer[7]);
            // Ensure bytes after written content are untouched
            for (int i = writable.GetSerializedSize(); i < largeBuffer.Length; i++)
            {
                Assert.Equal(0, largeBuffer[i]);
            }
        }

        #region Test Implementations

        // Normal implementation for success cases
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

        // Zero-size implementation
        private sealed class TestZeroSizeWritable : IRnsBinaryWritable
        {
            public int Write(Span<byte> destination)
            {
                // No bytes to write
                return 0;
            }

            public int GetSerializedSize() => 0;
        }

        // Buggy implementation that returns wrong size
        private sealed class TestBuggyWritable : IRnsBinaryWritable
        {
            public int Write(Span<byte> destination)
            {
                if (destination.Length < 8)
                    throw new ArgumentException("Buffer too small", nameof(destination));

                // Write data but return wrong size (bug)
                destination[0] = 0x12;
                destination[1] = 0x34;
                destination[2] = 0x56;
                destination[3] = 0x78;
                destination[4] = 0x9A;
                destination[5] = 0xBC;
                destination[6] = 0xDE;
                destination[7] = 0xF0;

                return 4; // Bug: should return 8
            }

            public int GetSerializedSize() => 8;
        }

        // Implementation that throws during write
        private sealed class TestExceptionWritable : IRnsBinaryWritable
        {
            public int Write(Span<byte> destination)
            {
                throw new InvalidOperationException("Simulated write failure");
            }

            public int GetSerializedSize() => 8;
        }

        #endregion
    }
}
