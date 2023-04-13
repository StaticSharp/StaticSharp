using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;

namespace StaticSharpComponentSourceGenerator {


    

    [Microsoft.CodeAnalysis.Generator]
    public class StaticSharpComponentSourceGenerator : ISourceGenerator {

        static bool IsInheritFrom(INamedTypeSymbol type, INamedTypeSymbol baseType) {
            INamedTypeSymbol currentType = type;
            while (currentType != null) {
                if (currentType.BaseType != null && SymbolEqualityComparer.Default.Equals(currentType.BaseType, baseType)) {
                    return true;
                }
                currentType = currentType.BaseType;
            }
            return false;
        }


        public void Execute(GeneratorExecutionContext context) {

            var allSymbols = context.Compilation.GetSymbolsWithName(_ => true);
            var typeSymbols = allSymbols.OfType<INamedTypeSymbol>();

            var entityType = typeSymbols.First(x => (x.ContainingNamespace + "." + x.MetadataName) == "StaticSharp.Gears.Entity");
            
            var partialTypes = typeSymbols.Where(x=>
               ((TypeDeclarationSyntax)x.DeclaringSyntaxReferences.First().GetSyntax())
               .Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword))
               );

            var entityTypes = partialTypes.Where(x => IsInheritFrom(x, entityType));


            foreach (var i in entityTypes) {
                ProcessClass(i, context);
            }



                context.AddSource("Generated.cs", $@"
public static class HelloClass
{{
    public static string HelloString = ""Hello from {nameof(StaticSharpComponentSourceGenerator)}!"";
}}
");
        }

        public void ProcessClass(INamedTypeSymbol type, GeneratorExecutionContext context) { 
            
        }


        public void Initialize(GeneratorInitializationContext context) {

        }
    }
}
