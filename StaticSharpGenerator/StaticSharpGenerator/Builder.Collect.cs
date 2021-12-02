using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpGenerator {
    partial class Builder {
        public NamespaceInfo Collect() {
            var result = new NamespaceInfo(null, null);
            foreach (var tree in Compilation.SyntaxTrees) {
                Collect(result, tree.GetRoot().ChildNodes());
            }
            return result;
        }

        public static void Collect(NamespaceInfo @namespace, IEnumerable<SyntaxNode> nodes) {
            foreach (var item in nodes) {
                if (item is NamespaceDeclarationSyntax namespaceDeclaration) {
                    var nameItems = namespaceDeclaration.Name.Split();
                    var tip = @namespace.AddNamespaceBranch(nameItems.Select(n => n.Identifier.ValueText));

                    Collect(tip, namespaceDeclaration.Members);
                    //namespaceDeclaration.Members.CollectNamespaces(tip);
                } else {
                    if (item is TypeDeclarationSyntax typeDeclaration) {
                        @namespace.AddType(typeDeclaration);

                    }
                }
            }
        }
        public static void Collect(TypeInfo type, IEnumerable<SyntaxNode> nodes) {
            foreach (var item in nodes) {
                if (item is TypeDeclarationSyntax typeDeclaration) {
                    type.AddType(typeDeclaration);
                }
            }
        }
    }
}