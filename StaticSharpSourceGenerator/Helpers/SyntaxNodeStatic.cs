using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

static class SyntaxNodeStatic {
    public static List<T> GetParents<T>(this SyntaxNode x) {
        var node = x.Parent;
        var result = new List<T>();
        while (node != null) {
            if (node is T t) {
                result.Add(t);
            }
            node = node.Parent;
        }
        return result;
    }
    public static List<ClassDeclarationSyntax> GetParentClasses(this SyntaxNode x) {
        return x.GetParents<ClassDeclarationSyntax>();
    }

    public static List<NamespaceDeclarationSyntax> GetNamespaces(this SyntaxNode x) {
        return x.GetParents<NamespaceDeclarationSyntax>();
    }


    

}
