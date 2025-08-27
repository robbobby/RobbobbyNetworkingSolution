namespace Serializer.Generator.Templates
{
    public static class ClassTemplate
    {
        public const string ClassHeader = @"// Generated serialization code
#nullable enable
using System;
using System.Collections.Generic;
using Serializer.Runtime;

namespace {0}
{{
    public partial class {1}
    {{
{2}
    }}
}}";

        public const string KeysClass = @"        public static class Keys
        {{
{0}
        }}

";

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

        public const string TryReadMethod = @"        public static bool TryRead(ReadOnlySpan<byte> buffer, out {0} result, out int bytesConsumed)
        {{
            result = new {0}();
            bytesConsumed = 0;
            var used = 0;

            try
            {{
{1}
                bytesConsumed = used;
                return true;
            }}
            catch
            {{
                bytesConsumed = 0;
                return false;
            }}
        }}

";

        public const string PropertyWriteBlock = @"                // Write {0}
                if (!({1}))
                {{
{2}
                }}

";

        public const string PropertyReadBlock = @"                // Read {0}
                if (used + 4 <= buffer.Length) // Key (2) + Length (2)
                {{
{1}
                }}

";
    }
}
