using Serializer.Abstractions;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Test packet for testing the source generator
    /// </summary>
    [RnsSerializable]
    public class TestPacket : IRnsPacket<int>
    {
        /// <summary>
        /// Gets or sets the packet identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player name
        /// </summary>
        public string PlayerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the X coordinate
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the player health
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets whether the player is alive
        /// </summary>
        public bool IsAlive { get; set; }
    }
}
