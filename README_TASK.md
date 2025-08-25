# SerializerStack — Full Task List

## A. Repository -- COMPLETE

* [ ] Create mono-repo with solution `RobbobbyNetworkingSolution.sln`.

  * `Serializer.Abstractions`
  * `Serializer.Runtime`
  * `Serializer.Generator`
  * `Serializer.Wire`
  * `Transport.Client`
  * `Transport.Server`
  * `Protocol.Sample`
  * `Unity.Package`
* [ ] Projects under `/tests`:

  * `Serializer.Generator.Tests`
  * `Serializer.Runtime.Tests`
  * `Serializer.Wire.Tests`
  * `Transport.Client.Tests`
  * `Transport.Server.Tests`
  * `Integration.Tests`
  * `Benchmarks.Tests`
  
## A.1 Infrastructure

* [x] Configure:

  * [x] `Directory.Build.props` for TFM (`net9.0`, `netstandard2.1` where required).
  * [x] Central `Directory.Packages.props` for package versions.
  * [x] `.editorconfig` (C# style, formatting).
  * [x] `.gitignore` (VS, Rider, Unity).
* [x] Setup CI (GitHub Actions):

  * [x] Build/test across all target frameworks (automatic detection).
  * [x] Pack NuGet artifacts.
  * [x] Smart triggers and advanced optimizations.
  * [x] Publish on tags (manual approval required).
* [x] Setup code coverage (Coverlet + ReportGenerator).
* [ ] Setup benchmark job (optional artifact upload).

---

## B. Serializer.Abstractions

* [ ] Implement `BinarySerializableAttribute`.
* [ ] Define `IPacket` (marker).
* [ ] Define `IPacket<TId>` with `TId Id { get; }`.
* [ ] Optional: `IBinaryWritable` with `Write(Span<byte>)`, `GetSerializedSize()`.
* [ ] Unit tests: attributes apply correctly, interface compile checks.

---

## C. Serializer.Runtime

* [ ] Implement `BinarySerializer`:

  * [ ] Write/Read for:

    * Boolean, byte, sbyte.
    * Int16, UInt16, Int32, UInt32, Int64, UInt64.
    * Single, Double.
    * Guid (16 bytes).
  * [ ] Write/Read string (UInt16 length prefix, UTF-8 encoded).
* [ ] Add guard methods for span length & overflow.
* [ ] Unit tests:

  * Round-trip each primitive, string (empty, max length), Guid.
  * Invalid inputs throw.
  * Fuzz/randomized input doesn’t crash.

---

## D. Serializer.Generator

* [ ] Roslyn source generator setup.
* [ ] Scan `[BinarySerializable]` types implementing `IPacket<TId>`.
* [ ] Generate for each:

  * `int Write(Span<byte> dst)`
  * `static bool TryRead(ReadOnlySpan<byte> src, out T value, out int bytesRead)`
  * `int GetSerializedSize()`
  * `int ToBytes(byte[] buffer)`
* [ ] Handle:

  * Enum underlying type width (byte/ushort/etc.).
  * Nested `[BinarySerializable]` types.
  * Arrays (fixed length).
  * Strings (via Runtime).
* [ ] Unit tests:

  * Verify generated code compiles.
  * Round-trip simple + complex packet.
  * Zero allocations (`BenchmarkDotNet` sanity).

---

## E. Serializer.Wire

* [ ] Define interfaces:

  * `IConnectionContext`
  * `IFrameReader`
  * `IFrameWriter`
  * `IBufferPolicy`
* [ ] Implement `StreamFrameReader` (4-byte LE length, partial reads).
* [ ] Implement `StreamFrameWriter` (prefix + payload).
* [ ] Implement `PooledBufferPolicy` (ArrayPool backed).
* [ ] Add configurable max frame size, reject oversize.
* [ ] Unit tests:

  * Frames round-trip via MemoryStream.
  * Partial read simulation.
  * Oversize frame rejected.

---

## F. Transport.Client

* [ ] Implement `TcpClientConnection`:

  * ctor(host, port, buffer policy).
  * `ConnectAsync`, `CloseAsync`.
  * `ReadFramesAsync()` → yields `(ctx, payload)`.
  * `SendAsync<TPacket>()` → serialize + write frame.
  * Socket options: `NoDelay`, keep-alive, timeouts.
  * Graceful reconnect option.
* [ ] Integration tests:

  * Loopback server → client connect, send, receive.
  * Partial frame splitting.
  * Multiple back-to-back frames.
  * Disconnect/reconnect.

---

## G. Transport.Server

* [ ] Implement `TcpServer`:

  * ctor(port, buffer policy).
  * `StartAsync`, `StopAsync`.
  * Accept loop using `TcpListener`.
  * Manage `ClientSession` objects (implements `IConnectionContext`).
  * `ReadFramesAsync()` across clients.
  * `BroadcastAsync<TPacket>()`.
  * Per-client send queue with back-pressure.
  * Socket options: `NoDelay`, keep-alive.
* [ ] Integration tests:

  * Start server, connect multiple clients, send/receive.
  * Broadcast to all clients.
  * Graceful stop.
  * Oversize payload rejection.

---

## H. Protocol.Sample

* [ ] Enum `ServerPacketId : byte`.
* [ ] Implement sample packets:

  * `HeartbeatPacket`
  * `AckPacket`
  * `StartPacket`
  * `ComplexTestPacket` (nested objects, arrays).
* [ ] Unit tests: round-trip each.

---

## I. App Handler Pattern

* [ ] Provide reference `IAppPacketHandler` with single `HandleAsync(ctx, payload, ct)` method.
* [ ] Demo `switch` on `ServerPacketId`, calling generated `TryRead`.
* [ ] Samples:

  * .NET console server + client.
  * Unity client.

---

## J. Tests

* [ ] Unit:

  * Runtime primitives, strings, Guid.
  * Generator packets round-trip.
  * Edge cases (truncated, oversize).
* [ ] Integration:

  * Client ↔ Server handshake with Heartbeat/Ack.
  * Back-to-back packets.
  * Partial frames.
  * Single handler pattern verified.
* [ ] Property/fuzz tests for `TryRead`.
* [ ] Benchmarks:

  * Heartbeat + ComplexTestPacket serialize/deserialize.
  * Assert B/op == 0.
  * Compare vs MessagePack-CSharp for reference.

---

## K. Unity Support

* [ ] `Unity.Package` folder with prebuilt DLLs: Abstractions, Runtime, Wire, Transport.Client.
* [ ] `package.json`, `.asmdef`, `.meta`.
* [ ] Unity sample scene (connects to server, sends Heartbeat, receives Ack).
* [ ] Confirm IL2CPP build works (AOT safety).
* [ ] Document Unity integration in README.

---

## L. Diagnostics & Safety

* [ ] Add metrics hooks (bytes in/out, frames/sec).
* [ ] Add logging callbacks (connection open/close, errors).
* [ ] Add configurable max frame size guard.
* [ ] TLS support (`SslStream`) option.
* [ ] Cancellation token support everywhere.

---

## M. Packaging & Release

* [ ] NuGet pack for each project.
* [ ] Analyzer packaging for Generator (`PrivateAssets=all`).
* [ ] SourceLink enabled.
* [ ] Symbol packages (`.snupkg`).
* [ ] UPM package built + published.
* [ ] Release pipeline: semantic versioning, changelog, tag, publish.
* [ ] Smoke test in .NET and Unity.

---

## N. Documentation

* [ ] Top-level README (what, why, single handler usage).
* [ ] Project APIs Reference (done).
* [ ] Architecture doc (framing diagram, generator design).
* [ ] Unity quick start doc.
* [ ] FAQ: frame size, reconnects, TLS, IL2CPP.
