ecusing System;
using System.Linq;
using Xunit;

namespace Serializer.Generator.Tests
{
    public class ComprehensiveGeneratorTests
    {
        [Fact]
        public void ComprehensiveTestPacket_BooleanField_WriteReadCorrectly()
        {
            // Test boolean serialization
            var packet = new ComprehensiveTestPacket
            {
                BooleanField = true
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify boolean field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(1, keys); // BooleanField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.True(deserialized.BooleanField);
        }

        [Fact]
        public void ComprehensiveTestPacket_ByteField_WriteReadCorrectly()
        {
            // Test byte serialization
            var packet = new ComprehensiveTestPacket
            {
                ByteField = 42
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify byte field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(2, keys); // ByteField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal((byte)42, deserialized.ByteField);
        }

        [Fact]
        public void ComprehensiveTestPacket_SByteField_WriteReadCorrectly()
        {
            // Test sbyte serialization
            var packet = new ComprehensiveTestPacket
            {
                SByteField = -42
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify sbyte field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(3, keys); // SByteField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal((sbyte)-42, deserialized.SByteField);
        }

        [Fact]
        public void ComprehensiveTestPacket_Int16Field_WriteReadCorrectly()
        {
            // Test short serialization
            var packet = new ComprehensiveTestPacket
            {
                Int16Field = -12345
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify short field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(4, keys); // Int16Field key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal((short)-12345, deserialized.Int16Field);
        }

        [Fact]
        public void ComprehensiveTestPacket_UInt16Field_WriteReadCorrectly()
        {
            // Test ushort serialization
            var packet = new ComprehensiveTestPacket
            {
                UInt16Field = 54321
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify ushort field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(5, keys); // UInt16Field key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal((ushort)54321, deserialized.UInt16Field);
        }

        [Fact]
        public void ComprehensiveTestPacket_Int32Field_WriteReadCorrectly()
        {
            // Test int serialization
            var packet = new ComprehensiveTestPacket
            {
                Int32Field = -987654321
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify int field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(6, keys); // Int32Field key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(-987654321, deserialized.Int32Field);
        }

        [Fact]
        public void ComprehensiveTestPacket_UInt32Field_WriteReadCorrectly()
        {
            // Test uint serialization
            var packet = new ComprehensiveTestPacket
            {
                UInt32Field = 987654321
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify uint field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(7, keys); // UInt32Field key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(987654321U, deserialized.UInt32Field);
        }

        [Fact]
        public void ComprehensiveTestPacket_Int64Field_WriteReadCorrectly()
        {
            // Test long serialization
            var packet = new ComprehensiveTestPacket
            {
                Int64Field = -1234567890123456789L
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify long field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(8, keys); // Int64Field key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(-1234567890123456789L, deserialized.Int64Field);
        }

        [Fact]
        public void ComprehensiveTestPacket_UInt64Field_WriteReadCorrectly()
        {
            // Test ulong serialization
            var packet = new ComprehensiveTestPacket
            {
                UInt64Field = 1234567890123456789UL
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify ulong field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(9, keys); // UInt64Field key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(1234567890123456789UL, deserialized.UInt64Field);
        }

        [Fact]
        public void ComprehensiveTestPacket_SingleField_WriteReadCorrectly()
        {
            // Test float serialization
            var packet = new ComprehensiveTestPacket
            {
                SingleField = 3.14159f
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify float field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(10, keys); // SingleField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(3.14159f, deserialized.SingleField);
        }

        [Fact]
        public void ComprehensiveTestPacket_DoubleField_WriteReadCorrectly()
        {
            // Test double serialization
            var packet = new ComprehensiveTestPacket
            {
                DoubleField = 2.718281828459045
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify double field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(11, keys); // DoubleField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(2.718281828459045, deserialized.DoubleField);
        }

        [Fact]
        public void ComprehensiveTestPacket_GuidField_WriteReadCorrectly()
        {
            // Test Guid serialization
            var testGuid = Guid.NewGuid();
            var packet = new ComprehensiveTestPacket
            {
                GuidField = testGuid
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify Guid field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(12, keys); // GuidField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(testGuid, deserialized.GuidField);
        }

        [Fact]
        public void ComprehensiveTestPacket_GuidRfc4122Field_WriteReadCorrectly()
        {
            // Test GuidRfc4122 serialization
            var testGuid = Guid.NewGuid();
            var packet = new ComprehensiveTestPacket
            {
                GuidRfc4122Field = testGuid
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify GuidRfc4122 field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(13, keys); // GuidRfc4122Field key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(testGuid, deserialized.GuidRfc4122Field);
        }

        [Fact]
        public void ComprehensiveTestPacket_StringField_WriteReadCorrectly()
        {
            // Test string serialization
            var packet = new ComprehensiveTestPacket
            {
                StringField = "Hello, World!"
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify string field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(14, keys); // StringField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal("Hello, World!", deserialized.StringField);
        }

        [Fact]
        public void ComprehensiveTestPacket_NestedObjectField_WriteReadCorrectly()
        {
            // Test nested object serialization
            var packet = new ComprehensiveTestPacket
            {
                NestedObjectField = new ComprehensiveTestFieldPacket
                {
                    Name = "Nested String",
                    Value = 42
                }
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify nested object field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(15, keys); // NestedObjectField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.NotNull(deserialized.NestedObjectField);
            Assert.Equal("Nested String", deserialized.NestedObjectField.Name);
            Assert.Equal(42, deserialized.NestedObjectField.Value);
        }

        [Fact]
        public void ComprehensiveTestPacket_DeepNestedObjectField_WriteReadCorrectly()
        {
            // Test deep nested object serialization
            var packet = new ComprehensiveTestPacket
            {
                DeepNestedObjectField = new ComprehensiveTestFieldPacket2
                {
                    DeepName = "Deep Nested String",
                    DoubleValue = 99.0,
                    LongValue = 123L
                }
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify deep nested object field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(16, keys); // DeepNestedObjectField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.NotNull(deserialized.DeepNestedObjectField);
            Assert.Equal("Deep Nested String", deserialized.DeepNestedObjectField.DeepName);
            Assert.Equal(99.0, deserialized.DeepNestedObjectField.DoubleValue);
            Assert.Equal(123L, deserialized.DeepNestedObjectField.LongValue);
        }

        [Fact]
        public void ComprehensiveTestPacket_ObjectArrayField_WriteReadCorrectly()
        {
            // Test object array serialization
            var packet = new ComprehensiveTestPacket
            {
                ObjectArrayField = new[]
                {
                    new ComprehensiveTestFieldPacket { Name = "Array Item 1", Value = 1 },
                    new ComprehensiveTestFieldPacket { Name = "Array Item 2", Value = 2 },
                    new ComprehensiveTestFieldPacket { Name = "Array Item 3", Value = 3 }
                }
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify object array field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(17, keys); // ObjectArrayField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.NotNull(deserialized.ObjectArrayField);
            Assert.Equal(3, deserialized.ObjectArrayField.Count);
            Assert.Equal("Array Item 1", deserialized.ObjectArrayField[0].Name);
            Assert.Equal(1, deserialized.ObjectArrayField[0].Value);
            Assert.Equal("Array Item 2", deserialized.ObjectArrayField[1].Name);
            Assert.Equal(2, deserialized.ObjectArrayField[1].Value);
            Assert.Equal("Array Item 3", deserialized.ObjectArrayField[2].Name);
            Assert.Equal(3, deserialized.ObjectArrayField[2].Value);
        }

        [Fact]
        public void ComprehensiveTestPacket_DeepNestedArrayField_WriteReadCorrectly()
        {
            // Test deep nested array serialization
            var packet = new ComprehensiveTestPacket
            {
                DeepNestedArrayField = new[]
                {
                    new ComprehensiveTestFieldPacket2
                    {
                        DeepName = "Deep Array Item 1",
                        DoubleValue = 10.0,
                        LongValue = 100L
                    },
                    new ComprehensiveTestFieldPacket2
                    {
                        DeepName = "Deep Array Item 2",
                        DoubleValue = 20.0,
                        LongValue = 200L
                    }
                }
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify deep nested array field is written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(18, keys); // DeepNestedArrayField key

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.NotNull(deserialized.DeepNestedArrayField);
            Assert.Equal(2, deserialized.DeepNestedArrayField.Count);
            Assert.Equal("Deep Array Item 1", deserialized.DeepNestedArrayField[0].DeepName);
            Assert.Equal(10.0, deserialized.DeepNestedArrayField[0].DoubleValue);
            Assert.Equal(100L, deserialized.DeepNestedArrayField[0].LongValue);
            Assert.Equal("Deep Array Item 2", deserialized.DeepNestedArrayField[1].DeepName);
            Assert.Equal(20.0, deserialized.DeepNestedArrayField[1].DoubleValue);
            Assert.Equal(200L, deserialized.DeepNestedArrayField[1].LongValue);
        }

        [Fact]
        public void ComprehensiveTestPacket_AllFields_WriteReadCorrectly()
        {
            // Test all fields together
            var packet = new ComprehensiveTestPacket
            {
                Id = ComprehensivePacketType.Comprehensive,
                BooleanField = true,
                ByteField = 42,
                SByteField = -42,
                Int16Field = -12345,
                UInt16Field = 54321,
                Int32Field = -987654321,
                UInt32Field = 987654321,
                Int64Field = -1234567890123456789L,
                UInt64Field = 1234567890123456789UL,
                SingleField = 3.14159f,
                DoubleField = 2.718281828459045,
                GuidField = Guid.NewGuid(),
                GuidRfc4122Field = Guid.NewGuid(),
                StringField = "All Fields Test",
                NestedObjectField = new ComprehensiveTestFieldPacket { Name = "Nested", Value = 42 },
                DeepNestedObjectField = new ComprehensiveTestFieldPacket2 { DeepName = "Deep", DoubleValue = 99.0 },
                ObjectArrayField = new[] { new ComprehensiveTestFieldPacket { Name = "Array", Value = 1 } },
                DeepNestedArrayField = new[] { new ComprehensiveTestFieldPacket2 { DeepName = "Deep Array", DoubleValue = 10.0 } }
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Equal(ComprehensivePacketType.Comprehensive, deserialized.Id);
            Assert.True(deserialized.BooleanField);
            Assert.Equal((byte)42, deserialized.ByteField);
            Assert.Equal((sbyte)-42, deserialized.SByteField);
            Assert.Equal((short)-12345, deserialized.Int16Field);
            Assert.Equal((ushort)54321, deserialized.UInt16Field);
            Assert.Equal(-987654321, deserialized.Int32Field);
            Assert.Equal(987654321U, deserialized.UInt32Field);
            Assert.Equal(-1234567890123456789L, deserialized.Int64Field);
            Assert.Equal(1234567890123456789UL, deserialized.UInt64Field);
            Assert.Equal(3.14159f, deserialized.SingleField);
            Assert.Equal(2.718281828459045, deserialized.DoubleField);
            Assert.Equal(packet.GuidField, deserialized.GuidField);
            Assert.Equal(packet.GuidRfc4122Field, deserialized.GuidRfc4122Field);
            Assert.Equal("All Fields Test", deserialized.StringField);
            Assert.NotNull(deserialized.NestedObjectField);
            Assert.Equal("Nested", deserialized.NestedObjectField.Name);
            Assert.Equal(42, deserialized.NestedObjectField.Value);
            Assert.NotNull(deserialized.DeepNestedObjectField);
            Assert.Equal("Deep", deserialized.DeepNestedObjectField.DeepName);
            Assert.Equal(99.0, deserialized.DeepNestedObjectField.DoubleValue);
            Assert.NotNull(deserialized.ObjectArrayField);
            Assert.Single(deserialized.ObjectArrayField);
            Assert.Equal("Array", deserialized.ObjectArrayField[0].Name);
            Assert.Equal(1, deserialized.ObjectArrayField[0].Value);
            Assert.NotNull(deserialized.DeepNestedArrayField);
            Assert.Single(deserialized.DeepNestedArrayField);
            Assert.Equal("Deep Array", deserialized.DeepNestedArrayField[0].DeepName);
            Assert.Equal(10.0, deserialized.DeepNestedArrayField[0].DoubleValue);
        }

        [Fact]
        public void ComprehensiveTestPacket_NullValues_HandledCorrectly()
        {
            // Test that null values are handled correctly (omitted from serialization)
            var packet = new ComprehensiveTestPacket
            {
                // Only set a few fields, leave others as null/default
                BooleanField = true,
                StringField = "Only Some Fields"
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify only non-null fields are written
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.Contains<ushort>(1, keys); // BooleanField key
            Assert.Contains<ushort>(14, keys); // StringField key
            Assert.DoesNotContain<ushort>(2, keys); // ByteField key (should be omitted)
            Assert.DoesNotContain<ushort>(15, keys); // NestedObjectField key (should be omitted)

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.True(deserialized.BooleanField);
            Assert.Equal("Only Some Fields", deserialized.StringField);
            Assert.False(deserialized.ByteField != 0); // Should be default value
            Assert.Null(deserialized.NestedObjectField); // Should be null
        }

        [Fact]
        public void ComprehensiveTestPacket_EmptyArrays_HandledCorrectly()
        {
            // Test that empty arrays are handled correctly (omitted from serialization)
            var packet = new ComprehensiveTestPacket
            {
                Id = ComprehensivePacketType.Comprehensive,
                ObjectArrayField = Array.Empty<ComprehensiveTestFieldPacket>(),
                DeepNestedArrayField = Array.Empty<ComprehensiveTestFieldPacket2>()
            };

            var buffer = new byte[1024];
            bool writeSuccess = packet.Write(buffer, out var bytesWritten);
            Assert.True(writeSuccess);
            Assert.True(bytesWritten > 0);

            // Verify empty arrays are omitted
            var keys = ParseKeysFromBuffer(buffer, bytesWritten);
            Assert.DoesNotContain<ushort>(17, keys); // ObjectArrayField key (should be omitted)
            Assert.DoesNotContain<ushort>(18, keys); // DeepNestedArrayField key (should be omitted)

            // Round-trip test
            int consumed = 0;
            bool readSuccess = ComprehensiveTestPacket.TryRead(buffer.AsSpan(0, bytesWritten), ref consumed, out var deserialized);
            Assert.True(readSuccess);
            Assert.Equal(bytesWritten, consumed);
            Assert.Null(deserialized.ObjectArrayField); // Should be null (not empty array)
            Assert.Null(deserialized.DeepNestedArrayField); // Should be null (not empty array)
        }

        /// <summary>
        /// Helper method to parse keys from the serialized buffer
        /// </summary>
        private static ushort[] ParseKeysFromBuffer(byte[] buffer, int length)
        {
            var keys = new System.Collections.Generic.List<ushort>();
            var span = buffer.AsSpan(0, length);
            var offset = 0;

            while (offset < span.Length)
            {
                if (offset + 2 > span.Length) break;

                var key = BitConverter.ToUInt16(span.Slice(offset, 2));
                keys.Add(key);

                // Skip to next key (primitive types have no length prefix, so we need to determine size)
                // For now, just skip 2 bytes for the key and assume primitive types are fixed size
                offset += 2;

                // For primitive types, skip the value bytes
                if (key == 1) offset += 1; // Boolean
                else if (key == 2) offset += 1; // Byte
                else if (key == 3) offset += 1; // SByte
                else if (key == 4) offset += 2; // Int16
                else if (key == 5) offset += 2; // UInt16
                else if (key == 6) offset += 4; // Int32
                else if (key == 7) offset += 4; // UInt32
                else if (key == 8) offset += 8; // Int64
                else if (key == 9) offset += 8; // UInt64
                else if (key == 10) offset += 4; // Single
                else if (key == 11) offset += 8; // Double
                else if (key == 12) offset += 16; // Guid
                else if (key == 13) offset += 16; // GuidRfc4122
                else if (key == 14) offset += 2 + System.Text.Encoding.UTF8.GetByteCount("Hello, World!"); // String
                else if (key == 15) offset += 2 + System.Text.Encoding.UTF8.GetByteCount("Nested") + 4; // Nested object
                else if (key == 16) offset += 2 + System.Text.Encoding.UTF8.GetByteCount("Deep") + 4 + 2 + System.Text.Encoding.UTF8.GetByteCount("Deep Deep") + 4; // Deep nested
                else if (key == 17) offset += 2 + 3 * (2 + System.Text.Encoding.UTF8.GetByteCount("Array") + 4); // Object array
                else if (key == 18) offset += 2 + 2 * (2 + System.Text.Encoding.UTF8.GetByteCount("Deep Array") + 4 + 2 + System.Text.Encoding.UTF8.GetByteCount("Deep Deep Array") + 4); // Deep nested array
            }

            return keys.ToArray();
        }
    }
}
