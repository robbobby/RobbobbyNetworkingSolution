# SerializerStack – Project APIs Reference

> Short, consumer‑focused reference for each package: what it is and the main public surface you call. (Signatures are illustrative; minor names may vary during implementation.)

---

## Serializer.Abstractions (NuGet)

**What it is:** shared contracts and attributes; no runtime dependencies.

### Key types

```csharp
namespace Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class BinarySerializableAttribute : Attribute {}

    public interface IPacket { }
    public interface IPacket<TId> : IPacket
    {
        TId Id { get; }
    }

    // Optional – implemented by generated code if you choose
    public interface IBinaryWritable
    {
        int Write(Span<byte> destination);
        int GetSerializedSize();
    }
}
```

---

## Serializer.Runtime (NuGet / UPM)

**What it is:** span‑based helpers used by generated code and transports.

### Core APIs

```csharp
public static class BinarySerializer
{
    // Integers (LE)
    static void   WriteInt16(Span<byte> dst, short value);
    static short  ReadInt16(ReadOnlySpan<byte> src);
    static void   WriteUInt16(Span<byte> dst, ushort value);
    static ushort ReadUInt16(ReadOnlySpan<byte> src);
    static void   WriteInt32(Span<byte> dst, int value);
    static int    ReadInt32(ReadOnlySpan<byte> src);
    static void   WriteUInt32(Span<byte> dst, uint value);
    static uint   ReadUInt32(ReadOnlySpan<byte> src);
    static void   WriteInt64(Span<byte> dst, long value);
    static long   ReadInt64(ReadOnlySpan<byte> src);

    // Floating point
    static void   WriteSingle(Span<byte> dst, float value);
    static float  ReadSingle(ReadOnlySpan<byte> src);
    static void   WriteDouble(Span<byte> dst, double value);
    static double ReadDouble(ReadOnlySpan<byte> src);

    // Bool / Byte
    static void   WriteBoolean(Span<byte> dst, bool value);
    static bool   ReadBoolean(ReadOnlySpan<byte> src);

    // Strings: UTF‑8 with UInt16 byte length prefix
    static int    WriteString(Span<byte> dst, string value);
    static int    ReadString(ReadOnlySpan<byte> src, out string value);

    // Guid as 16 bytes
    static void   WriteGuid(Span<byte> dst, Guid value);
    static Guid   ReadGuid(ReadOnlySpan<byte> src);
}
```

### Generated members (appear on your `[BinarySerializable]` types)

```csharp
// on TPacket
int Write(Span<byte> destination);
int GetSerializedSize();
int ToBytes(byte[] buffer);
static bool TryRead(ReadOnlySpan<byte> source, out TPacket value, out int bytesRead);
```

---

## Serializer.Generator (NuGet Analyzer)

**What it is:** Roslyn source generator that adds the methods above to your annotated types at **compile‑time**. No runtime API.

### Usage

* Reference the analyzer package.
* Annotate your classes/structs with `[BinarySerializable]`.
* Implement `IPacket<TId>` with the `Id` property.

---

## Serializer.Wire (NuGet / UPM)

**What it is:** shared wire‑level contracts (framing, buffers, minimal abstractions).

### Key interfaces

```csharp
public interface IConnectionContext
{
    int ConnectionId { get; }
    EndPoint? RemoteEndPoint { get; }
    ValueTask SendAsync<T>(T packet, CancellationToken ct = default) where T : class;
}

public interface IFrameReader
{
    // Returns exactly one payload (without the 4‑byte length prefix)
    ValueTask<ReadOnlyMemory<byte>> ReadFrameAsync(CancellationToken ct = default);
}

public interface IFrameWriter
{
    // Writes [u32 length][payload]
    ValueTask WriteFrameAsync(ReadOnlyMemory<byte> payload, CancellationToken ct = default);
}

public interface IBufferPolicy
{
    byte[] RentRxBuffer(int minSize);
    void   ReturnRxBuffer(byte[] buffer);
}
```

---

## Transport.Client (NuGet / UPM)

**What it is:** Unity‑safe **TCP client** with 4‑byte LE length‑prefix framing.

### Main APIs

```csharp
public sealed class TcpClientConnection
{
    public TcpClientConnection(string host, int port, IBufferPolicy buffers);

    public Task ConnectAsync(CancellationToken ct = default);
    public Task CloseAsync(CancellationToken ct = default);

    // Read payload frames as they arrive (without length prefix)
    public IAsyncEnumerable<(IConnectionContext Context, ReadOnlyMemory<byte> Payload)> ReadFramesAsync(CancellationToken ct = default);

    // Serialize with generated Write(Span<byte>) and send (framed)
    public ValueTask SendAsync<TPacket>(TPacket packet, CancellationToken ct = default) where TPacket : class;
}
```

---

## Transport.Server (NuGet)

**What it is:** **TCP server** (listener + sessions + broadcast) with framing.

### Main APIs

```csharp
public sealed class TcpServer
{
    public TcpServer(int port, IBufferPolicy buffers);

    public Task StartAsync(CancellationToken ct = default);
    public Task StopAsync(CancellationToken ct = default);

    public IReadOnlyCollection<ClientSession> Clients { get; }

    // Read incoming frames from all clients
    public IAsyncEnumerable<(IConnectionContext Context, ReadOnlyMemory<byte> Payload)> ReadFramesAsync(CancellationToken ct = default);

    // Broadcast a typed packet to all clients
    public ValueTask BroadcastAsync<TPacket>(TPacket packet, CancellationToken ct = default) where TPacket : class;
}

public sealed class ClientSession : IConnectionContext
{
    public int ConnectionId { get; }
    public EndPoint? RemoteEndPoint { get; }

    public ValueTask SendAsync<TPacket>(TPacket packet, CancellationToken ct = default) where TPacket : class;
}
```

---

## Unity.Package (UPM)

**What it is:** Unity wrapper shipping compiled DLLs from:

* **Serializer.Abstractions**
* **Serializer.Runtime**
* **Serializer.Wire**
* **Transport.Client**

### How you use it in Unity

* Define packets (`[BinarySerializable]`) in a **.NET library** that references the Analyzer (generator) during build, or include pre‑generated code.
* In Unity, reference the UPM package and the compiled packet assembly; call `TcpClientConnection` and your generated methods.

---

## Protocol.Sample (optional project)

**What it is:** Examples used by tests/samples; not required in consumer apps.

### Example types

```csharp
[BinarySerializable]
public sealed partial class HeartbeatPacket : IPacket<ServerPacketId>
{
    public ServerPacketId Id { get; } = ServerPacketId.Heartbeat;
    public long Timestamp { get; set; }
}

[BinarySerializable]
public sealed partial class AckPacket : IPacket<ServerPacketId>
{
    public ServerPacketId Id { get; } = ServerPacketId.Ack;
    public bool   Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
```

---

## Notes

* All framing is `[u32 little‑endian length][payload]`; length excludes the prefix.
* First field of each payload is the packet `Id`, written using its underlying size (`byte/ushort/...`).
* The generator aims for **zero allocations** on the hot path; use per‑connection buffers.
