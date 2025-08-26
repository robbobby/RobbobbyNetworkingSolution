using System;
using Xunit;
using Serializer.Abstractions;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Tests for the SerializerGenerator source generator
    /// </summary>
    [Trait("Category", "Unit")]
    public class SerializerGeneratorTests
    {
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
        public void TestPacket_HasIdProperty_ShouldBeAccessible()
        {
            // Arrange
            var testPacket = new TestPacket();
            var expectedId = 123;

            // Act
            testPacket.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, testPacket.Id);
        }

        [Fact]
        public void TestPacket_HasPlayerNameProperty_ShouldBeAccessible()
        {
            // Arrange
            var testPacket = new TestPacket();
            var expectedName = "TestPlayer";

            // Act
            testPacket.PlayerName = expectedName;

            // Assert
            Assert.Equal(expectedName, testPacket.PlayerName);
        }

        [Fact]
        public void TestPacket_HasPositionProperties_ShouldBeAccessible()
        {
            // Arrange
            var testPacket = new TestPacket();
            var expectedX = 10.5f;
            var expectedY = 20.7f;

            // Act
            testPacket.X = expectedX;
            testPacket.Y = expectedY;

            // Assert
            Assert.Equal(expectedX, testPacket.X);
            Assert.Equal(expectedY, testPacket.Y);
        }

        [Fact]
        public void TestPacket_HasHealthProperty_ShouldBeAccessible()
        {
            // Arrange
            var testPacket = new TestPacket();
            var expectedHealth = 100;

            // Act
            testPacket.Health = expectedHealth;

            // Assert
            Assert.Equal(expectedHealth, testPacket.Health);
        }

        [Fact]
        public void TestPacket_HasIsAliveProperty_ShouldBeAccessible()
        {
            // Arrange
            var testPacket = new TestPacket();
            var expectedIsAlive = true;

            // Act
            testPacket.IsAlive = expectedIsAlive;

            // Assert
            Assert.Equal(expectedIsAlive, testPacket.IsAlive);
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
    }
}
