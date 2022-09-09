using Exo.CSharpSyntaxTreeInspector;
using Exo.RoslynSourceGeneratorDebuggable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scopes.C;
using System;
using System.Collections.Generic;

//using SourceGeneratorHelpers;


using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MixinGenerator {

    static class RefKindStatic {
        public static string ToParameterPrefix(this RefKind kind) {
            switch (kind) {
                case RefKind.Out: return "out ";
                case RefKind.Ref: return "ref ";
                case RefKind.In: return "in ";
                case RefKind.None: return string.Empty;
                default: return string.Empty;
            }
        }
    }

    [Microsoft.CodeAnalysis.Generator]
    public class MixinGenerator : SourceGeneratorDebuggable {


        private static readonly SymbolDisplayFormat SymbolDisplayFormat = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters
                );

        /*
           Use this, if it is not possible to use an entrypoint from nuget */

#if SOURCE_GENERATOR_EXECUTABLE_MODE
        public static async Task Main(string[] args) {
            await Exo.RoslynSourceGeneratorDebuggable.Launcher.Main();        
        }
#endif

        public override void Initialize(IGeneratorInitializationContext context) {

        }


        public override void Execute(IGeneratorExecutionContext context) {

            var assemblyInfo = new AssemblyInfo(context.Compilation);


            var result = new Scopes.Group() {
            "#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required"

            };


            /*IEnumerable<AttributeSyntax> FilterAttributes(IEnumerable<AttributeSyntax> attributes) {
                attributes = attributes.Where(x => x.Name.GetLastName().Identifier.ValueText == "Mix");
                attributes = attributes.Where(x => x.Name is GenericNameSyntax);
                return attributes;
            }*/




            var types = assemblyInfo.GlobalNamespace.GetTypesRecursively().ToArray();

            var partialClasses = types.Where(x => x.IsPartial && (x.IsClass /*|| */));


            foreach (var type in partialClasses) {

                /*var mixAttributes = FilterAttributes(type.Attributes);

                if (!mixAttributes.Any())
                    continue;*/

                SemanticModel aggregateTypeSemanticModel = context.Compilation.GetSemanticModel(type.Parts.First().SyntaxTree);

                var aggregateTypeSymbol = aggregateTypeSemanticModel.GetDeclaredSymbol(type.Parts.First());

                var attributes = aggregateTypeSymbol.GetAttributes();

                var members = aggregateTypeSymbol.GetMembers();

                foreach (var attribute in attributes) {

                    //var fillName = typeof(MixAttribute).FullName;
                    //var mixAttributeMetadata = context.Compilation.GetTypeByMetadataName(fillName);

                    


                    var attributeClass = attribute.AttributeClass;
                    if (attributeClass.ConstructedFrom.ToString() != "MixAttribute") {
                        continue;
                    }

                    var arguments = attribute.ConstructorArguments;
                    if (arguments[0].Value is INamedTypeSymbol mixinTypeUsage) {
                        
                        result.Add(Mix(aggregateTypeSymbol, mixinTypeUsage));
                    }

                    //var mixinTypeUsage = attributeClass.TypeArguments.First() as INamedTypeSymbol;


                    

                }

            }

            context.AddSource("Generated.cs", result.ToString());

            /*var outputPath = Path.Combine(outputDirectory, "Generated.cs");
            File.WriteAllText(outputPath, result.ToString());*/
            

        }


        public void GetAllPublicMembers(INamedTypeSymbol type, Dictionary<string, ISymbol> result) {
            if (type.BaseType != null) {
                var name = type.BaseType.ToDisplayString(SymbolDisplayFormat);
                if (name != typeof(Object).FullName) {
                    GetAllPublicMembers(type.BaseType, result);
                }
                
            }


            foreach (var member in type.GetMembers()
                .Where(x => !x.IsStatic)
                .Where(x => x.DeclaredAccessibility == Accessibility.Public)
                ) {
                var name = member.Name;
                if (member is IMethodSymbol methodSymbol) {//TODO: Test me

                    var genericParametersCount = methodSymbol.TypeParameters.Length;
                    var parametersTypes = string.Join(",", methodSymbol.Parameters.Select(x => x.Type.ToDisplayString(SymbolDisplayFormat)));
                    name = $"{name}<{genericParametersCount}>({parametersTypes})";
                }
                result[name] = member;
            }
        }


        public Scope Mix(INamedTypeSymbol aggregateType, INamedTypeSymbol mixinType/*, Dictionary<string, INamedTypeSymbol> mixinSpecializationMap*/) {
            
             

            var keyword = aggregateType.TypeKind.ToString().ToLower();

            //TODO: think more about private variable naming
            var mixinVariableName = mixinType.ToDisplayString(SymbolDisplayFormat).Replace('.','_').Replace('<', '_').Replace('>', '_');


            

            var result = new Scope($"partial {keyword} {aggregateType.Name}") {
                $"private {mixinType.ToDisplayString(SymbolDisplayFormat)} {mixinVariableName} = new {mixinType.ToDisplayString(SymbolDisplayFormat)}();"
            };


            var publicInstanceMembers = mixinType.GetMembers()
                .Where(x => !x.IsStatic)
                .Where(x => x.DeclaredAccessibility == Accessibility.Public).ToArray();



            Scopes.Group SetAggrigator(object inner) {
                return new Scopes.Group() {
                    "var previousAggregator = Aggregator.Current;",
                    new Scope("try"){
                        "Aggregator.Current = this;",
                        inner
                    },
                    new Scope("finally"){
                        "Aggregator.Current = previousAggregator;"
                    }
                };
            }

            var members = new Dictionary<string, ISymbol>();
            GetAllPublicMembers(mixinType, members);




            foreach (var member in members.Values) {
                var name = member.Name;



                if (member is IFieldSymbol fieldSymbol) {
                    result.Add(
                        new Scope($"new public {fieldSymbol.Type.ToDisplayString(SymbolDisplayFormat)} {name}") { 
                            new Scope("get"){ 
                                $"return {mixinVariableName}.{name};" 
                            },
                            new Scope("set"){
                                $"{mixinVariableName}.{name} = value;"
                            }                    
                        }
                    );
                    continue;
                }

                if (member is IPropertySymbol propertySymbol) {
                    var property = new Scope($"new public {propertySymbol.Type.ToDisplayString(SymbolDisplayFormat)} {name}");
                    result.Add(property);

                    if (propertySymbol.GetMethod != null) {
                        if (propertySymbol.GetMethod.DeclaredAccessibility == Accessibility.Public) {
                            property.Add(new Scope("get") {
                                SetAggrigator($"return {mixinVariableName}.{name};")
                            });
                        }
                        
                    }

                    if (propertySymbol.SetMethod != null) {
                        if (propertySymbol.SetMethod.DeclaredAccessibility == Accessibility.Public) {
                            property.Add(new Scope("set") {
                                SetAggrigator($"{mixinVariableName}.{name} = value;")
                            });
                        }
                    }
                    continue;
                }

                if (member is IMethodSymbol methodSymbol) {
                    if (methodSymbol.MethodKind == MethodKind.Ordinary) {
                        var parametersCall = new List<string>();
                        var parametersDeclaration = new List<string>();

                        var genericFragment = methodSymbol.TypeParameters.Any()
                            ? $"<{string.Join(",", methodSymbol.TypeParameters.Select(x => x.Name))}>"
                            : "";


                        var constraintClauses = new List<string>();

                        foreach (var t in methodSymbol.OriginalDefinition.DeclaringSyntaxReferences) {
                            var syntax = t.GetSyntax();
                            if (syntax is MethodDeclarationSyntax methodDeclarationSyntax) {
                                foreach (var c in methodDeclarationSyntax.ConstraintClauses) { 
                                    var normalized = c.NormalizeWhitespace();
                                    var text = normalized.ToString();
                                    constraintClauses.Add(" "+text);
                                }                                
                             }
                        }
                        var constraintClausesFragment = string.Concat(constraintClauses);


                        foreach (var p in methodSymbol.Parameters) {
                            var refKinds = p.RefKind.ToParameterPrefix();
                            parametersCall.Add(refKinds + p.Name);
                            parametersDeclaration.Add(refKinds + p.Type.ToDisplayString(SymbolDisplayFormat)+" "+p.Name);
                        }
                        var callParemeterList = string.Join(", ", parametersDeclaration);

                        var resultTypeString = methodSymbol.ReturnsVoid ? "void" : methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat);

                        result.Add(new Scope($"new public {resultTypeString} {methodSymbol.Name}{genericFragment}({callParemeterList}){constraintClausesFragment}") {
                            SetAggrigator((methodSymbol.ReturnsVoid?"":"return ")+$"{mixinVariableName}.{methodSymbol.Name}{genericFragment}({string.Join(",", parametersCall)});")
                        });

                    }
                    continue;                                
                }


            }


            var namespaceName = aggregateType.ContainingNamespace.ToDisplayString(SymbolDisplayFormat);
            
            if (!string.IsNullOrEmpty(namespaceName)) {
                result = new Scope($"namespace {namespaceName}") {
                    result
                };
            }



            return result;

            /*

            foreach (var member in mixinTypeInfo.GetMembers()) {

                if (!member.IsPrivate() && !member.IsStatic()) {

                    if (member is FieldDeclarationSyntax fieldDeclarationSyntax) {
                        var declaration = fieldDeclarationSyntax.Declaration;

                        //Console.WriteLine(declaration.Type.GetType().FullName);

                        var fieldTypeInfo = AssemblyInfo.FindType(declaration.Type);

                        foreach (var variable in declaration.Variables) {
                            result.Add($"{member.GetVisibility()} {variable.Identifier.ValueText} ");

                        }
                        continue;
                    }



                }

            }



            var namespacePath = aggregateTypeInfo.Parent.PathString;
            if (!string.IsNullOrEmpty(namespacePath)) {

                result = new Scope($"namespace {aggregateTypeInfo.Parent.PathString}") {
                    result
                };
            }
                
                
            return result;*/
        }


    }
}