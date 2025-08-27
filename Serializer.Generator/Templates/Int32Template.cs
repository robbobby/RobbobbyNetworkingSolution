using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serializer.Generator.Templates
{
    public abstract class Int32Template
    {
        public static void Read(ref int consumed, ReadOnlySpan<byte> buffer, TestPacket testPacket)
        {
            consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var PROPERTY_VALUE);
            testPacket.PROPERTY_KEY = PROPERTY_VALUE;
        }

        public static void Write(ref int used, Span<byte> buffer, int value, ushort key)
        {
            if (!(value == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), value);
            }
        }

        public static string GenerateReadCode(string propertyName, Compilation compilation = null)
        {
            var methodBody = Helpers.ExtractMethodBody<Int32Template>(compilation, nameof(Read));
            return methodBody
                .Replace("PROPERTY_VALUE", $"{propertyName}Value")
                .Replace("PROPERTY_KEY", propertyName);
        }


        public static string GenerateWriteCode(string propertyName, Compilation compilation = null)
        {
            var methodBody = Helpers.ExtractMethodBody<Int32Template>(compilation, nameof(Write));
            return methodBody
                .Replace("value", propertyName)
                .Replace("key", $"Keys.{propertyName}");
        }

    }

    public class TestPacket
    {
        public int PROPERTY_KEY { get; set; }
    }
}
