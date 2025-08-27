using Serializer.Abstractions;

namespace Serializer.Generator.Tests
{
    /// <summary>
    /// Comprehensive test packet that includes ALL data types the generator can handle
    /// This allows us to test every single read/write method the generator creates
    /// </summary>
    public partial class ComprehensiveTestPacket : IRnsPacket<ComprehensivePacketType>
    {
        public ComprehensivePacketType Id { get; set; }

        // All primitive types the generator supports
        public bool BooleanField { get; set; }
        public byte ByteField { get; set; }
        public sbyte SByteField { get; set; }
        public short Int16Field { get; set; }
        public ushort UInt16Field { get; set; }
        public int Int32Field { get; set; }
        public uint UInt32Field { get; set; }
        public long Int64Field { get; set; }
        public ulong UInt64Field { get; set; }
        public float SingleField { get; set; }
        public double DoubleField { get; set; }
        public Guid GuidField { get; set; }
        public Guid GuidRfc4122Field { get; set; }

        // String type
        public string StringField { get; set; } = string.Empty;

        // Nested object types
        public ComprehensiveTestFieldPacket NestedObjectField { get; set; } = null!;
        public ComprehensiveTestFieldPacket2 DeepNestedObjectField { get; set; } = null!;

        // Array types
        public IReadOnlyList<ComprehensiveTestFieldPacket> ObjectArrayField { get; set; } = null!;
        public IReadOnlyList<ComprehensiveTestFieldPacket2> DeepNestedArrayField { get; set; } = null!;

        // Mixed complex structure
        public ComprehensiveTestFieldPacket MixedNestedField { get; set; } = null!;
        public IReadOnlyList<ComprehensiveTestFieldPacket> MixedArrayField { get; set; } = null!;
    }

    /// <summary>
    /// Nested test packet for testing nested object serialization
    /// </summary>
    public partial class ComprehensiveTestFieldPacket : IRnsPacketField
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public float FloatValue { get; set; }
        public bool BoolValue { get; set; }
        public ComprehensiveTestFieldPacket2 NestedField { get; set; } = null!;
    }

    /// <summary>
    /// Deeply nested test packet for testing complex nesting
    /// </summary>
    public partial class ComprehensiveTestFieldPacket2 : IRnsPacketField
    {
        public string DeepName { get; set; } = string.Empty;
        public double DoubleValue { get; set; }
        public long LongValue { get; set; }
        public Guid GuidValue { get; set; }
    }

    /// <summary>
    /// Enum for packet type identification
    /// </summary>
    public enum ComprehensivePacketType
    {
        None = 0,
        Comprehensive = 1,
        Nested = 2,
        DeepNested = 3
    }
}
