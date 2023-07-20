using Microsoft.CodeAnalysis;

namespace RoutingSg.Src.Helpers {

    /// <summary>
    /// Identifies C# items having special meaning for StaticSharp. NOTE: some conventions are strings in <see cref="Kw"/>
    /// </summary>
    public static class StaticSharpConventions {

        public static string GetRootNamespaceFullName(Compilation compilation) =>
            $"{compilation.AssemblyName}.{Kw.Root}";

        public static ITypeSymbol GetLanguageEnum(Compilation compilation) =>
            compilation.GetTypeByMetadataName($"{GetRootNamespaceFullName(compilation)}.{Kw.Language}");

        public static bool IsPage(ISymbol s) =>
            s is INamedTypeSymbol &&
            ((INamedTypeSymbol)s).InheritsFrom(Kw.Page) &&
            ((INamedTypeSymbol)s).IsPartial() &&
            !((INamedTypeSymbol)s).IsStatic;

        /// <summary>
        /// Checks that symbol is "representative" (impying we already know that symbol is "page")
        /// </summary>
        /// <param name="pageSymbol">Page symbol</param>
        /// <returns></returns>
        public static bool IsPageRepresentative(ISymbol pageSymbol) =>
            !pageSymbol.IsAbstract;
    }
}
