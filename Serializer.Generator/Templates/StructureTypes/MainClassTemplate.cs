namespace Serializer.Generator.Templates.StructureTypes
{
    public static class MainClassTemplate
    {
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

        public const string KeysClass = @"        public static class Keys
        {{
{0}
        }}

";

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

        public const string PropertyWriteBlock = @"                // Write {0}
                if (!({1}))
                {{
                    used += RndCodec.WriteUInt16(buffer.Slice(used), Keys.{0});
{2}
                }}

";

        public const string PropertyReadCase = @"                        case Keys.{0}:
{1}
                            break;
";

        public const string DefaultValueChecks = @"
        // Default value check constants
        public const string StringDefaultCheck = ""string.IsNullOrEmpty({0})"";
        public const string BooleanDefaultCheck = ""{0} == false"";
        public const string NumericDefaultCheck = ""{0} == 0"";
        public const string FloatDefaultCheck = ""{0} == 0f"";
        public const string DoubleDefaultCheck = ""{0} == 0.0"";
        public const string EnumDefaultCheck = ""{0} == default"";
        public const string ObjectDefaultCheck = ""{0} == null"";
        public const string ArrayDefaultCheck = ""{0} == null || {0}.Length == 0"";
";
    }
}
