using System;
using Xunit;
using Serializer.Runtime;

namespace Serializer.Runtime.Tests
{
    public sealed class BinarySerializerTests
    {
        #region Boolean Tests

        [Fact]
        public void WriteBoolean_True_WritesCorrectByte()
        {
            // Arrange
            var buffer = new byte[1];

            // Act
            var bytesWritten = BinarySerializer.WriteBoolean(buffer, true);

            // Assert
            Assert.Equal(1, bytesWritten);
            Assert.Equal(1, buffer[0]);
        }

        [Fact]
        public void WriteBoolean_False_WritesCorrectByte()
        {
            // Arrange
            var buffer = new byte[1];

            // Act
            var bytesWritten = BinarySerializer.WriteBoolean(buffer, false);

            // Assert
            Assert.Equal(1, bytesWritten);
            Assert.Equal(0, buffer[0]);
        }

        [Fact]
        public void WriteBoolean_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = Array.Empty<byte>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteBoolean(buffer, true));
            Assert.Contains("Buffer too small for boolean", exception.Message, StringComparison.Ordinal);
            Assert.Equal("destination", exception.ParamName);
        }

        [Fact]
        public void ReadBoolean_True_ReadsCorrectValue()
        {
            // Arrange
            var buffer = new byte[] { 1 };

            // Act
            var bytesRead = BinarySerializer.ReadBoolean(buffer, out var value);

            // Assert
            Assert.Equal(1, bytesRead);
            Assert.True(value);
        }

        [Fact]
        public void ReadBoolean_False_ReadsCorrectValue()
        {
            // Arrange
            var buffer = new byte[] { 0 };

            // Act
            var bytesRead = BinarySerializer.ReadBoolean(buffer, out var value);

            // Assert
            Assert.Equal(1, bytesRead);
            Assert.False(value);
        }

        [Fact]
        public void ReadBoolean_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = Array.Empty<byte>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.ReadBoolean(buffer, out _));
            Assert.Contains("Buffer too small for boolean", exception.Message, StringComparison.Ordinal);
            Assert.Equal("source", exception.ParamName);
        }

        #endregion

        #region Byte Tests

        [Fact]
        public void WriteByte_WritesCorrectValue()
        {
            // Arrange
            var buffer = new byte[1];
            var testValue = (byte)0xAB;

            // Act
            var bytesWritten = BinarySerializer.WriteByte(buffer, testValue);

            // Assert
            Assert.Equal(1, bytesWritten);
            Assert.Equal(testValue, buffer[0]);
        }

        [Fact]
        public void ReadByte_ReadsCorrectValue()
        {
            // Arrange
            var testValue = (byte)0xCD;
            var buffer = new byte[] { testValue };

            // Act
            var bytesRead = BinarySerializer.ReadByte(buffer, out var value);

            // Assert
            Assert.Equal(1, bytesRead);
            Assert.Equal(testValue, value);
        }

        #endregion

        #region SByte Tests

        [Fact]
        public void WriteSByte_WritesCorrectValue()
        {
            // Arrange
            var buffer = new byte[1];
            var testValue = (sbyte)-42;

            // Act
            var bytesWritten = BinarySerializer.WriteSByte(buffer, testValue);

            // Assert
            Assert.Equal(1, bytesWritten);
            Assert.Equal((byte)testValue, buffer[0]);
        }

        [Fact]
        public void ReadSByte_ReadsCorrectValue()
        {
            // Arrange
            var testValue = (sbyte)-123;
            var buffer = new byte[] { (byte)testValue };

            // Act
            var bytesRead = BinarySerializer.ReadSByte(buffer, out var value);

            // Assert
            Assert.Equal(1, bytesRead);
            Assert.Equal(testValue, value);
        }

        #endregion

        #region Int16 Tests

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)12345)]
        [InlineData((short)-12345)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        public void WriteInt16_ReadInt16_RoundTrip(short testValue)
        {
            // Arrange
            var buffer = new byte[2];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteInt16(buffer, testValue);
            Assert.Equal(2, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadInt16(buffer, out var readValue);

            // Assert
            Assert.Equal(2, bytesRead);
            Assert.Equal(testValue, readValue);
        }

        [Fact]
        public void WriteInt16_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[1];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteInt16(buffer, 12345));
            Assert.Contains("Buffer too small for Int16", exception.Message, StringComparison.Ordinal);
        }

        #endregion

        #region UInt16 Tests

        [Theory]
        [InlineData((ushort)0)]
        [InlineData((ushort)12345)]
        [InlineData((ushort)65535)]
        public void WriteUInt16_ReadUInt16_RoundTrip(ushort testValue)
        {
            // Arrange
            var buffer = new byte[2];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteUInt16(buffer, testValue);
            Assert.Equal(2, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadUInt16(buffer, out var readValue);

            // Assert
            Assert.Equal(2, bytesRead);
            Assert.Equal(testValue, readValue);
        }

        #endregion

        #region Int32 Tests

        [Theory]
        [InlineData(0)]
        [InlineData(123456789)]
        [InlineData(-123456789)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void WriteInt32_ReadInt32_RoundTrip(int testValue)
        {
            // Arrange
            var buffer = new byte[4];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteInt32(buffer, testValue);
            Assert.Equal(4, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadInt32(buffer, out var readValue);

            // Assert
            Assert.Equal(4, bytesRead);
            Assert.Equal(testValue, readValue);
        }

        [Fact]
        public void WriteInt32_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[3];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteInt32(buffer, 123456789));
            Assert.Contains("Buffer too small for Int32", exception.Message, StringComparison.Ordinal);
        }

        #endregion

        #region UInt32 Tests

        [Theory]
        [InlineData(0U)]
        [InlineData(123456789U)]
        [InlineData(uint.MaxValue)]
        public void WriteUInt32_ReadUInt32_RoundTrip(uint testValue)
        {
            // Arrange
            var buffer = new byte[4];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteUInt32(buffer, testValue);
            Assert.Equal(4, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadUInt32(buffer, out var readValue);

            // Assert
            Assert.Equal(4, bytesRead);
            Assert.Equal(testValue, readValue);
        }

        #endregion

        #region Int64 Tests

        [Theory]
        [InlineData(0L)]
        [InlineData(1234567890123456789L)]
        [InlineData(-1234567890123456789L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void WriteInt64_ReadInt64_RoundTrip(long testValue)
        {
            // Arrange
            var buffer = new byte[8];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteInt64(buffer, testValue);
            Assert.Equal(8, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadInt64(buffer, out var readValue);

            // Assert
            Assert.Equal(8, bytesRead);
            Assert.Equal(testValue, readValue);
        }

        [Fact]
        public void WriteInt64_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[7];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteInt64(buffer, 1234567890123456789L));
            Assert.Contains("Buffer too small for Int64", exception.Message, StringComparison.Ordinal);
        }

        #endregion

        #region UInt64 Tests

        [Theory]
        [InlineData(0UL)]
        [InlineData(1234567890123456789UL)]
        [InlineData(ulong.MaxValue)]
        public void WriteUInt64_ReadUInt64_RoundTrip(ulong testValue)
        {
            // Arrange
            var buffer = new byte[8];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteUInt64(buffer, testValue);
            Assert.Equal(8, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadUInt64(buffer, out var readValue);

            // Assert
            Assert.Equal(8, bytesRead);
            Assert.Equal(testValue, readValue);
        }

        #endregion

        #region Single Tests

        [Theory]
        [InlineData(0.0f)]
        [InlineData(3.14159f)]
        [InlineData(-2.71828f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        [InlineData(float.Epsilon)]
        public void WriteSingle_ReadSingle_RoundTrip(float testValue)
        {
            // Arrange
            var buffer = new byte[4];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteSingle(buffer, testValue);
            Assert.Equal(4, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadSingle(buffer, out var readValue);

            // Assert
            Assert.Equal(4, bytesRead);
            Assert.Equal(testValue, readValue);
        }

        [Fact]
        public void WriteSingle_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[3];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteSingle(buffer, 3.14159f));
            Assert.Contains("Buffer too small for Single", exception.Message, StringComparison.Ordinal);
        }

        #endregion

        #region Double Tests

        [Theory]
        [InlineData(0.0)]
        [InlineData(3.141592653589793)]
        [InlineData(-2.718281828459045)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(double.Epsilon)]
        public void WriteDouble_ReadDouble_RoundTrip(double testValue)
        {
            // Arrange
            var buffer = new byte[8];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteDouble(buffer, testValue);
            Assert.Equal(8, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadDouble(buffer, out var readValue);

            // Assert
            Assert.Equal(8, bytesRead);
            Assert.Equal(testValue, readValue);
        }

        [Fact]
        public void WriteDouble_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[7];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteDouble(buffer, 3.141592653589793));
            Assert.Contains("Buffer too small for Double", exception.Message, StringComparison.Ordinal);
        }

        #endregion

        #region Guid Tests

        [Fact]
        public void WriteGuid_ReadGuid_RoundTrip()
        {
            // Arrange
            var testGuid = Guid.NewGuid();
            var buffer = new byte[16];

            // Act - Write
            var bytesWritten = BinarySerializer.WriteGuid(buffer, testGuid);
            Assert.Equal(16, bytesWritten);

            // Act - Read
            var bytesRead = BinarySerializer.ReadGuid(buffer, out var readGuid);

            // Assert
            Assert.Equal(16, bytesRead);
            Assert.Equal(testGuid, readGuid);
        }

        [Fact]
        public void WriteGuid_EmptyGuid_WritesCorrectBytes()
        {
            // Arrange
            var emptyGuid = Guid.Empty;
            var buffer = new byte[16];

            // Act
            var bytesWritten = BinarySerializer.WriteGuid(buffer, emptyGuid);

            // Assert
            Assert.Equal(16, bytesWritten);
            // Empty GUID should write 16 zero bytes
            for (int i = 0; i < 16; i++)
            {
                Assert.Equal(0, buffer[i]);
            }
        }

        [Fact]
        public void WriteGuid_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[15];
            var testGuid = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteGuid(buffer, testGuid));
            Assert.Contains("Buffer too small for Guid", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void ReadGuid_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[15];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.ReadGuid(buffer, out _));
            Assert.Contains("Buffer too small for Guid", exception.Message, StringComparison.Ordinal);
        }

        #endregion

        #region Edge Cases and Error Handling

        [Fact]
        public void AllMethods_ReturnCorrectByteCounts()
        {
            // Test that all methods return the expected byte counts
            var buffer = new byte[32];

            Assert.Equal(1, BinarySerializer.WriteBoolean(buffer, true));
            Assert.Equal(1, BinarySerializer.WriteByte(buffer, 0x42));
            Assert.Equal(1, BinarySerializer.WriteSByte(buffer, -42));
            Assert.Equal(2, BinarySerializer.WriteInt16(buffer, 12345));
            Assert.Equal(2, BinarySerializer.WriteUInt16(buffer, 54321));
            Assert.Equal(4, BinarySerializer.WriteInt32(buffer, 123456789));
            Assert.Equal(4, BinarySerializer.WriteUInt32(buffer, 987654321));
            Assert.Equal(8, BinarySerializer.WriteInt64(buffer, 1234567890123456789L));
            Assert.Equal(8, BinarySerializer.WriteUInt64(buffer, 9876543210987654321UL));
            Assert.Equal(4, BinarySerializer.WriteSingle(buffer, 3.14159f));
            Assert.Equal(8, BinarySerializer.WriteDouble(buffer, 3.141592653589793));
            Assert.Equal(16, BinarySerializer.WriteGuid(buffer, Guid.NewGuid()));
        }

        [Fact]
        public void MethodsHandleMinMaxValues()
        {
            var buffer = new byte[32];
            var span = buffer.AsSpan();

            // Test min/max values for each type
            BinarySerializer.WriteInt16(span, short.MinValue);
            BinarySerializer.WriteInt16(span.Slice(2), short.MaxValue);
            BinarySerializer.WriteInt32(span.Slice(4), int.MinValue);
            BinarySerializer.WriteInt32(span.Slice(8), int.MaxValue);
            BinarySerializer.WriteInt64(span.Slice(12), long.MinValue);
            BinarySerializer.WriteInt64(span.Slice(20), long.MaxValue);

            // Read back and verify
            BinarySerializer.ReadInt16(span, out var minInt16);
            BinarySerializer.ReadInt16(span.Slice(2), out var maxInt16);
            BinarySerializer.ReadInt32(span.Slice(4), out var minInt32);
            BinarySerializer.ReadInt32(span.Slice(8), out var maxInt32);
            BinarySerializer.ReadInt64(span.Slice(12), out var minInt64);
            BinarySerializer.ReadInt64(span.Slice(20), out var maxInt64);

            Assert.Equal(short.MinValue, minInt16);
            Assert.Equal(short.MaxValue, maxInt16);
            Assert.Equal(int.MinValue, minInt32);
            Assert.Equal(int.MaxValue, maxInt32);
            Assert.Equal(long.MinValue, minInt64);
            Assert.Equal(long.MaxValue, maxInt64);
        }

        #endregion
    }
}
