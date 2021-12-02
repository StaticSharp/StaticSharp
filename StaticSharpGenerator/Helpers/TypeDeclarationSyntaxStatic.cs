using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

static class TypeDeclarationSyntaxStatic {
    public static bool IsPartial(this TypeDeclarationSyntax x) {
        foreach (var m in x.Modifiers) {
            if (m.Kind() == SyntaxKind.PartialKeyword) {
                return true;
            }
        }
        return false;
    }
}

