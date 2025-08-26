using System;

namespace Serializer.Abstractions
{
    /// <summary>
    /// Generic interface for network packets with a strongly-typed identifier following RNS Binary Spec v1.
    /// </summary>
    /// <typeparam name="TId">The type of the packet identifier.</typeparam>
    public interface IRnsPacket<TId> : IRnsPacketField where TId : notnull
    {
        /// <summary>
        /// Gets the protocol identifier for this packet. Implementations may return a
        /// type-level constant (e.g., an opcode) or an instance-specific value, depending
        /// on the protocol's design.
        /// </summary>
        TId Id { get; }
    }

    /// <summary>
    /// Generic interface for network serializable fields within a packet.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marker interface for generated code")]
    public interface IRnsPacketField : IRnsPacket 
    {
        /// <summary>
        /// Writes the packet field's KV payload to the provided buffer.
        /// Returns false if buffer is too small; no partial writes are allowed.
        /// Omits default/empty/null fields entirely.
        /// </summary>
        /// <param name="buffer">Destination buffer for the KV payload</param>
        /// <param name="bytesWritten">Number of bytes written on success</param>
        /// <returns>True if successful, false if buffer too small</returns>
        bool Write(Span<byte> buffer, out int bytesWritten);
    }
}
