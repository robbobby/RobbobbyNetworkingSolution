# SerializerStack — Quick Guide

A reusable, fixed-layout binary **serializer** with **TCP client/server transport**, split for .NET and Unity.
Use the generator for compile-time codecs; ship the runtime + transports via NuGet (and UPM for Unity).

---

## Projects at a glance

| Project                     | Type             | Targets                    | Purpose                                                                                                                          |
| --------------------------- | ---------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| **Serializer.Abstractions** | NuGet            | `net9.0`, `netstandard2.1` | Attributes (`[BinarySerializable]`), `IPacket`, small shared interfaces. Zero deps.                                              |
| **Serializer.Runtime**      | NuGet            | `net9.0`, `netstandard2.1` | `BinarySerializer` helpers (Span-based read/write of primitives, strings, Guid).                                                 |
| **Serializer.Generator**    | NuGet (Analyzer) | `netstandard2.0`           | Roslyn source generator. Emits `Write(Span<byte>)`, `TryRead(ReadOnlySpan<byte> ...)`, `GetSerializedSize()`, `ToBytes(byte[])`. |
| **Serializer.Wire**         | NuGet            | `net9.0`, `netstandard2.1` | Shared wire utilities: 4-byte LE length-prefixed framing, dispatch contracts, buffer policy.                                     |
| **Transport.Client**        | NuGet + UPM      | `net9.0`, `netstandard2.1` | TCP **client** (Unity-safe). Connect, send, receive loop, per-connection buffers.                                                |
| **Transport.Server**        | NuGet            | `net9.0`                   | TCP **server**: listener, client sessions, broadcast, buffer mgmt. (server-only).                                                |
| **Protocol.Sample**         | (optional)       | `net9.0`, `netstandard2.1` | Example packets (Heartbeat, Ack, Start, ComplexTestPacket) to validate generation.                                               |
| **Unity.Package**           | UPM              | Unity 2021+                | Ships **compiled** DLLs from Abstractions + Runtime + Wire + Transport.Client (no generator).                                    |

---

## How reading & writing works

Both **server** and **client** use the same mechanism:

1. **Read**

    * Read the 4‑byte length prefix (UInt32 LE).
    * Read exactly `length` bytes into a buffer.
    * Pass the payload to a single `PacketRouter.Dispatch(...)` method.

2. **Write**

    * Ask the packet for its `GetSerializedSize()`.
    * Write the size prefix + payload via its generated `Write(...)` method.

This means the logic for *switching on packet id* and *invoking handlers* lives in one place — the **PacketRouter**.

---

## Minimal usage (copy‑paste)

> Package handles **read/write + TCP framing**. Your app provides **one handler** that switches on Id and calls generated `TryRead`.

### 1) Create a single app handler

```csharp
public interface IAppPacketHandler
{
    ValueTask HandleAsync(IConnectionContext ctx, ReadOnlySpan<byte> payload, CancellationToken ct);
}

public sealed class AppPacketHandler : IAppPacketHandler
{
    public async ValueTask HandleAsync(IConnectionContext ctx, ReadOnlySpan<byte> payload, CancellationToken ct)
    {
        if (payload.IsEmpty) return;
        var id = (ServerPacketId)payload[0]; // adjust if IdType != byte

        switch (id)
        {
            case ServerPacketId.Heartbeat:
                if (HeartbeatPacket.TryRead(payload, out var hb, out _))
                    await OnHeartbeatAsync(ctx, hb, ct);
                break;
            case ServerPacketId.Ack:
                if (AckPacket.TryRead(payload, out var ack, out _))
                    await OnAckAsync(ctx, ack, ct);
                break;
            case ServerPacketId.Start:
                if (StartPacket.TryRead(payload, out var st, out _))
                    await OnStartAsync(ctx, st, ct);
                break;
            default:
                // unknown id
                break;
        }
    }

    static ValueTask OnHeartbeatAsync(IConnectionContext ctx, HeartbeatPacket p, CancellationToken ct) => ValueTask.CompletedTask;
    static ValueTask OnAckAsync(IConnectionContext ctx, AckPacket p, CancellationToken ct) => ValueTask.CompletedTask;
    static ValueTask OnStartAsync(IConnectionContext ctx, StartPacket p, CancellationToken ct) => ValueTask.CompletedTask;
}
```

### 2) Server: start, read loop, (optional) broadcast

```csharp
var handler = new AppPacketHandler();
var serverBuffers = new PooledBufferPolicy(64 * 1024);
var server = new TcpServer(7777, serverBuffers); // from package
await server.StartAsync();

_ = Task.Run(async () =>
{
    await foreach (var (ctx, frame) in server.ReadFramesAsync())
        await handler.HandleAsync(ctx, frame.Span, CancellationToken.None);
});

await server.BroadcastAsync(new AckPacket { Success = true, Message = "Welcome" });
```

### 3) Client: connect, read loop, send

```csharp
var handler = new AppPacketHandler();
var clientBuffers = new PooledBufferPolicy(64 * 1024);
var client = new TcpClientConnection("127.0.0.1", 7777, clientBuffers); // from package
await client.ConnectAsync();

_ = Task.Run(async () =>
{
    await foreach (var (ctx, frame) in client.ReadFramesAsync())
        await handler.HandleAsync(ctx, frame.Span, CancellationToken.None);
});

// Send using generated Write(Span<byte>) under the hood
await client.SendAsync(new HeartbeatPacket { Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() });
```

That’s it: **one handler**, used by client and server. The package stays focused on **read/write/TCP**.

---

## Client quick start (Unity or .NET)

```csharp
var router = new PacketRouter();
var buffers = new PooledBufferPolicy(64 * 1024);

var client = new TcpClientConnection("127.0.0.1", 7777, router, buffers);
await client.ConnectAsync();

await client.SendAsync(new HeartbeatPacket
{
    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
});
```

---

## Server quick start (.NET only)

```csharp
var router = new PacketRouter();
var buffers = new PooledBufferPolicy(64 * 1024);

var server = new TcpServer(7777, router, buffers);
await server.StartAsync();

await server.BroadcastAsync(new AckPacket { Success = true, Message = "Welcome" });
```

---

## Complex nested packet (for testing)

```csharp
[BinarySerializable]
public sealed partial class ComplexTestPacket : IPacket<ServerPacketId>
{
    public ServerPacketId Id { get; } = ServerPacketId.Ack;
    public int SessionId { get; set; }
    public PlayerInfo Player { get; set; } = new();
    public WorldState State { get; set; } = new();
    public InventoryItem[] Hotbar { get; set; } = new InventoryItem[5];
}

[BinarySerializable] public sealed partial class PlayerInfo
{
    public Guid PlayerId { get; set; }
    public int Level { get; set; }
    public Stats Stats { get; set; } = new();
    public Skill[] Skills { get; set; } = new Skill[3];
}

[BinarySerializable] public sealed partial class Stats
{
    public short Str { get; set; }
    public short Dex { get; set; }
    public short Int { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
}

[BinarySerializable] public sealed partial class Skill
{
    public ushort Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte Rank { get; set; }
}

[BinarySerializable] public sealed partial class WorldState
{
    public long Tick { get; set; }
    public Vec3 PlayerPos { get; set; } = new();
    public Region Region { get; set; } = new();
}

[BinarySerializable] public sealed partial class Vec3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

[BinarySerializable] public sealed partial class Region
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

[BinarySerializable] public sealed partial class InventoryItem
{
    public int ItemId { get; set; }
    public byte Quantity { get; set; }
}
```

---

## Separation of responsibilities

**This package is only responsible for:**

* Binary **read/write** (generated `Write`/`TryRead`/`GetSerializedSize`).
* TCP **framing + I/O** (4‑byte LE length prefix, partial‑read handling, per‑connection buffers).

**Your app is responsible for:**

* A single **`IAppPacketHandler`** that switches on Id and handles packets.

---

## Writing (both sides)

Use the generated `Write(Span<byte>)` plus the package’s frame writer:

```csharp
async ValueTask SendAsync(IFrameWriter writer, object packet, byte[] tx, CancellationToken ct)
{
    var span = tx.AsSpan();
    int size = packet switch
    {
        HeartbeatPacket p => p.Write(span),
        AckPacket p       => p.Write(span),
        StartPacket p     => p.Write(span),
        _ => throw new NotSupportedException(packet.GetType().Name)
    };
    await writer.WriteFrameAsync(tx.AsMemory(0, size), ct);
}
```
