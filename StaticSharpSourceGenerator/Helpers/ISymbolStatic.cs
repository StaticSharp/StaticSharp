using Microsoft.CodeAnalysis;

static class ISymbolStatic {
    public static string GetFullyQualifiedName(this ISymbol x) {
        var format = SymbolDisplayFormat.FullyQualifiedFormat;
        //format = format.WithMiscellaneousOptions(format.MiscellaneousOptions & ~SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        var resut = x.ToDisplayString(format);

        return resut;

    }
}


static class IParameterSymbolStatic {
    public static string GetDeclaration(this IParameterSymbol x) {
        var text = x.DeclaringSyntaxReferences[0].GetSyntax().GetText();


        return text.ToString();
        //return x.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }
}