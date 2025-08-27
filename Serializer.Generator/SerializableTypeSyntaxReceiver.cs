using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.ObjectModel;
using System.Linq;

namespace Serializer.Generator
{
    /// <summary>
    /// Syntax receiver that scans for partial types that might implement IRnsPacket&lt;TEnum&gt; or IRnsPacketField
    /// </summary>
    public class SerializableTypeSyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// Gets the list of candidate types that are partial and might be serializable
        /// </summary>
        public Collection<TypeDeclarationSyntax> CandidateTypes { get; } = new Collection<TypeDeclarationSyntax>();

        /// <summary>
        /// Called for each syntax node in the compilation
        /// </summary>
        /// <param name="syntaxNode">The syntax node to examine</param>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Look for partial class and struct declarations
            if (syntaxNode is TypeDeclarationSyntax typeDeclaration &&
                typeDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
            {
                // Check if the type has a base list (implements interfaces)
                if (typeDeclaration.BaseList != null)
                {
                    // Look for types that might implement IRnsPacket<T> or IRnsPacketField
                    foreach (var baseType in typeDeclaration.BaseList.Types)
                    {
                        var baseTypeName = baseType.Type.ToString();
                        if (baseTypeName.Contains("IRnsPacket") || baseTypeName.Contains("IRnsPacketField"))
                        {
                            CandidateTypes.Add(typeDeclaration);
                            return; // Found one, no need to check more
                        }
                    }
                }
            }
        }
    }
}
