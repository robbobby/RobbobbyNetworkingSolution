using System;
using Microsoft.CodeAnalysis;

namespace Serializer.Generator.Templates.StructureTypes
{
    public class NamespaceTemplate
    {
        // Actual method with real code that gets extracted by Roslyn
        public static void NamespaceStart(string NAMESPACE_NAME)
        {
            // namespace NAMESPACE_NAME
            // {
        }

        public static void NamespaceEnd()
        {
            // }
        }

        // Template generation methods that work like primitive templates
        public static string GenerateNamespaceStart(string namespaceName, Compilation compilation = null)
        {
            var methodBody = Helpers.ExtractMethodBody<NamespaceTemplate>(compilation, nameof(NamespaceStart));
            return methodBody.Replace("NAMESPACE_NAME", namespaceName);
        }

        public static string GenerateNamespaceEnd(Compilation compilation = null)
        {
            var methodBody = Helpers.ExtractMethodBody<NamespaceTemplate>(compilation, nameof(NamespaceEnd));
            return methodBody;
        }
    }
}
