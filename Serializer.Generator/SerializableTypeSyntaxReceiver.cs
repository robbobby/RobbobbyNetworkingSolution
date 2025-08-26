using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Serializer.Generator
{
    /// <summary>
    /// Syntax receiver that scans for types with the [RnsSerializable] attribute
    /// </summary>
    public class SerializableTypeSyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// Gets the list of candidate types that might be serializable
        /// </summary>
        public Collection<TypeDeclarationSyntax> CandidateTypes { get; } = new Collection<TypeDeclarationSyntax>();

        /// <summary>
        /// Called for each syntax node in the compilation
        /// </summary>
        /// <param name="syntaxNode">The syntax node to examine</param>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Look for class and struct declarations
            if (syntaxNode is TypeDeclarationSyntax typeDeclaration)
            {
                // Check if the type has any attributes
                if (typeDeclaration.AttributeLists.Count > 0)
                {
                    // Check if any attribute looks like [RnsSerializable]
                    foreach (var attributeList in typeDeclaration.AttributeLists)
                    {
                        foreach (var attribute in attributeList.Attributes)
                        {
                            // Simple name-based check - the semantic analysis will do the real work
                            if (attribute.Name.ToString().Contains("RnsSerializable"))
                            {
                                CandidateTypes.Add(typeDeclaration);
                                return; // Found one, no need to check more attributes
                            }
                        }
                    }
                }
            }
        }
    }
}
