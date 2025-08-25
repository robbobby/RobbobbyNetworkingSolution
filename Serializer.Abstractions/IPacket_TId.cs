namespace Serializer.Abstractions
{
    /// <summary>
    /// Generic interface for network packets with a strongly-typed identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the packet identifier.</typeparam>
    public interface IPacket<TId> : IPacket
    {
        /// <summary>
        /// Gets the unique identifier for this packet type.
        /// </summary>
        TId Id { get; }
    }
}
