using System;

namespace Serializer.Abstractions
{
    /// <summary>
    /// Interface for types that can write themselves to a binary buffer.
    /// </summary>
    public interface IRnsBinaryWritable
    {
            /// <summary>
    /// Writes the current instance to the specified buffer.
    /// </summary>
    /// <param name="destination">
    /// The destination buffer to write to. Implementations must write starting at index 0 and
    /// must not write beyond <paramref name="destination"/>.Length. When the buffer is too small,
    /// they should throw <see cref="ArgumentException"/> with <paramref name="destination"/> as the
    /// parameter name.
    /// </param>
    /// <returns>The number of bytes written. Implementations should return exactly the value of
    /// <see cref="GetSerializedSize"/>.</returns>
    int Write(Span<byte> destination);

    /// <summary>
    /// Gets the total number of bytes required to serialize this instance. This value should be
    /// constant with respect to the current state so callers can pre-size buffers without trial writes.
    /// </summary>
    /// <returns>The serialized size in bytes.</returns>
    int GetSerializedSize();
    }
}
