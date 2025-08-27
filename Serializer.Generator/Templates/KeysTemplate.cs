namespace Serializer.Generator.Templates
{
    public static class KeysTemplate
    {
        public const string KeysClass = @"        public static class Keys
        {{
{0}
        }}

";

        public const string KeyConstant = "            public const ushort {0} = {1};";
    }
}
