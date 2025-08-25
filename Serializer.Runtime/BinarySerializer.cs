using System;
using Serializer.Abstractions;

namespace Serializer.Runtime
{
    /// <summary>
    /// Binary serializer implementation
    /// </summary>
    public class BinarySerializer : ISerializer
    {
        /// <summary>
        /// Serializes an object to bytes
        /// </summary>
        public byte[] Serialize(object obj)
        {
            // TODO: Implement actual serialization logic
            return Array.Empty<byte>();
        }

        /// <summary>
        /// Deserializes bytes to an object
        /// </summary>
        public object Deserialize(byte[] data)
        {
            // TODO: Implement actual deserialization logic
            return new object();
        }
    }
}
