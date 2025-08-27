using System;

namespace Serializer.Generator
{
    /// <summary>
    /// Generic interface for network packets with a strongly-typed identifier.
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces",
        Justification = "This is intentionally a marker interface for packet field identification")]
    public interface IRnsPacketField
    {
        bool Write(byte[] buffer, out int bytesWritten);
        // static abstract bool TryRead<T>(ReadOnlySpan<byte> buffer, ref int consumed, out T readPacket);
    }
}
