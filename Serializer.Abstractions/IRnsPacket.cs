using System.Diagnostics.CodeAnalysis;

namespace Serializer.Abstractions
{
    /// <summary>
    /// Marker interface that identifies a type as a network packet.
    /// All packet types should implement this interface.
    /// </summary>
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is intentionally a marker interface for packet identification")]
    public interface IRnsPacket
    {
        // Marker interface - no members required
    }
}
