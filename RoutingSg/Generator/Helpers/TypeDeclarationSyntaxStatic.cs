using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

static class TypeDeclarationSyntaxStatic {
    public static bool IsPartial(this TypeDeclarationSyntax x) {
        foreach (var m in x.Modifiers) {
            if (m.IsKind(SyntaxKind.PartialKeyword)) {
                return true;
            }
        }
        return false;
    }
}

