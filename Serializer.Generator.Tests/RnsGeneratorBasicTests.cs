using Xunit;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Basic tests for the RNS serialization generator following the problem statement
    /// </summary>
    public class RnsGeneratorBasicTests
    {
        [Fact]
        public void TestPacket_HasGeneratedKeys()
        {
            // Test that generated Keys class exists and has expected values
            Assert.Equal(1, TestPacket.Keys.PlayerName);
            Assert.Equal(2, TestPacket.Keys.X);
            Assert.Equal(3, TestPacket.Keys.Y);
            Assert.Equal(4, TestPacket.Keys.Health);
            Assert.Equal(5, TestPacket.Keys.IsAlive);
            Assert.Equal(6, TestPacket.Keys.FieldPacket);
            Assert.Equal(7, TestPacket.Keys.FieldPacketList);
        }

        [Fact]
        public void TestFieldPacket_HasGeneratedKeys()
        {
            // Test nested packet keys
            Assert.Equal(1, TestFieldPacket.Keys.PlayerName);
            Assert.Equal(2, TestFieldPacket.Keys.X);
            Assert.Equal(3, TestFieldPacket.Keys.Y);
            Assert.Equal(4, TestFieldPacket.Keys.Health);
            Assert.Equal(5, TestFieldPacket.Keys.FieldPacket2);
        }

        [Fact]
        public void TestFieldPacket2_HasGeneratedKeys()
        {
            // Test deeply nested packet keys
            Assert.Equal(1, TestFieldPacket2.Keys.PlayerName);
            Assert.Equal(2, TestFieldPacket2.Keys.X);
            Assert.Equal(3, TestFieldPacket2.Keys.Y);
            Assert.Equal(4, TestFieldPacket2.Keys.Health);
        }

        [Fact]
        public void TestPacket_DefaultValues_SerializesToZeroBytes()
        {
            // Test that default values are omitted entirely
            var packet = new TestPacket();
            var buffer = new byte[1024];

            bool success = packet.Write(buffer, out var bytesWritten);

            Assert.True(success);
            Assert.Equal(0, bytesWritten);
        }

        [Fact]
        public void TestPacket_WithValues_RoundTrip()
        {
            // Test basic round-trip serialization
            var original = new TestPacket
            {
                PlayerName = "Alice",
                X = 1.5f,
                Health = 100,
                IsAlive = true
                // Y remains default (0f)
            };

            var writeBuffer = new byte[1024];
            bool writeSuccess = original.Write(writeBuffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify that we wrote some data
            Console.WriteLine($"Serialized {bytesWritten} bytes");

            // Deserialize
            int consumed = 0;
            bool readSuccess = TestPacket.TryRead(writeBuffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);

            // Verify values
            Assert.Equal(original.PlayerName, deserialized.PlayerName);
            Assert.Equal(original.X, deserialized.X);
            Assert.Equal(original.Y, deserialized.Y); // Should be default
            Assert.Equal(original.Health, deserialized.Health);
            Assert.Equal(original.IsAlive, deserialized.IsAlive);
        }

        [Fact]
        public void TestPacket_WithNestedObject_RoundTrip()
        {
            // Test nested object serialization
            var original = new TestPacket
            {
                PlayerName = "Bob",
                FieldPacket = new TestFieldPacket
                {
                    PlayerName = "NestedPlayer",
                    X = 2.0f,
                    Health = 50
                }
            };

            var writeBuffer = new byte[1024];
            bool writeSuccess = original.Write(writeBuffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            int consumed = 0;
            bool readSuccess = TestPacket.TryRead(writeBuffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);

            Assert.Equal(original.PlayerName, deserialized.PlayerName);
            Assert.NotNull(deserialized.FieldPacket);
            Assert.Equal(original.FieldPacket.PlayerName, deserialized.FieldPacket.PlayerName);
            Assert.Equal(original.FieldPacket.X, deserialized.FieldPacket.X);
            Assert.Equal(original.FieldPacket.Health, deserialized.FieldPacket.Health);
        }

        [Fact]
        public void TestPacket_WithArray_RoundTrip()
        {
            // Test array serialization
            var original = new TestPacket
            {
                PlayerName = "Charlie",
                FieldPacketList = new[]
                {
                    new TestFieldPacket { PlayerName = "Item1", Health = 10 },
                    new TestFieldPacket { PlayerName = "Item2", Health = 20 }
                }
            };

            var writeBuffer = new byte[1024];
            bool writeSuccess = original.Write(writeBuffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            int consumed = 0;
            bool readSuccess = TestPacket.TryRead(writeBuffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);

            Assert.Equal(original.PlayerName, deserialized.PlayerName);
            Assert.NotNull(deserialized.FieldPacketList);
            Assert.Equal(2, deserialized.FieldPacketList.Length);

            Assert.Equal("Item1", deserialized.FieldPacketList[0].PlayerName);
            Assert.Equal(10, deserialized.FieldPacketList[0].Health);

            Assert.Equal("Item2", deserialized.FieldPacketList[1].PlayerName);
            Assert.Equal(20, deserialized.FieldPacketList[1].Health);
        }
    }
}
