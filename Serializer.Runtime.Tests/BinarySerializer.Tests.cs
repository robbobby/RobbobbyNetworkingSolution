using System;
using Xunit;
using Serializer.Runtime;

namespace Serializer.Runtime.Tests
{
    [Trait("Category", "Unit")]
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

        [Fact]
        public void ReadBooleanStrict_ValidValues_ReadsCorrectly()
        {
            // Arrange
            var buffer = new byte[1];

            // Test true (1)
            buffer[0] = 1;
            var result = BinarySerializer.ReadBooleanStrict(buffer, out var value);
            Assert.Equal(1, result);
            Assert.True(value);

            // Test false (0)
            buffer[0] = 0;
            result = BinarySerializer.ReadBooleanStrict(buffer, out value);
            Assert.Equal(1, result);
            Assert.False(value);
        }

        [Fact]
        public void ReadBooleanStrict_InvalidValue_ThrowsFormatException()
        {
            // Arrange
            var buffer = new byte[1];
            buffer[0] = 42; // Invalid boolean value

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => BinarySerializer.ReadBooleanStrict(buffer, out _));
            Assert.Contains("Invalid boolean encoding: expected 0 or 1, got 42", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void ReadBooleanStrict_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = Array.Empty<byte>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.ReadBooleanStrict(buffer, out _));
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

        [Fact]
        public void WriteByte_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = Array.Empty<byte>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteByte(buffer, 0x12));
            Assert.Contains("Buffer too small for byte", exception.Message, StringComparison.Ordinal);
            Assert.Equal("destination", exception.ParamName);
        }

        [Fact]
        public void ReadByte_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = Array.Empty<byte>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.ReadByte(buffer, out _));
            Assert.Contains("Buffer too small for byte", exception.Message, StringComparison.Ordinal);
            Assert.Equal("source", exception.ParamName);
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

        [Fact]
        public void WriteUInt16_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[1];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteUInt16(buffer, 12345));
            Assert.Contains("Buffer too small for UInt16", exception.Message, StringComparison.Ordinal);
            Assert.Equal("destination", exception.ParamName);
        }

        [Fact]
        public void ReadUInt16_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[1];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.ReadUInt16(buffer, out _));
            Assert.Contains("Buffer too small for UInt16", exception.Message, StringComparison.Ordinal);
            Assert.Equal("source", exception.ParamName);
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

        [Fact]
        public void WriteUInt32_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[3];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteUInt32(buffer, 123456789U));
            Assert.Contains("Buffer too small for UInt32", exception.Message, StringComparison.Ordinal);
            Assert.Equal("destination", exception.ParamName);
        }

        [Fact]
        public void ReadUInt32_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[3];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.ReadUInt32(buffer, out _));
            Assert.Contains("Buffer too small for UInt32", exception.Message, StringComparison.Ordinal);
            Assert.Equal("source", exception.ParamName);
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

        [Fact]
        public void WriteUInt64_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[7];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteUInt64(buffer, 1234567890123456789UL));
            Assert.Contains("Buffer too small for UInt64", exception.Message, StringComparison.Ordinal);
            Assert.Equal("destination", exception.ParamName);
        }

        [Fact]
        public void ReadUInt64_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[7];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.ReadUInt64(buffer, out _));
            Assert.Contains("Buffer too small for UInt64", exception.Message, StringComparison.Ordinal);
            Assert.Equal("source", exception.ParamName);
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

        [Fact]
        public void WriteSingle_ReadSingle_RoundTrip_NaN_And_Infinities()
        {
            var buf = new byte[4];

            // Test NaN
            Assert.Equal(4, BinarySerializer.WriteSingle(buf, float.NaN));
            BinarySerializer.ReadSingle(buf, out var fNaN);
            Assert.True(float.IsNaN(fNaN));

            // Test Positive Infinity
            Assert.Equal(4, BinarySerializer.WriteSingle(buf, float.PositiveInfinity));
            BinarySerializer.ReadSingle(buf, out var fPosInf);
            Assert.True(float.IsPositiveInfinity(fPosInf));

            // Test Negative Infinity
            Assert.Equal(4, BinarySerializer.WriteSingle(buf, float.NegativeInfinity));
            BinarySerializer.ReadSingle(buf, out var fNegInf);
            Assert.True(float.IsNegativeInfinity(fNegInf));
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

        [Fact]
        public void WriteDouble_ReadDouble_RoundTrip_NaN_And_Infinities()
        {
            var buf = new byte[8];

            // Test NaN
            Assert.Equal(8, BinarySerializer.WriteDouble(buf, double.NaN));
            BinarySerializer.ReadDouble(buf, out var dNaN);
            Assert.True(double.IsNaN(dNaN));

            // Test Positive Infinity
            Assert.Equal(8, BinarySerializer.WriteDouble(buf, double.PositiveInfinity));
            BinarySerializer.ReadDouble(buf, out var dPosInf);
            Assert.True(double.IsPositiveInfinity(dPosInf));

            // Test Negative Infinity
            Assert.Equal(8, BinarySerializer.WriteDouble(buf, double.NegativeInfinity));
            BinarySerializer.ReadDouble(buf, out var dNegInf);
            Assert.True(double.IsNegativeInfinity(dNegInf));
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

        [Fact]
        public void WriteGuidRfc4122_ReadGuidRfc4122_RoundTrip()
        {
            // Arrange
            var testGuid = Guid.NewGuid();
            var buffer = new byte[16];

            // Act - Write RFC 4122
            var bytesWritten = BinarySerializer.WriteGuidRfc4122(buffer, testGuid);
            Assert.Equal(16, bytesWritten);

            // Act - Read RFC 4122
            var bytesRead = BinarySerializer.ReadGuidRfc4122(buffer, out var readGuid);

            // Assert
            Assert.Equal(16, bytesRead);
            Assert.Equal(testGuid, readGuid);
        }

        [Fact]
        public void WriteGuidRfc4122_EmptyGuid_WritesCorrectBytes()
        {
            // Arrange
            var emptyGuid = Guid.Empty;
            var buffer = new byte[16];

            // Act
            var bytesWritten = BinarySerializer.WriteGuidRfc4122(buffer, emptyGuid);

            // Assert
            Assert.Equal(16, bytesWritten);
            // Empty GUID should write 16 zero bytes in RFC 4122 order
            for (int i = 0; i < 16; i++)
            {
                Assert.Equal(0, buffer[i]);
            }
        }

        [Fact]
        public void WriteGuidRfc4122_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[15];
            var testGuid = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.WriteGuidRfc4122(buffer, testGuid));
            Assert.Contains("Buffer too small for Guid", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void ReadGuidRfc4122_BufferTooSmall_ThrowsArgumentException()
        {
            // Arrange
            var buffer = new byte[15];

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => BinarySerializer.ReadGuidRfc4122(buffer, out _));
            Assert.Contains("Buffer too small for Guid", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void Guid_StandardVsRfc4122_DifferentByteOrder()
        {
            // Arrange
            var testGuid = Guid.NewGuid();
            var standardBuffer = new byte[16];
            var rfc4122Buffer = new byte[16];

            // Act - Write using both methods
            BinarySerializer.WriteGuid(standardBuffer, testGuid);
            BinarySerializer.WriteGuidRfc4122(rfc4122Buffer, testGuid);

            // Assert - The byte orders should be different for the first 8 bytes
            // (Data1, Data2, Data3 are reversed in RFC 4122)
            Assert.NotEqual(standardBuffer[0], rfc4122Buffer[0]);
            Assert.NotEqual(standardBuffer[1], rfc4122Buffer[1]);
            Assert.NotEqual(standardBuffer[2], rfc4122Buffer[2]);
            Assert.NotEqual(standardBuffer[3], rfc4122Buffer[3]);
            Assert.NotEqual(standardBuffer[4], rfc4122Buffer[4]);
            Assert.NotEqual(standardBuffer[5], rfc4122Buffer[5]);
            Assert.NotEqual(standardBuffer[6], rfc4122Buffer[6]);
            Assert.NotEqual(standardBuffer[7], rfc4122Buffer[7]);
            // Last 8 bytes should be the same
            for (int i = 8; i < 16; i++)
            {
                Assert.Equal(standardBuffer[i], rfc4122Buffer[i]);
            }
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
