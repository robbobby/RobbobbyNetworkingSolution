using System;
using System.Text;
using Xunit;
using Serializer.Abstractions;
using System.Linq;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Comprehensive tests for the SerializerGenerator source generator
    /// </summary>
    [Trait("Category", "Unit")]
    public class SerializerGeneratorTests
    {
        #region Basic Functionality Tests

        [Fact]
        public void TestPacket_ImplementsIRnsPacket_ShouldBeTrue()
        {
            // Arrange & Act
            var testPacket = new TestPacket();

            // Assert
            Assert.True(testPacket is IRnsPacket<int>);
            Assert.True(testPacket is IRnsPacket);
        }

        [Fact]
        public void TestPacket_Properties_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var testPacket = new TestPacket();

            // Assert
            Assert.Equal(0, testPacket.Id);
            Assert.Equal(string.Empty, testPacket.PlayerName);
            Assert.Equal(0.0f, testPacket.X);
            Assert.Equal(0.0f, testPacket.Y);
            Assert.Equal(0, testPacket.Health);
            Assert.False(testPacket.IsAlive);
        }

        [Fact]
        public void TestPacket_CanSetAllProperties_ShouldWork()
        {
            // Arrange
            var testPacket = new TestPacket();
            var testData = new
            {
                Id = 456,
                PlayerName = "AllPropertiesTest",
                X = 15.3f,
                Y = 25.8f,
                Health = 75,
                IsAlive = false
            };

            // Act
            testPacket.Id = testData.Id;
            testPacket.PlayerName = testData.PlayerName;
            testPacket.X = testData.X;
            testPacket.Y = testData.Y;
            testPacket.Health = testData.Health;
            testPacket.IsAlive = testData.IsAlive;

            // Assert
            Assert.Equal(testData.Id, testPacket.Id);
            Assert.Equal(testData.PlayerName, testPacket.PlayerName);
            Assert.Equal(testData.X, testPacket.X);
            Assert.Equal(testData.Y, testPacket.Y);
            Assert.Equal(testData.Health, testPacket.Health);
            Assert.Equal(testData.IsAlive, testPacket.IsAlive);
        }

        #endregion

        #region RnsKeys Tests

        [Fact]
        public void TestPacket_RnsKeys_ShouldBeAccessible()
        {
            // Arrange & Act
            var keys = TestPacket.RnsKeys;

            // Assert
            Assert.NotNull(keys);
            Assert.IsAssignableFrom<ITestPacketKeys>(keys);
        }

        [Fact]
        public void TestPacket_RnsKeys_ShouldHaveCorrectPacketTypeId()
        {
            // Arrange & Act
            var keys = TestPacket.RnsKeys;

            // Assert
            Assert.True(keys.PacketTypeId >= 1000 && keys.PacketTypeId < 10000);
        }

        [Fact]
        public void TestPacket_RnsKeys_ShouldHaveCorrectPacketName()
        {
            // Arrange & Act
            var keys = TestPacket.RnsKeys;

            // Assert
            Assert.Equal("TestPacket", keys.PacketName);
        }

        [Fact]
        public void TestPacket_RnsKeys_ShouldHaveCorrectPacketVersion()
        {
            // Arrange & Act
            var keys = TestPacket.RnsKeys;

            // Assert
            Assert.Equal(1, keys.PacketVersion);
        }

        [Fact]
        public void TestPacket_RnsKeys_ShouldHaveCorrectPacketType()
        {
            // Arrange & Act
            var keys = TestPacket.RnsKeys;

            // Assert
            Assert.Equal(typeof(TestPacket), keys.PacketType);
        }

        [Fact]
        public void TestPacket_Keys_ShouldBeSameAsRnsKeys()
        {
            // Arrange & Act
            var keys1 = TestPacket.RnsKeys;
            var keys2 = TestPacket.Keys;

            // Assert
            Assert.Same(keys1, keys2);
        }

        #endregion

        #region Serialization Size Tests

        [Fact]
        public void TestPacket_GetSerializedSize_WithAllDefaultValues_ShouldReturnKeysOnly()
        {
            // Arrange
            var testPacket = new TestPacket(); // All properties are default values

            // Act
            var size = testPacket.GetSerializedSize();

            // Assert
            // 6 properties * 2 bytes (key + flag) + 1 terminator = 13 bytes
            Assert.Equal(13, size);
        }

        [Fact]
        public void TestPacket_GetSerializedSize_WithAllValuesSet_ShouldReturnFullSize()
        {
            // Arrange
            var testPacket = new TestPacket
            {
                Id = 123,
                PlayerName = "TestPlayer",
                X = 10.5f,
                Y = 20.7f,
                Health = 100,
                IsAlive = true
            };

            // Act
            var size = testPacket.GetSerializedSize();

            // Assert
            // 6 keys (1 byte each) + Id (4 bytes) + PlayerName (2 + 10 bytes) + X (4 bytes) + Y (4 bytes) + Health (4 bytes) + IsAlive (1 byte)
            // 6 properties * 2 bytes (key + flag) + Id (4 bytes) + PlayerName (2 + 10 bytes) + X (4 bytes) + Y (4 bytes) + Health (4 bytes) + IsAlive (1 byte) + 1 terminator = 32 bytes
            Assert.Equal(32, size);
        }

        [Fact]
        public void TestPacket_GetSerializedSize_WithMixedValues_ShouldReturnCorrectSize()
        {
            // Arrange
            var testPacket = new TestPacket
            {
                Id = 123,
                PlayerName = "", // Default value
                X = 10.5f,
                Y = 0.0f, // Default value
                Health = 0, // Default value
                IsAlive = true
            };

            // Act
            var size = testPacket.GetSerializedSize();

            // Assert
            // 6 properties * 2 bytes (key + flag) + Id (4 bytes) + X (4 bytes) + IsAlive (1 byte) + 1 terminator = 22 bytes
            Assert.Equal(22, size);
        }

        #endregion

        #region Serialization Tests

        [Fact]
        public void TestPacket_Write_WithAllDefaultValues_ShouldWriteKeysOnly()
        {
            // Arrange
            var testPacket = new TestPacket();
            var buffer = new byte[testPacket.GetSerializedSize()];

            // Act
            var bytesWritten = testPacket.Write(buffer);

            // Assert
            Assert.Equal(13, bytesWritten);
            // Verify only keys are written (0, 1, 2, 3, 4, 5) after 4-byte length prefix
            // Format: [4 bytes: length] [flag1][key1] [flag2][key2] [flag3][key3] [flag4][key4] [flag5][key5] [flag6][key6]
            Assert.Equal(0, buffer[0]); // key1
            Assert.Equal(1, buffer[2]); // key2
            Assert.Equal(2, buffer[4]); // key3
            Assert.Equal(3, buffer[6]); // key4
            Assert.Equal(4, buffer[8]); // key5
            Assert.Equal(5, buffer[10]); // key6
        }

        [Fact]
        public void TestPacket_Write_WithAllValuesSet_ShouldWriteKeysAndValues()
        {
            // Arrange
            var testPacket = new TestPacket
            {
                Id = 123,
                PlayerName = "TestPlayer",
                X = 10.5f,
                Y = 20.7f,
                Health = 100,
                IsAlive = true
            };
            var buffer = new byte[testPacket.GetSerializedSize()];

            // Act
            var bytesWritten = testPacket.Write(buffer);

            // Assert
            Assert.Equal(32, bytesWritten);

            // Verify keys are written
            Assert.Equal(0, buffer[0]); // Id key
            Assert.Equal(1, buffer[1]); // PlayerName key
            Assert.Equal(2, buffer[2]); // X key
            Assert.Equal(3, buffer[3]); // Y key
            Assert.Equal(4, buffer[4]); // Health key
            Assert.Equal(5, buffer[5]); // IsAlive key

            // Verify values are written (simplified check)
            Assert.True(bytesWritten > 13); // More than just keys + flags + terminator
        }

        [Fact]
        public void TestPacket_Write_WithMixedValues_ShouldWriteKeysAndNonDefaultValues()
        {
            // Arrange
            var testPacket = new TestPacket
            {
                Id = 123,
                PlayerName = "", // Default value
                X = 10.5f,
                Y = 0.0f, // Default value
                Health = 0, // Default value
                IsAlive = true
            };
            var buffer = new byte[testPacket.GetSerializedSize()];

            // Act
            var bytesWritten = testPacket.Write(buffer);

            // Assert
            Assert.Equal(22, bytesWritten);

            // Verify keys are written
            Assert.Equal(0, buffer[0]); // Id key
            Assert.Equal(1, buffer[1]); // PlayerName key (empty string)
            Assert.Equal(2, buffer[2]); // X key
            Assert.Equal(3, buffer[3]); // Y key (default)
            Assert.Equal(4, buffer[4]); // Health key (default)
            Assert.Equal(5, buffer[5]); // IsAlive key
        }

        #endregion

        #region Deserialization Tests

        [Fact]
        public void TestPacket_TryRead_WithAllDefaultValues_ShouldSucceed()
        {
            // Arrange
            var originalPacket = new TestPacket();
            var buffer = new byte[originalPacket.GetSerializedSize()];
            originalPacket.Write(buffer);

            // Act
            var success = TestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(13, bytesRead);
            Assert.Equal(0, readPacket.Id);
            Assert.Equal(string.Empty, readPacket.PlayerName);
            Assert.Equal(0.0f, readPacket.X);
            Assert.Equal(0.0f, readPacket.Y);
            Assert.Equal(0, readPacket.Health);
            Assert.False(readPacket.IsAlive);
        }

        [Fact]
        public void TestPacket_TryRead_WithAllValuesSet_ShouldSucceed()
        {
            // Arrange
            var originalPacket = new TestPacket
            {
                Id = 123,
                PlayerName = "TestPlayer",
                X = 10.5f,
                Y = 20.7f,
                Health = 100,
                IsAlive = true
            };
            var buffer = new byte[originalPacket.GetSerializedSize()];
            originalPacket.Write(buffer);

            // Act
            var success = TestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(32, bytesRead);
            Assert.Equal(123, readPacket.Id);
            Assert.Equal("TestPlayer", readPacket.PlayerName);
            Assert.Equal(10.5f, readPacket.X);
            Assert.Equal(20.7f, readPacket.Y);
            Assert.Equal(100, readPacket.Health);
            Assert.True(readPacket.IsAlive);
        }

        [Fact]
        public void TestPacket_TryRead_WithMixedValues_ShouldSucceed()
        {
            // Arrange
            var originalPacket = new TestPacket
            {
                Id = 123,
                PlayerName = "", // Default value
                X = 10.5f,
                Y = 0.0f, // Default value
                Health = 0, // Default value
                IsAlive = true
            };
            var buffer = new byte[originalPacket.GetSerializedSize()];
            originalPacket.Write(buffer);

            // Act
            var success = TestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(22, bytesRead);
            Assert.Equal(123, readPacket.Id);
            Assert.Equal("", readPacket.PlayerName);
            Assert.Equal(10.5f, readPacket.X);
            Assert.Equal(0.0f, readPacket.Y);
            Assert.Equal(0, readPacket.Health);
            Assert.True(readPacket.IsAlive);
        }

        #endregion

        #region Round-Trip Tests

        [Fact]
        public void TestPacket_WriteThenRead_ShouldPreserveAllValues()
        {
            // Arrange
            var originalPacket = new TestPacket
            {
                Id = 999,
                PlayerName = "RoundTripTest",
                X = 42.7f,
                Y = 73.1f,
                Health = 50,
                IsAlive = false
            };

            // Act
            var buffer = new byte[originalPacket.GetSerializedSize()];
            var bytesWritten = originalPacket.Write(buffer);
            var success = TestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(originalPacket.Id, readPacket.Id);
            Assert.Equal(originalPacket.PlayerName, readPacket.PlayerName);
            Assert.Equal(originalPacket.X, readPacket.X);
            Assert.Equal(originalPacket.Y, readPacket.Y);
            Assert.Equal(originalPacket.Health, readPacket.Health);
            Assert.Equal(originalPacket.IsAlive, readPacket.IsAlive);
        }

        [Fact]
        public void TestPacket_WriteThenRead_WithEdgeCaseValues_ShouldPreserveAllValues()
        {
            // Arrange
            var originalPacket = new TestPacket
            {
                Id = int.MaxValue,
                PlayerName = "EdgeCaseTest",
                X = float.MaxValue,
                Y = float.MinValue,
                Health = int.MinValue,
                IsAlive = true
            };

            // Act
            var buffer = new byte[originalPacket.GetSerializedSize()];
            var bytesWritten = originalPacket.Write(buffer);
            var success = TestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(originalPacket.Id, readPacket.Id);
            Assert.Equal(originalPacket.PlayerName, readPacket.PlayerName);
            Assert.Equal(originalPacket.X, readPacket.X);
            Assert.Equal(originalPacket.Y, readPacket.Y);
            Assert.Equal(originalPacket.Health, readPacket.Health);
            Assert.Equal(originalPacket.IsAlive, readPacket.IsAlive);
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public void TestPacket_TryRead_WithEmptyBuffer_ShouldFail()
        {
            // Arrange
            var emptyBuffer = Array.Empty<byte>();

            // Act
            var success = TestPacket.TryRead(emptyBuffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.False(success);
            Assert.Equal(0, bytesRead);
        }

        [Fact]
        public void TestPacket_TryRead_WithIncompleteBuffer_ShouldFail()
        {
            // Arrange
            var incompleteBuffer = new byte[] { 0, 1, 2 }; // Only keys, no values

            // Act
            var success = TestPacket.TryRead(incompleteBuffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.False(success);
        }

        #endregion

        #region Convenience Method Tests

        [Fact]
        public void TestPacket_ToBytes_ShouldWorkWithArray()
        {
            // Arrange
            var testPacket = new TestPacket
            {
                Id = 123,
                PlayerName = "TestPlayer",
                X = 10.5f,
                Y = 20.7f,
                Health = 100,
                IsAlive = true
            };
            var buffer = new byte[testPacket.GetSerializedSize()];

            // Act
            var bytesWritten = testPacket.ToBytes(buffer);

            // Assert
            Assert.Equal(32, bytesWritten);
            Assert.True(bytesWritten > 0);
        }

        #endregion

        #region Property Key Tests

        [Fact]
        public void TestPacket_PropertyKeys_ShouldBeSequential()
        {
            // Arrange
            var testPacket = new TestPacket();
            var buffer = new byte[testPacket.GetSerializedSize()];

            // Act
            testPacket.Write(buffer);

            // Assert
            // Verify keys are written in sequential order
            // Format: [key1][flag1] [key2][flag2] [key3][flag3] [key4][flag4] [key5][flag5] [key6][flag6] [terminator]
            for (int i = 0; i < 6; i++)
            {
                Assert.Equal(i, buffer[i * 2]); // Keys are written at even offsets
            }
        }

        #endregion
    }

    /// <summary>
    /// Tests for EnumTestPacket to verify enum serialization
    /// </summary>
    [Trait("Category", "Unit")]
    public class EnumTestPacketTests
    {
        [Fact]
        public void EnumTestPacket_ImplementsIRnsPacket_ShouldBeTrue()
        {
            // Arrange & Act
            var testPacket = new EnumTestPacket();

            // Assert
            Assert.True(testPacket is IRnsPacket<byte>);
            Assert.True(testPacket is IRnsPacket);
        }

        [Fact]
        public void EnumTestPacket_RnsKeys_ShouldBeAccessible()
        {
            // Arrange & Act
            var keys = EnumTestPacket.RnsKeys;

            // Assert
            Assert.NotNull(keys);
            Assert.IsAssignableFrom<IEnumTestPacketKeys>(keys);
        }

        [Fact]
        public void EnumTestPacket_WriteThenRead_WithEnumValues_ShouldPreserveValues()
        {
            // Arrange
            var originalPacket = new EnumTestPacket
            {
                Id = 123,
                Type = EnumTestPacket.PacketType.Chat,
                UserStatus = EnumTestPacket.Status.Online,
                Username = "EnumTestUser",
                Timestamp = 1234567890
            };

            // Act
            var buffer = new byte[originalPacket.GetSerializedSize()];
            var bytesWritten = originalPacket.Write(buffer);
            var success = EnumTestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(originalPacket.Id, readPacket.Id);
            Assert.Equal(originalPacket.Type, readPacket.Type);
            Assert.Equal(originalPacket.UserStatus, readPacket.UserStatus);
            Assert.Equal(originalPacket.Username, readPacket.Username);
            Assert.Equal(originalPacket.Timestamp, readPacket.Timestamp);
        }

        [Fact]
        public void EnumTestPacket_WriteThenRead_WithDefaultEnumValues_ShouldSkipSerialization()
        {
            // Arrange
            var originalPacket = new EnumTestPacket
            {
                Id = 123,
                Type = EnumTestPacket.PacketType.Unknown, // Default value
                UserStatus = EnumTestPacket.Status.Offline, // Default value
                Username = "EnumTestUser",
                Timestamp = 0 // Default value
            };

            // Act
            var buffer = new byte[originalPacket.GetSerializedSize()];
            var bytesWritten = originalPacket.Write(buffer);
            var success = EnumTestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(originalPacket.Id, readPacket.Id);
            Assert.Equal(originalPacket.Type, readPacket.Type);
            Assert.Equal(originalPacket.UserStatus, readPacket.UserStatus);
            Assert.Equal(originalPacket.Username, readPacket.Username);
            Assert.Equal(originalPacket.Timestamp, readPacket.Timestamp);
        }
    }

    /// <summary>
    /// Tests for PrimitiveTestPacket to verify all primitive type serialization
    /// </summary>
    [Trait("Category", "Unit")]
    public class PrimitiveTestPacketTests
    {
        [Fact]
        public void PrimitiveTestPacket_ImplementsIRnsPacket_ShouldBeTrue()
        {
            // Arrange & Act
            var testPacket = new PrimitiveTestPacket();

            // Assert
            Assert.True(testPacket is IRnsPacket<long>);
            Assert.True(testPacket is IRnsPacket);
        }

        [Fact]
        public void PrimitiveTestPacket_RnsKeys_ShouldBeAccessible()
        {
            // Arrange & Act
            var keys = PrimitiveTestPacket.RnsKeys;

            // Assert
            Assert.NotNull(keys);
            Assert.IsAssignableFrom<IPrimitiveTestPacketKeys>(keys);
        }

        [Fact]
        public void PrimitiveTestPacket_WriteThenRead_WithAllPrimitiveTypes_ShouldPreserveValues()
        {
            // Arrange
            var originalPacket = new PrimitiveTestPacket
            {
                Id = 987654321,
                ByteValue = 255,
                SByteValue = -128,
                ShortValue = -32768,
                UShortValue = 65535,
                IntValue = -2147483648,
                UIntValue = 4294967295,
                LongValue = -9223372036854775808,
                ULongValue = 18446744073709551615,
                FloatValue = 3.14159f,
                DoubleValue = 2.718281828459045,
                BoolValue = true,
                StringValue = "PrimitiveTestString"
            };

            // Act
            var buffer = new byte[originalPacket.GetSerializedSize()];
            var bytesWritten = originalPacket.Write(buffer);
            var success = PrimitiveTestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(originalPacket.Id, readPacket.Id);
            Assert.Equal(originalPacket.ByteValue, readPacket.ByteValue);
            Assert.Equal(originalPacket.SByteValue, readPacket.SByteValue);
            Assert.Equal(originalPacket.ShortValue, readPacket.ShortValue);
            Assert.Equal(originalPacket.UShortValue, readPacket.UShortValue);
            Assert.Equal(originalPacket.IntValue, readPacket.IntValue);
            Assert.Equal(originalPacket.UIntValue, readPacket.UIntValue);
            Assert.Equal(originalPacket.LongValue, readPacket.LongValue);
            Assert.Equal(originalPacket.ULongValue, readPacket.ULongValue);
            Assert.Equal(originalPacket.FloatValue, readPacket.FloatValue);
            Assert.Equal(originalPacket.DoubleValue, readPacket.DoubleValue);
            Assert.Equal(originalPacket.BoolValue, readPacket.BoolValue);
            Assert.Equal(originalPacket.StringValue, readPacket.StringValue);
        }

        [Fact]
        public void PrimitiveTestPacket_WriteThenRead_WithAllDefaultValues_ShouldSkipSerialization()
        {
            // Arrange
            var originalPacket = new PrimitiveTestPacket(); // All properties are default values

            // Act
            var buffer = new byte[originalPacket.GetSerializedSize()];
            var bytesWritten = originalPacket.Write(buffer);
            var success = PrimitiveTestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(bytesWritten, consumed);
            // Verify all properties are default values
            Assert.Equal(0, readPacket.Id);
            Assert.Equal(0, readPacket.ByteValue);
            Assert.Equal(0, readPacket.SByteValue);
            Assert.Equal(0, readPacket.ShortValue);
            Assert.Equal(0, readPacket.UShortValue);
            Assert.Equal(0, readPacket.IntValue);
            Assert.Equal((uint)0, readPacket.UIntValue);
            Assert.Equal(0, readPacket.LongValue);
            Assert.Equal((ulong)0, readPacket.ULongValue);
            Assert.Equal(0.0f, readPacket.FloatValue);
            Assert.Equal(0.0, readPacket.DoubleValue);
            Assert.False(readPacket.BoolValue);
            Assert.Equal(string.Empty, readPacket.StringValue);
        }
    }

    /// <summary>
    /// Tests for EdgeCaseTestPacket to verify edge case handling
    /// </summary>
    [Trait("Category", "Unit")]
    public class EdgeCaseTestPacketTests
    {
        [Fact]
        public void EdgeCaseTestPacket_ImplementsIRnsPacket_ShouldBeTrue()
        {
            // Arrange & Act
            var testPacket = new EdgeCaseTestPacket();

            // Assert
            Assert.True(testPacket is IRnsPacket<string>);
            Assert.True(testPacket is IRnsPacket);
        }

        [Fact]
        public void EdgeCaseTestPacket_RnsKeys_ShouldBeAccessible()
        {
            // Arrange & Act
            var keys = EdgeCaseTestPacket.RnsKeys;

            // Assert
            Assert.NotNull(keys);
            Assert.IsAssignableFrom<IEdgeCaseTestPacketKeys>(keys);
        }

        [Fact]
        public void EdgeCaseTestPacket_WriteThenRead_WithEdgeCaseValues_ShouldPreserveValues()
        {
            // Arrange
            var originalPacket = new EdgeCaseTestPacket
            {
                Id = "EdgeCaseTest",
                MaxInt = int.MaxValue,
                MinInt = int.MinValue,
                MaxFloat = float.MaxValue,
                MinFloat = float.MinValue,
                MaxDouble = double.MaxValue,
                MinDouble = double.MinValue,
                EmptyString = "",
                LongString = new string('A', 1000),
                TrueValue = true,
                FalseValue = false
            };

            // Act
            var buffer = new byte[originalPacket.GetSerializedSize()];
            var bytesWritten = originalPacket.Write(buffer);
            var success = EdgeCaseTestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(originalPacket.Id, readPacket.Id);
            Assert.Equal(originalPacket.MaxInt, readPacket.MaxInt);
            Assert.Equal(originalPacket.MinInt, readPacket.MinInt);
            Assert.Equal(originalPacket.MaxFloat, readPacket.MaxFloat);
            Assert.Equal(originalPacket.MinFloat, readPacket.MinFloat);
            Assert.Equal(originalPacket.MaxDouble, readPacket.MaxDouble);
            Assert.Equal(originalPacket.MinDouble, readPacket.MinDouble);
            Assert.Equal(originalPacket.EmptyString, readPacket.EmptyString);
            Assert.Equal(originalPacket.LongString, readPacket.LongString);
            Assert.Equal(originalPacket.TrueValue, readPacket.TrueValue);
            Assert.Equal(originalPacket.FalseValue, readPacket.FalseValue);
        }

        [Fact]
        public void EdgeCaseTestPacket_GetSerializedSize_WithLongString_ShouldCalculateCorrectSize()
        {
            // Arrange
            var testPacket = new EdgeCaseTestPacket
            {
                Id = "Test",
                LongString = new string('X', 500)
            };

            // Act
            var size = testPacket.GetSerializedSize();

            // Assert
            // 11 keys (1 byte each) + Id (2 + 4 bytes) + LongString (2 + 500 bytes) + other non-default values
            Assert.True(size > 500); // Should be at least the string length plus overhead
        }
    }

    /// <summary>
    /// Tests for NullableTestPacket to verify nullable type handling
    /// </summary>
    [Trait("Category", "Unit")]
    public class NullableTestPacketTests
    {
        [Fact]
        public void NullableTestPacket_ImplementsIRnsPacket_ShouldBeTrue()
        {
            // Arrange & Act
            var testPacket = new NullableTestPacket();

            // Assert
            Assert.True(testPacket is IRnsPacket<int>);
            Assert.True(testPacket is IRnsPacket);
        }

        [Fact]
        public void NullableTestPacket_RnsKeys_ShouldBeAccessible()
        {
            // Arrange & Act
            var keys = NullableTestPacket.RnsKeys;

            // Assert
            Assert.NotNull(keys);
            Assert.IsAssignableFrom<INullableTestPacketKeys>(keys);
        }

        [Fact]
        public void NullableTestPacket_WriteThenRead_WithNullValues_ShouldPreserveValues()
        {
            // Arrange
            var originalPacket = new NullableTestPacket
            {
                Id = 123,
                NullableString = null,
                NonNullableString = "NonNullable",
                NullableInt = null,
                NonNullableInt = 456
            };

            // Act
            var buffer = new byte[originalPacket.GetSerializedSize()];
            var bytesWritten = originalPacket.Write(buffer);
            var success = NullableTestPacket.TryRead(buffer, out var readPacket, out var bytesRead);

            // Assert
            Assert.True(success);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(originalPacket.Id, readPacket.Id);
            Assert.Equal(originalPacket.NullableString, readPacket.NullableString);
            Assert.Equal(originalPacket.NonNullableString, readPacket.NonNullableString);
            Assert.Equal(originalPacket.NullableInt, readPacket.NullableInt);
            Assert.Equal(originalPacket.NonNullableInt, readPacket.NonNullableInt);
        }
    }

    /// <summary>
    /// Integration tests for multiple packet types
    /// </summary>
    [Trait("Category", "Integration")]
    public class MultiPacketIntegrationTests
    {
        [Fact]
        public void MultiplePacketTypes_ShouldGenerateUniquePacketTypeIds()
        {
            // Arrange & Act
            var testPacketId = TestPacket.RnsKeys.PacketTypeId;
            var enumPacketId = EnumTestPacket.RnsKeys.PacketTypeId;
            var primitivePacketId = PrimitiveTestPacket.RnsKeys.PacketTypeId;
            var edgeCasePacketId = EdgeCaseTestPacket.RnsKeys.PacketTypeId;
            var nullablePacketId = NullableTestPacket.RnsKeys.PacketTypeId;

            // Assert
            var allIds = new[] { testPacketId, enumPacketId, primitivePacketId, edgeCasePacketId, nullablePacketId };
            var uniqueIds = allIds.Distinct();
            Assert.Equal(allIds.Length, uniqueIds.Count());
        }

        [Fact]
        public void MultiplePacketTypes_ShouldHaveCorrectPacketNames()
        {
            // Arrange & Act
            var testPacketName = TestPacket.RnsKeys.PacketName;
            var enumPacketName = EnumTestPacket.RnsKeys.PacketName;
            var primitivePacketName = PrimitiveTestPacket.RnsKeys.PacketName;
            var edgeCasePacketName = EdgeCaseTestPacket.RnsKeys.PacketName;
            var nullablePacketName = NullableTestPacket.RnsKeys.PacketName;

            // Assert
            Assert.Equal("TestPacket", testPacketName);
            Assert.Equal("EnumTestPacket", enumPacketName);
            Assert.Equal("PrimitiveTestPacket", primitivePacketName);
            Assert.Equal("EdgeCaseTestPacket", edgeCasePacketName);
            Assert.Equal("NullableTestPacket", nullablePacketName);
        }

        [Fact]
        public void MultiplePacketTypes_ShouldHaveCorrectPacketVersions()
        {
            // Arrange & Act
            var testPacketVersion = TestPacket.RnsKeys.PacketVersion;
            var enumPacketVersion = EnumTestPacket.RnsKeys.PacketVersion;
            var primitivePacketVersion = PrimitiveTestPacket.RnsKeys.PacketVersion;
            var edgeCasePacketVersion = EdgeCaseTestPacket.RnsKeys.PacketVersion;
            var nullablePacketVersion = NullableTestPacket.RnsKeys.PacketVersion;

            // Assert
            Assert.Equal(1, testPacketVersion);
            Assert.Equal(1, enumPacketVersion);
            Assert.Equal(1, primitivePacketVersion);
            Assert.Equal(1, edgeCasePacketVersion);
            Assert.Equal(1, nullablePacketVersion);
        }
    }
}
