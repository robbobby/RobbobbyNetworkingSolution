namespace Serializer.Abstractions
{
    /// <summary>
    /// Base interface for binary serializers
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes an object to bytes
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>Serialized bytes</returns>
        byte[] Serialize(object obj);

        /// <summary>
        /// Deserializes bytes to an object
        /// </summary>
        /// <param name="data">The bytes to deserialize</param>
        /// <returns>Deserialized object</returns>
        object Deserialize(byte[] data);
    }
}
