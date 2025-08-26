using System;
using System.Linq;
using Xunit;
using Test.RnsPackets;

namespace Serializer.Runtime.Tests
{
    /// <summary>
    /// Tests for RNS Packet Binary Spec (v1) implementation
    /// </summary>
    public class RnsPacketTests
    {
        [Fact]
        public void TestPacket_Write_EmptyPacket_OmitsDefaults()
        {
            // Arrange
            var packet = new TestPacket();
            var buffer = new byte[1024];

            // Act
            var result = packet.Write(buffer, out var bytesWritten);

            // Assert
            Assert.True(result);
            Assert.Equal(0, bytesWritten); // All default values should be omitted
        }

        [Fact]
        public void TestPacket_Write_WithValues_SerializesCorrectly()
        {
            // Arrange
            var packet = new TestPacket
            {
                PlayerName = "Player1",
                X = 10.5f,
                Y = 20.7f,
                Health = 100,
                IsAlive = true
            };
            var buffer = new byte[1024];

            // Act
            var result = packet.Write(buffer, out var bytesWritten);

            // Assert
            Assert.True(result);
            Assert.True(bytesWritten > 0);
            
            // The buffer should contain encoded data
            Assert.NotEqual(0, buffer[0]); // Should have some data
        }

        [Fact]
        public void TestPacket_WriteRead_RoundTrip_PreservesData()
        {
            // Arrange
            var original = new TestPacket
            {
                PlayerName = "TestPlayer",
                X = 42.5f,
                Y = -17.3f,
                Health = -50,
                IsAlive = true
            };
            var buffer = new byte[1024];

            // Act - Write
            var writeResult = original.Write(buffer, out var bytesWritten);
            Assert.True(writeResult);

            // Act - Read
            var readResult = TestPacket.TryRead(buffer.AsSpan(0, bytesWritten), out var deserialized, out var bytesRead);

            // Assert
            Assert.True(readResult);
            Assert.Equal(bytesWritten, bytesRead);
            Assert.Equal(original.PlayerName, deserialized.PlayerName);
            Assert.Equal(original.X, deserialized.X);
            Assert.Equal(original.Y, deserialized.Y);
            Assert.Equal(original.Health, deserialized.Health);
            Assert.Equal(original.IsAlive, deserialized.IsAlive);
        }

        [Fact]
        public void TestPacket_WithNestedObject_SerializesCorrectly()
        {
            // Arrange
            var packet = new TestPacket
            {
                PlayerName = "Player1",
                FieldPacket = new TestFieldPacket
                {
                    PlayerName = "NestedPlayer",
                    X = 5.0f,
                    Health = 75
                }
            };
            var buffer = new byte[1024];

            // Act - Write
            var writeResult = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeResult);

            // Act - Read
            var readResult = TestPacket.TryRead(buffer.AsSpan(0, bytesWritten), out var deserialized, out var bytesRead);

            // Assert
            Assert.True(readResult);
            Assert.Equal(bytesWritten, bytesRead);
            Assert.Equal(packet.PlayerName, deserialized.PlayerName);
            Assert.NotNull(deserialized.FieldPacket);
            Assert.Equal(packet.FieldPacket.PlayerName, deserialized.FieldPacket.PlayerName);
            Assert.Equal(packet.FieldPacket.X, deserialized.FieldPacket.X);
            Assert.Equal(packet.FieldPacket.Health, deserialized.FieldPacket.Health);
        }

        [Fact]
        public void TestPacket_WithArray_SerializesCorrectly()
        {
            // Arrange
            var packet = new TestPacket
            {
                PlayerName = "Player1",
                FieldPacketList = new[]
                {
                    new TestFieldPacket { PlayerName = "Item1", X = 1.0f },
                    new TestFieldPacket { PlayerName = "Item2", Y = 2.0f, Health = 50 }
                }
            };
            var buffer = new byte[1024];

            // Act - Write
            var writeResult = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeResult);

            // Act - Read
            var readResult = TestPacket.TryRead(buffer.AsSpan(0, bytesWritten), out var deserialized, out var bytesRead);

            // Assert
            Assert.True(readResult);
            Assert.Equal(bytesWritten, bytesRead);
            Assert.Equal(packet.PlayerName, deserialized.PlayerName);
            Assert.NotNull(deserialized.FieldPacketList);
            Assert.Equal(2, deserialized.FieldPacketList.Length);
            
            Assert.Equal("Item1", deserialized.FieldPacketList[0].PlayerName);
            Assert.Equal(1.0f, deserialized.FieldPacketList[0].X);
            
            Assert.Equal("Item2", deserialized.FieldPacketList[1].PlayerName);
            Assert.Equal(2.0f, deserialized.FieldPacketList[1].Y);
            Assert.Equal(50, deserialized.FieldPacketList[1].Health);
        }

        [Fact]
        public void TestPacket_WithDeeplyNestedObjects_SerializesCorrectly()
        {
            // Arrange
            var packet = new TestPacket
            {
                PlayerName = "Root",
                FieldPacket = new TestFieldPacket
                {
                    PlayerName = "Level1",
                    FieldPacket2 = new TestFieldPacket2
                    {
                        PlayerName = "Level2",
                        Health = 42
                    }
                }
            };
            var buffer = new byte[1024];

            // Act - Write
            var writeResult = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeResult);

            // Act - Read
            var readResult = TestPacket.TryRead(buffer.AsSpan(0, bytesWritten), out var deserialized, out var bytesRead);

            // Assert
            Assert.True(readResult);
            Assert.Equal(bytesWritten, bytesRead);
            Assert.Equal("Root", deserialized.PlayerName);
            Assert.NotNull(deserialized.FieldPacket);
            Assert.Equal("Level1", deserialized.FieldPacket.PlayerName);
            Assert.NotNull(deserialized.FieldPacket.FieldPacket2);
            Assert.Equal("Level2", deserialized.FieldPacket.FieldPacket2.PlayerName);
            Assert.Equal(42, deserialized.FieldPacket.FieldPacket2.Health);
        }

        [Fact]
        public void TestPacket_BufferTooSmall_ReturnsFalse()
        {
            // Arrange
            var packet = new TestPacket
            {
                PlayerName = "VeryLongPlayerNameThatWillNotFitInSmallBuffer"
            };
            var buffer = new byte[5]; // Too small

            // Act
            var result = packet.Write(buffer, out var bytesWritten);

            // Assert
            Assert.False(result);
            Assert.Equal(0, bytesWritten); // No partial writes
        }

        [Fact]
        public void TestPacket_ReadMalformedData_ReturnsFalse()
        {
            // Arrange
            var buffer = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }; // Invalid VarUInt

            // Act
            var result = TestPacket.TryRead(buffer, out var packet, out var bytesRead);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TestPacket_ReadWithUnknownFields_SkipsGracefully()
        {
            // Arrange
            var originalPacket = new TestPacket { PlayerName = "Test", Health = 100 };
            var buffer = new byte[1024];
            
            // Write original packet
            originalPacket.Write(buffer, out var written);
            
            // Manually add an unknown field (field 99, wire type 0, value 42)
            var bufferSpan = buffer.AsSpan();
            var tag = (99 << 3) | 0; // Field 99, wire type 0
            
            // Insert unknown field at the beginning by shifting existing data
            var existingData = bufferSpan.Slice(0, written).ToArray();
            var newBuffer = new byte[1024];
            var pos = 0;
            
            // Write unknown field
            newBuffer[pos++] = (byte)tag; // Tag for field 99
            newBuffer[pos++] = 42; // Value
            
            // Copy existing data after unknown field
            existingData.CopyTo(newBuffer.AsSpan(pos));
            var totalBytes = pos + written;

            // Act
            var result = TestPacket.TryRead(newBuffer.AsSpan(0, totalBytes), out var deserialized, out var bytesRead);

            // Assert
            Assert.True(result);
            Assert.Equal(originalPacket.PlayerName, deserialized.PlayerName);
            Assert.Equal(originalPacket.Health, deserialized.Health);
        }
    }
}