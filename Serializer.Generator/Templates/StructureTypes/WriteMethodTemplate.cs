namespace Serializer.Generator.Templates.StructureTypes
{
    public static class WriteMethodTemplate
    {
        public const string WriteMethod = @"        public bool Write(Span<byte> buffer, out int bytesWritten)
        {{
            bytesWritten = 0;
            var used = 0;

            try
            {{
{0}
                bytesWritten = used;
                return true;
            }}
            catch
            {{
                bytesWritten = 0;
                return false;
            }}
        }}

";

        public const string PropertyWriteBlock = @"                // Write {0}
                if (!({1}))
                {{
{2}
{3}
                }}

";

        public const string PrimitiveWrite = @"                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort){0}); // Length
                    used += RndCodec.{1}(buffer.Slice(used), {2});";

        public const string StringWrite = @"                    var {0}Bytes = System.Text.Encoding.UTF8.GetByteCount({0});
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)(2 + {0}Bytes)); // Length (2 for string length + string bytes)
                    used += RndCodec.WriteString(buffer.Slice(used), {0});";

        public const string NestedWrite = @"                    // Write nested object as length-prefixed payload
                    var nestedBuffer = new byte[1024]; // Temporary buffer
                    if ({0}!.Write(nestedBuffer, out var nestedLength))
                    {{
                        used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort)nestedLength);
                        nestedBuffer.AsSpan(0, nestedLength).CopyTo(buffer.Slice(used));
                        used += nestedLength;
                    }}";

        public const string ArrayWrite = @"                    var {0}TotalLength = 2; // Start with array count
                    var {0}TempBuffer = new byte[1024]; // Temporary buffer for array items
                    var {0}ItemOffset = 0;

                    foreach (var item in {0})
                    {{
                        if (item.Write({0}TempBuffer.AsSpan({0}ItemOffset), out var itemLength))
                        {{
                            {0}TotalLength += itemLength;
                            {0}ItemOffset += itemLength;
                        }}
                    }}

                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort){0}TotalLength);
                    used += RndCodec.WriteUInt16(buffer.Slice(used), (ushort){0}.Length);
                    {0}TempBuffer.AsSpan(0, {0}ItemOffset).CopyTo(buffer.Slice(used));
                    used += {0}ItemOffset;";
    }
}
