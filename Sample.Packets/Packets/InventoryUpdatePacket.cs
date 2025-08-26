using Serializer.Abstractions;

namespace Sample.Packets.Packets
{
    /// <summary>
    /// Sample packet for inventory updates
    /// </summary>
    [RnsSerializable]
    public partial class InventoryUpdatePacket : IRnsPacket<string>
    {
        public string Id { get; set; } = string.Empty;
        public string PlayerId { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public bool IsAdd { get; set; } // true = add item, false = remove item
        public float ItemWeight { get; set; }
        public int SlotIndex { get; set; }
    }
}
