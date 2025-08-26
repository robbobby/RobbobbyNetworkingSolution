using System;
using Sample.Packets.Packets;

namespace Sample.Packets
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🚀 RNS Source Generator Demo - Key-Value Serialization");
            Console.WriteLine("=====================================================");

            // Test PlayerMovePacket with some default values
            Console.WriteLine("\n📦 Testing PlayerMovePacket:");
            var movePacket = new PlayerMovePacket
            {
                Id = 12345,
                PlayerName = "TestPlayer",
                X = 100.5f,
                Y = 200.7f,
                Z = 0.0f, // Default value
                IsRunning = false, // Default value
                Timestamp = 1234567890
            };

            Console.WriteLine($"  Packet ID: {PlayerMovePacket.RnsKeys.PacketTypeId}");
            Console.WriteLine($"  Packet Name: {PlayerMovePacket.RnsKeys.PacketName}");
            Console.WriteLine($"  Packet Version: {PlayerMovePacket.RnsKeys.PacketVersion}");
            Console.WriteLine($"  Packet Type: {PlayerMovePacket.RnsKeys.PacketType}");

            var moveSize = movePacket.GetSerializedSize();
            Console.WriteLine($"  Serialized Size: {moveSize} bytes");
            Console.WriteLine($"  Properties: Id={movePacket.Id}, Name='{movePacket.PlayerName}', X={movePacket.X}, Y={movePacket.Y}, Z={movePacket.Z}, Running={movePacket.IsRunning}, Time={movePacket.Timestamp}");

            // Test serialization
            var moveBuffer = new byte[moveSize];
            var moveBytesWritten = movePacket.Write(moveBuffer);
            Console.WriteLine($"  Bytes Written: {moveBytesWritten}");
            Console.WriteLine($"  Buffer (hex): {BitConverter.ToString(moveBuffer, 0, moveBytesWritten)}");

            // Test ChatMessagePacket
            Console.WriteLine("\n💬 Testing ChatMessagePacket:");
            var chatPacket = new ChatMessagePacket
            {
                Id = 987654321,
                SenderName = "ChatUser",
                Message = "Hello, World!",
                Channel = "", // Default value (empty string)
                MessageType = 0,
                Timestamp = 1234567890
            };

            Console.WriteLine($"  Packet ID: {ChatMessagePacket.RnsKeys.PacketTypeId}");
            Console.WriteLine($"  Packet Name: {ChatMessagePacket.RnsKeys.PacketName}");
            Console.WriteLine($"  Packet Version: {ChatMessagePacket.RnsKeys.PacketVersion}");
            Console.WriteLine($"  Packet Type: {ChatMessagePacket.RnsKeys.PacketType}");

            var chatSize = chatPacket.GetSerializedSize();
            Console.WriteLine($"  Serialized Size: {chatSize} bytes");
            Console.WriteLine($"  Properties: Id={chatPacket.Id}, Sender='{chatPacket.SenderName}', Message='{chatPacket.Message}', Channel='{chatPacket.Channel}', Type={chatPacket.MessageType}, Time={chatPacket.Timestamp}");

            // Test serialization
            var chatBuffer = new byte[chatSize];
            var chatBytesWritten = chatPacket.Write(chatBuffer);
            Console.WriteLine($"  Bytes Written: {chatBytesWritten}");
            Console.WriteLine($"  Buffer (hex): {BitConverter.ToString(chatBuffer, 0, chatBytesWritten)}");

            // Test InventoryUpdatePacket
            Console.WriteLine("\n🎒 Testing InventoryUpdatePacket:");
            var inventoryPacket = new InventoryUpdatePacket
            {
                Id = "inv_001",
                PlayerId = "player_123",
                ItemId = 456,
                Quantity = 0, // Default value
                IsAdd = true,
                ItemWeight = 0.0f, // Default value
                SlotIndex = 3
            };

            Console.WriteLine($"  Packet ID: {InventoryUpdatePacket.RnsKeys.PacketTypeId}");
            Console.WriteLine($"  Packet Name: {InventoryUpdatePacket.RnsKeys.PacketName}");
            Console.WriteLine($"  Packet Version: {InventoryUpdatePacket.RnsKeys.PacketVersion}");
            Console.WriteLine($"  Packet Type: {InventoryUpdatePacket.RnsKeys.PacketType}");

            var inventorySize = inventoryPacket.GetSerializedSize();
            Console.WriteLine($"  Serialized Size: {inventorySize} bytes");
            Console.WriteLine($"  Properties: Id='{inventoryPacket.Id}', Player='{inventoryPacket.PlayerId}', Item={inventoryPacket.ItemId}, Qty={inventoryPacket.Quantity}, Add={inventoryPacket.IsAdd}, Weight={inventoryPacket.ItemWeight}, Slot={inventoryPacket.SlotIndex}");

            // Test serialization
            var inventoryBuffer = new byte[inventorySize];
            var inventoryBytesWritten = inventoryPacket.Write(inventoryBuffer);
            Console.WriteLine($"  Bytes Written: {inventoryBytesWritten}");
            Console.WriteLine($"  Buffer (hex): {BitConverter.ToString(inventoryBuffer, 0, inventoryBytesWritten)}");

            Console.WriteLine("\n✅ Source Generator Demo Complete!");
            Console.WriteLine("\n📝 Key-Value Serialization Features:");
            Console.WriteLine("  • Each property gets a 1-byte key (index)");
            Console.WriteLine("  • Only non-default values are written");
            Console.WriteLine("  • Default values (0, false, empty string) are skipped");
            Console.WriteLine("  • Efficient for sparse data structures");
        }
    }
}
