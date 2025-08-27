using System;
using System.Collections.Generic;

namespace Serializer.Generator.Templates
{
    public static class CompleteClassTemplate
    {
        public partial class ExampleType
        {
            public static class Keys
            {
                public const ushort Property1 = 1;
                public const ushort Property2 = 2;
                public const ushort Property3 = 3;
            }

            static ExampleType()
            {
                // Each packet has its own Keys class, no need for RnsKeyRegistry
            }

            public bool Write(Span<byte> buffer, out int bytesWritten)
            {
                bytesWritten = 0;
                var used = 0;
                try
                {
                    BooleanTemplate.Write(ref used, buffer, Property1, Keys.Property1);
                    Int32Template.Write(ref used, buffer, Property2, Keys.Property2);
                    StringTemplate.Write(ref used, buffer, Property3, Keys.Property3);

                    bytesWritten = used;
                    return true;
                }
                catch
                {
                    bytesWritten = 0;
                    return false;
                }
            }

            public static bool TryRead(ReadOnlySpan<byte> buffer, ref int consumed, out ExampleType readPacket)
            {
                readPacket = new ExampleType();
                try
                {
                    while (consumed < buffer.Length)
                    {
                        if (consumed + 2 > buffer.Length) break;
                        consumed += RndCodec.ReadUInt16(buffer.Slice(consumed), out ushort key);

                        switch (key)
                        {
                            case Keys.Property1:
                                BooleanTemplate.Read(ref consumed, buffer, out bool boolValue);
                                readPacket.Property1 = boolValue;
                                break;
                            case Keys.Property2:
                                Int32Template.Read(ref consumed, buffer, out int intValue);
                                readPacket.Property2 = intValue;
                                break;
                            case Keys.Property3:
                                StringTemplate.Read(ref consumed, buffer, out string stringValue);
                                readPacket.Property3 = stringValue;
                                break;
                            default:
                                break;
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public bool Property1 { get; set; }
            public int Property2 { get; set; }
            public string Property3 { get; set; } = "";
        }
    }
}
