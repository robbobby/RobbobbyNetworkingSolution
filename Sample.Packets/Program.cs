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

            Console.WriteLine($"  PlayerName Key: {PlayerMovePacket.Keys.PlayerName}");
            Console.WriteLine($"  X Key: {PlayerMovePacket.Keys.X}");
            Console.WriteLine($"  Y Key: {PlayerMovePacket.Keys.Y}");
            Console.WriteLine($"  Z Key: {PlayerMovePacket.Keys.Z}");

            Console.WriteLine($"  Properties: Id={movePacket.Id}, Name='{movePacket.PlayerName}', X={movePacket.X}, Y={movePacket.Y}, Z={movePacket.Z}, Running={movePacket.IsRunning}, Time={movePacket.Timestamp}");

            // Test serialization
            var moveBuffer = new byte[1024]; // Use fixed size buffer since GetSerializedSize doesn't exist
            bool moveWriteSuccess = movePacket.Write(moveBuffer, out var moveBytesWritten);
            Console.WriteLine($"  Write Success: {moveWriteSuccess}");
            Console.WriteLine($"  Bytes Written: {moveBytesWritten}");
            if (moveBytesWritten > 0)
            {
                Console.WriteLine($"  Buffer (hex): {BitConverter.ToString(moveBuffer, 0, moveBytesWritten)}");
            }

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

            Console.WriteLine($"  SenderName Key: {ChatMessagePacket.Keys.SenderName}");
            Console.WriteLine($"  Message Key: {ChatMessagePacket.Keys.Message}");
            Console.WriteLine($"  Channel Key: {ChatMessagePacket.Keys.Channel}");
            Console.WriteLine($"  MessageType Key: {ChatMessagePacket.Keys.MessageType}");

            Console.WriteLine($"  Properties: Id={chatPacket.Id}, Sender='{chatPacket.SenderName}', Message='{chatPacket.Message}', Channel='{chatPacket.Channel}', Type={chatPacket.MessageType}, Time={chatPacket.Timestamp}");

            // Test serialization
            var chatBuffer = new byte[1024]; // Use fixed size buffer since GetSerializedSize doesn't exist
            bool chatWriteSuccess = chatPacket.Write(chatBuffer, out var chatBytesWritten);
            Console.WriteLine($"  Write Success: {chatWriteSuccess}");
            Console.WriteLine($"  Bytes Written: {chatBytesWritten}");
            if (chatBytesWritten > 0)
            {
                Console.WriteLine($"  Buffer (hex): {BitConverter.ToString(chatBuffer, 0, chatBytesWritten)}");
            }

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

            Console.WriteLine($"  PlayerId Key: {InventoryUpdatePacket.Keys.PlayerId}");
            Console.WriteLine($"  ItemId Key: {InventoryUpdatePacket.Keys.ItemId}");
            Console.WriteLine($"  Quantity Key: {InventoryUpdatePacket.Keys.Quantity}");
            Console.WriteLine($"  IsAdd Key: {InventoryUpdatePacket.Keys.IsAdd}");

            Console.WriteLine($"  Properties: Id='{inventoryPacket.Id}', Player='{inventoryPacket.PlayerId}', Item={inventoryPacket.ItemId}, Qty={inventoryPacket.Quantity}, Add={inventoryPacket.IsAdd}, Weight={inventoryPacket.ItemWeight}, Slot={inventoryPacket.SlotIndex}");

            // Test serialization
            var inventoryBuffer = new byte[1024]; // Use fixed size buffer since GetSerializedSize doesn't exist
            bool inventoryWriteSuccess = inventoryPacket.Write(inventoryBuffer, out var inventoryBytesWritten);
            Console.WriteLine($"  Write Success: {inventoryWriteSuccess}");
            Console.WriteLine($"  Bytes Written: {inventoryBytesWritten}");
            if (inventoryBytesWritten > 0)
            {
                Console.WriteLine($"  Buffer (hex): {BitConverter.ToString(inventoryBuffer, 0, inventoryBytesWritten)}");
            }

            Console.WriteLine("\n✅ Source Generator Demo Complete!");
            Console.WriteLine("\n📝 Key-Value Serialization Features:");
            Console.WriteLine("  • Each property gets a 1-byte key (index)");
            Console.WriteLine("  • Only non-default values are written");
            Console.WriteLine("  • Default values (0, false, empty string) are skipped");
            Console.WriteLine("  • Efficient for sparse data structures");
        }
    }
}
