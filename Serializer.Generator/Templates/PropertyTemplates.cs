namespace Serializer.Generator.Templates
{
    public static class PropertyTemplates
    {
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

        public const string PrimitiveRead = @"                    var {0}Length = RndCodec.ReadUInt16(buffer.Slice(used + 2));
                    used += 4; // Key + Length
                    if (used + {0}Length <= buffer.Length)
                    {{
                        {1} = RndCodec.{2}(buffer.Slice(used));
                        used += {0}Length;
                    }}";

        public const string StringRead = @"                    var {0}Length = RndCodec.ReadUInt16(buffer.Slice(used + 2));
                    used += 4; // Key + Length
                    if (used + {0}Length <= buffer.Length)
                    {{
                        {0} = RndCodec.ReadString(buffer.Slice(used));
                        used += {0}Length;
                    }}";

        public const string NestedRead = @"                    var {0}Length = RndCodec.ReadUInt16(buffer.Slice(used + 2));
                    used += 4; // Key + Length
                    if (used + {0}Length <= buffer.Length)
                    {{
                        if ({1}.TryRead(buffer.Slice(used, {0}Length), out var {0}Temp, out var {0}Consumed))
                        {{
                            {0} = {0}Temp;
                            used += {0}Consumed;
                        }}
                    }}";

        public const string ArrayRead = @"                    var {0}TotalLength = RndCodec.ReadUInt16(buffer.Slice(used + 2));
                    var {0}Count = RndCodec.ReadUInt16(buffer.Slice(used + 4));
                    used += 6; // Key + TotalLength + Count
                    if (used + {0}TotalLength - 2 <= buffer.Length) // -2 for count
                    {{
                        {0} = new {1}[{0}Count];
                        var {0}ItemOffset = 0;
                        for (int i = 0; i < {0}Count; i++)
                        {{
                            if ({2}.TryRead(buffer.Slice(used + {0}ItemOffset), out var item, out var itemConsumed))
                            {{
                                {0}[i] = item;
                                {0}ItemOffset += itemConsumed;
                            }}
                        }}
                        used += {0}ItemOffset;
                    }}";
    }
}
