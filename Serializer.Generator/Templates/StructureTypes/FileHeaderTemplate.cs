using System;
using Microsoft.CodeAnalysis;

namespace Serializer.Generator.Templates.StructureTypes
{
    public class FileHeaderTemplate
    {
        // Actual method with real code that gets extracted by Roslyn
        public static void FileHeader()
        {
            // Generated serialization code
            // #nullable enable
            // using System;
            // using System.Collections.Generic;
            // using Serializer.Runtime;
        }

        // Template generation method that works like primitive templates
        public static string GenerateFileHeader(Compilation compilation = null)
        {
            // Use Roslyn analysis to extract the method body
            var methodBody = Helpers.ExtractMethodBody<FileHeaderTemplate>(compilation, nameof(FileHeader));
            return methodBody;
        }
    }
}
