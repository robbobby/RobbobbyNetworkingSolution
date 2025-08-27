using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serializer.Generator;

public static class Helpers
{
    /// <summary>
    /// Extracts method body from a template class using Roslyn analysis
    /// This approach works with source generators by analyzing the syntax directly
    /// </summary>
    public static string ExtractMethodBody<T>(Compilation compilation, string methodName)
    {
        if (compilation == null)
        {
            return string.Empty;
        }

        // Get the type symbol for the template class
        // Try different namespace patterns for templates
        var templateType = compilation.GetTypeByMetadataName($"Serializer.Generator.Templates.{typeof(T).Name}");
        if (templateType == null)
        {
            // Try StructureTypes namespace
            templateType = compilation.GetTypeByMetadataName($"Serializer.Generator.Templates.StructureTypes.{typeof(T).Name}");
        }
        if (templateType == null)
        {
            // Try PrimitiveTypes namespace
            templateType = compilation.GetTypeByMetadataName($"Serializer.Generator.Templates.PrimitiveTypes.{typeof(T).Name}");
        }
        if (templateType == null)
        {
            // Try ComplexTypes namespace
            templateType = compilation.GetTypeByMetadataName($"Serializer.Generator.Templates.ComplexTypes.{typeof(T).Name}");
        }
        if (templateType == null)
        {
            return string.Empty;
        }

        // Find the method by name
        var method = templateType.GetMembers(methodName).FirstOrDefault() as IMethodSymbol;
        if (method == null)
        {
            return string.Empty;
        }

        // Get the syntax reference and extract the method body
        var syntaxReference = method.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxReference == null)
        {
            return string.Empty;
        }

        var syntaxNode = syntaxReference.GetSyntax();
        if (syntaxNode is not MethodDeclarationSyntax methodDeclaration)
        {
            return string.Empty;
        }

        var body = methodDeclaration.Body;
        return body?.ToString() ?? string.Empty;
    }
}
