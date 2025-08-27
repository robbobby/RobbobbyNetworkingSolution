using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serializer.Generator;

public static class Helpers
{
    public static string ExtractMethodBody<T>(Compilation compilation, string methodName)
    {
        if (compilation == null) return string.Empty;

        var templateType = compilation.GetTypeByMetadataName($"Serializer.Generator.Templates.{nameof(T)}");
        if (templateType == null) return string.Empty;

        var method = templateType.GetMembers(methodName).FirstOrDefault() as IMethodSymbol;
        if (method == null) return string.Empty;

        var syntaxReference = method.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxReference == null) return string.Empty;

        var syntaxNode = syntaxReference.GetSyntax();
        if (syntaxNode is not MethodDeclarationSyntax methodDeclaration) return string.Empty;

        var body = methodDeclaration.Body;
        return body?.ToString() ?? string.Empty;
    }
}
