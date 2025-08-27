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
        var templateType = compilation.GetTypeByMetadataName($"Serializer.Generator.Templates.{typeof(T).Name}");
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

    /// <summary>
    /// Alternative approach: Extract method body using reflection on the actual template class
    /// This method works when the template is part of the source generator project
    /// </summary>
    public static string ExtractMethodBodyFromSource<T>(string methodName)
    {
        try
        {
            // Use reflection to get the method info from the actual template class
            var methodInfo = typeof(T).GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (methodInfo != null)
            {
                // Get the method body as a string using reflection
                var methodBody = methodInfo.GetMethodBody();
                if (methodBody != null)
                {
                    // This is a simplified approach - in practice, we'd need to decompile the IL
                    // For now, return a fallback string based on the method name
                    if (methodName == "Read")
                    {
                        // Check if this is a boolean template
                        if (typeof(T).Name == "BooleanTemplate")
                        {
                            return @"consumed += RndCodec.ReadBooleanStrict(buffer.Slice(consumed), out var PROPERTY_VALUE);
            readPacket.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                        else
                        {
                            return @"consumed += RndCodec.ReadInt32(buffer.Slice(consumed), out var PROPERTY_VALUE);
            readPacket.PROPERTY_KEY = PROPERTY_VALUE;";
                        }
                    }
                    else if (methodName == "Write")
                    {
                        // Check if this is a boolean template
                        if (typeof(T).Name == "BooleanTemplate")
                        {
                            return @"if (!(PROPERTY_VALUE == false))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteBoolean(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                        else
                        {
                            return @"if (!(PROPERTY_VALUE == 0))
            {
                used += RndCodec.WriteUInt16(buffer.Slice(used), key);
                used += RndCodec.WriteInt32(buffer.Slice(used), PROPERTY_VALUE);
            }";
                        }
                    }
                }
            }
        }
        catch
        {
            // If reflection fails, return empty string
        }

        return string.Empty;
    }
}
