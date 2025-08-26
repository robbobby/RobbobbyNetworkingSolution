namespace Serializer.Abstractions
{
    /// <summary>
    /// Generic interface for network packets with a strongly-typed identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the packet identifier.</typeparam>
    public interface IRnsPacket<TId> : IRnsPacket where TId : notnull
    {
        /// <summary>
        /// Gets the protocol identifier for this packet. Implementations may return a
        /// type-level constant (e.g., an opcode) or an instance-specific value, depending
        /// on the protocol's design.
        /// </summary>
        TId Id { get; }
    }
}
