using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;
using Scopes.C;
using Scopes;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace ComponentSg {

    static class Static {
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

    }
    

    [Microsoft.CodeAnalysis.Generator]
    public class ComponentSg : ISourceGenerator {


        INamedTypeSymbol EntityType;
        INamedTypeSymbol PageType;

        public void Execute(GeneratorExecutionContext context) {

            var allSymbols = context.Compilation.GetSymbolsWithName(_ => true);
            var typeSymbols = allSymbols.OfType<INamedTypeSymbol>();

            EntityType = context.Compilation.GetTypeByMetadataName("StaticSharp.Entity");// typeSymbols.First(x => (x.ContainingNamespace + "." + x.MetadataName) == "StaticSharp.Entity");
            PageType = context.Compilation.GetTypeByMetadataName("StaticSharp.Page"); //typeSymbols.First(x => (x.ContainingNamespace + "." + x.MetadataName) == "StaticSharp.Page");
            


            var partialTypes = typeSymbols.Where(x => {
                var declarationSyntax = x.DeclaringSyntaxReferences.First().GetSyntax();
                if (declarationSyntax is ClassDeclarationSyntax classDeclarationSyntax) {
                    return true;
                    /*if (classDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword))) {
                        return true;
                    }*/
                }
                return false;
            });


            var entityTypes = partialTypes.Where(x => x.InheritsFrom(EntityType) || x.Is(EntityType));
            
            //ProcessClass(EntityType, context);
            foreach (var i in entityTypes) {
                ProcessClass(i, context);
            }


        }

        IEnumerable<IPropertySymbol> GetWritableProperties(INamedTypeSymbol type) {
            var members = type.GetMembers().OfType<IPropertySymbol>();
            foreach (var m in members) {
                if (!m.IsReadOnly)
                    yield return m;
            }
        }

        IEnumerable<IPropertySymbol> GetAllWritablePropertiesOfInterface(INamedTypeSymbol type) {
            var members = type.GetMembers().OfType<IPropertySymbol>();//.Where(x=>x.Kind == SymbolKind.Property);

            foreach (var m in GetWritableProperties(type)) {
                yield return m;
            }

            var interfaces = type.AllInterfaces;
            foreach (var m in interfaces.SelectMany(x=> GetWritableProperties(x))) {
                yield return m;
            }
        }


        public void ProcessClass(INamedTypeSymbol type, GeneratorExecutionContext context) {

            var jType = context.Compilation
                .GetSymbolsWithName($"J{type.Name}")
                .Where(x=> x.ContainingNamespace.Is(type.ContainingNamespace))
                .FirstOrDefault();
            var body = new Group();

            


            var isPage = type.InheritsFrom(PageType) || type.Is(PageType);

            if (isPage) {
                var location = type.Locations.FirstOrDefault(x => x.IsInSource);
                if (location != null) {
                    var syntaxTree = location.SourceTree;
                    var root = syntaxTree.GetRoot();
                    var lineSpan = syntaxTree.GetLineSpan(location.SourceSpan);
                    var line = lineSpan.StartLinePosition.Line + 1;
                    var path = location.SourceTree.FilePath;

                    body.Add($"private static string SourceFilePath => @\"{path}\";");
                    body.Add($"private static int SourceLineNumber => {line};");
                }
                body.Add($"public {type.Name}():this(SourceLineNumber,SourceFilePath){{}}");
                body.Add($"protected {type.Name}(int callerLineNumber, string callerFilePath) : base(callerLineNumber, callerFilePath) {{}}");
            } else {


                body.Add(
                        type.IsAbstract
                        ? $"public {type.Name}(int callerLineNumber, string callerFilePath) : base(callerLineNumber, callerFilePath) {{}}"
                        : $"public {type.Name}([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = \"\") : base(callerLineNumber, callerFilePath) {{}}"
                        );
            }

            INamedTypeSymbol stringTypeSymbol = context.Compilation.GetTypeByMetadataName("System.String");
            INamedTypeSymbol intTypeSymbol = context.Compilation.GetTypeByMetadataName("System.Int32");
            var hasCopyConstructor = type.InstanceConstructors.Any(x => {
                var parameters = x.Parameters.ToArray();
                if (parameters.Length != 3) return false;
                if (!SymbolEqualityComparer.Default.Equals(parameters[0].Type, type)) return false;
                if (!SymbolEqualityComparer.Default.Equals(parameters[1].Type, intTypeSymbol)) return false;
                if (!SymbolEqualityComparer.Default.Equals(parameters[2].Type, stringTypeSymbol)) return false;
                return true;
            });

            if (!hasCopyConstructor)
                body.Add($"protected {type.Name}({type.Name} other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {{}}");


            //Assign
            if (jType != null) {
                //var new_ = type.Is(EntityType) ? "" : "new ";
                body.Add(new Scope($"public {type.Name} Assign(out Js.Variable<J{type.Name}> variable, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = \"\")") {
                    "variable = new(callerLineNumber, callerFilePath);",
                    "return Assign(variable);"
                });

                var current = type;
                var virtualOrOverride = type.IsSealed ? "": "virtual";
                while (true) {
                    body.Add(new Scope($"public {virtualOrOverride} {type.Name} Assign(Js.Variable<J{current.Name}> variable)") {
                        "if (VariableNames == null) VariableNames = new();",
                        "VariableNames.Add(variable.Name);",
                        "return this;"
                    });                    
                    if (current.Is(EntityType))
                        break;
                    virtualOrOverride = "override";
                    current = current.BaseType;
                }                
            }


            //Bindings
            if (jType != null) {
                var properties = GetAllWritablePropertiesOfInterface(jType as INamedTypeSymbol).ToArray();

                foreach (var p in properties) {

                    var setMethod = p.SetMethod;
                    if (setMethod.IsAbstract == false) {
                    }

                    string new_ = p.ContainingType.Is(jType) ? "" : "new ";

                    if (!setMethod.IsAbstract) {
                        var declarationSyntax = setMethod.DeclaringSyntaxReferences
                            .Select(r => r.GetSyntax())
                            .OfType<AccessorDeclarationSyntax>()
                            .FirstOrDefault();

                        body.Add(
                            new Scope($"public {new_}Binding<J{type.Name},{p.Type}> {p.Name}") {
                                new Scope("set"){
                                    declarationSyntax.Body.Statements.Select(x=>x.ToString())
                                }
                            }
                            );
                    } else {

                        body.Add(
                            new Scope($"public {new_}Binding<J{type.Name},{p.Type}> {p.Name}") {
                                new Scope("set"){
                                    $"Properties[\"{p.Name}\"] = value.CreateScriptExpression();"
                                }
                            }
                        );
                    }
                }
            }


            Group castExtension = null;
            if (jType != null) {
                castExtension = new Group {
                    "[Javascriptifier.JavascriptClass(\"\")]",
                    new Scope($"public static partial class {type.Name}Extension"){
                        "[Javascriptifier.JavascriptOnlyMember]",
                        $"[Javascriptifier.JavascriptMethodFormat(\"as({{0}},\\\"{type.Name}\\\")\")]",
                        $"public static J{type.Name} As{type.Name}(this JEntity _this) => throw new Javascriptifier.JavascriptOnlyException();"
                    }
                };
            }


            var result = new Group {
                "using System.Runtime.CompilerServices;",
                new Scope($"namespace {type.ContainingNamespace}") {
                    castExtension,
                    new Scope($"partial class {type.Name} /*Generated*/"){
                        body
                    }
                }
            };

            var fullName = type.Name;
            if (type.ContainingNamespace.Name != "") {
                fullName = $"{type.ContainingNamespace.Name}.{fullName}";
            }

            context.AddSource($"{fullName}.generated.cs", result.ToString());
        }


        public void Initialize(GeneratorInitializationContext context) {

        }
    }
}
