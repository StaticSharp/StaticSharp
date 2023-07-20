using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

static class SymbolHelper {

    public static bool Is(this ISymbol thisType, ISymbol otherType) {
        return SymbolEqualityComparer.Default.Equals(thisType, otherType);
    }

    public static bool InheritsFrom(this INamedTypeSymbol thisType, INamedTypeSymbol otherType) {
        INamedTypeSymbol currentType = thisType;
        while (currentType != null) {
            if (currentType.BaseType != null && SymbolEqualityComparer.Default.Equals(currentType.BaseType, otherType)) {
                return true;
            }
            currentType = currentType.BaseType;
        }
        return false;
    }

    public static bool InheritsFrom(this INamedTypeSymbol thisType, string otherTypeFullName) {
        INamedTypeSymbol currentType = thisType;
        while (currentType != null) {
            if (currentType.BaseType != null && currentType.BaseType.GetFullyQualifiedNameNoGlobal() == otherTypeFullName) {
                return true;
            }
            currentType = currentType.BaseType;
        }
        return false;
    }

    public static bool IsPartial(this ITypeSymbol x) {
        var firstSyntaxReferences = x.DeclaringSyntaxReferences.FirstOrDefault();
        if (firstSyntaxReferences == null) return false;

        if (firstSyntaxReferences.GetSyntax() is TypeDeclarationSyntax declarationSyntax) {
            return declarationSyntax.IsPartial();
        }
        return false;
        //return x.DeclaringSyntaxReferences.Skip(1).Any();
    }

    public static string GetFullyQualifiedName(this ISymbol x) {
        var format = SymbolDisplayFormat.FullyQualifiedFormat;
        //format = format.WithMiscellaneousOptions(format.MiscellaneousOptions & ~SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        var resut = x.ToDisplayString(format);

        return resut;

    }

    public static string GetFullyQualifiedNameNoGlobal(this ISymbol x) {
        var format = SymbolDisplayFormat.FullyQualifiedFormat;
        format = format.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);
        var resut = x.ToDisplayString(format);

        return resut;

    }
}