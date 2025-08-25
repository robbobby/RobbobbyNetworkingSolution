using System;

namespace Serializer.Abstractions
{
    /// <summary>
    /// Interface for types that can write themselves to a binary buffer.
    /// </summary>
    public interface IBinaryWritable
    {
        /// <summary>
        /// Writes the current instance to the specified buffer.
        /// </summary>
        /// <param name="destination">The destination buffer to write to.</param>
        /// <returns>The number of bytes written.</returns>
        int Write(Span<byte> destination);

        /// <summary>
        /// Gets the total number of bytes required to serialize this instance.
        /// </summary>
        /// <returns>The serialized size in bytes.</returns>
        int GetSerializedSize();
    }
}
