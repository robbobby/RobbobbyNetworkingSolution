namespace Serializer.Generator.Templates.StructureTypes
{
    public static class CodeGenerationTemplate
    {
        // Main class structure templates
        public const string FileHeader = @"// Generated serialization code
#nullable enable
using System;
using System.Collections.Generic;
using Serializer.Runtime;

";

        public const string NamespaceStart = @"namespace {0}
{{
";

        public const string NamespaceEnd = @"}
";

        public const string ClassStart = @"    public partial class {0}
    {{
";

        public const string ClassEnd = @"    }
";

        // Keys class template
        public const string KeysClass = @"        public static class Keys
        {{
{0}
        }}

";

        // Write method templates
        public const string WriteMethodStart = @"        public bool Write(Span<byte> buffer, out int bytesWritten)
        {{
            bytesWritten = 0;
            var used = 0;

            try
            {{
";

        public const string WriteMethodEnd = @"                bytesWritten = used;
                return true;
            }}
            catch
            {{
                bytesWritten = 0;
                return false;
            }}
        }}

";

        // Property write templates
        public const string PropertyWriteStart = @"                // Write {0}
                if (!({1}))
                {{
";

        public const string PropertyWriteEnd = @"                }}

";

        public const string PropertyKeyWrite = @"                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.{0});
";

        // TryRead method templates
        public const string TryReadMethodStart = @"        public static bool TryRead(ReadOnlySpan<byte> buffer, ref int consumed, out {0} readPacket)
        {{
            readPacket = new {0}();

            try
            {{
                while (consumed < buffer.Length)
                {{
                    if (consumed + 4 > buffer.Length) break; // Need at least key + length

                    consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var key);
                    consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var len);

                    if (consumed + len > buffer.Length) return false; // Malformed

                    switch (key)
                    {{
";

        public const string TryReadMethodEnd = @"                        default:
                            // Unknown key - skip len bytes
                            consumed += len;
                            break;
                    }}
                }}

                return true;
            }}
            catch
            {{
                return false;
            }}
        }}

";

        // Property read templates
        public const string PropertyReadCaseStart = @"                        case Keys.{0}:
";

        public const string PropertyReadCaseEnd = @"                            break;
";

        // String write template
        public const string StringWrite = @"                    var {0}Bytes = System.Text.Encoding.UTF8.GetByteCount({0});
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)(2 + {0}Bytes)); // Length (2 for string length + string bytes)
                    used += RndCodec.WriteString(buffer.Slice(used), {0});
";

        // Nested object write template
        public const string NestedObjectWrite = @"                    // Write nested object as length-prefixed payload
                    var nestedBuffer = new byte[1024]; // Temporary buffer
                    if ({0}!.Write(nestedBuffer, out var nestedLength))
                    {{
                        used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)nestedLength);
                        nestedBuffer.AsSpan(0, nestedLength).CopyTo(buffer.Slice(used));
                        used += nestedLength;
                    }}
";

        // Array write template
        public const string ArrayWrite = @"                    var {0}TotalLength = 2; // Start with array count
                    foreach (var item in {0})
                    {{
                        var itemBuffer = new byte[1024]; // Temporary buffer
                        if (item.Write(itemBuffer, out var itemLength))
                        {{
                            {0}TotalLength += 2 + itemLength; // Item length (2 bytes) + item data
                        }}
                    }}
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort){0}TotalLength); // Total length prefix
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort){0}.Length); // Array count
                    foreach (var item in {0})
                    {{
                        var itemBuffer = new byte[1024]; // Temporary buffer
                        if (item.Write(itemBuffer, out var itemLength))
                        {{
                            used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)itemLength);
                            itemBuffer.AsSpan(0, itemLength).CopyTo(buffer.Slice(used));
                            used += itemLength;
                        }}
                    }}
";

        // String read template
        public const string StringRead = @"                            var {0}Start = consumed;
                            consumed += RndCodec.ReadString(buffer.Slice(consumed), out var {0}Value);
                            readPacket.{0} = {0}Value;
                            // Verify we didn't read beyond the expected length
                            if (consumed - {0}Start > len) return false;
";

        // Nested object read template
        public const string NestedObjectRead = @"                            // DEBUG: Original type: {0}, After replace: {1}
                            if ({1}.TryRead(buffer.Slice(consumed, len), ref consumed, out var {2}Value))
                            {{
                                readPacket.{2} = {2}Value;
                            }}
                            consumed += len;
";

        // Array read template
        public const string ArrayRead = @"                            var arrayStart = consumed;
                            consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var count);
                            var {0}List = new List<{1}>();
                            for (int i = 0; i < count; i++)
                            {{
                                consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out var itemLen);
                                if ({1}.TryRead(buffer.Slice(consumed, itemLen), ref consumed, out var item))
                                {{
                                    {0}List.Add(item);
                                }}
                                consumed += itemLen;
                            }}
                            readPacket.{0} = {0}List.ToArray();
";

        // Fallback primitive read template
        public const string FallbackPrimitiveRead = @"                            consumed += RndCodec.{0}(buffer.Slice(consumed), out var {1}Value);
                            readPacket.{1} = {1}Value;
";

        // Fallback primitive write template
        public const string FallbackPrimitiveWrite = @"                    used += RndCodec.{0}(buffer.Slice(used), {1});
";
    }
}
