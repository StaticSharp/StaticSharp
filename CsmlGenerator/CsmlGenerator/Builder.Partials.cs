using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace CsmlGenerator {
    partial class Builder {


        void WriteStateProperties(BlockWriter place) {
            foreach(var i in State) {
                place.AddLine($"public {i.TypeName} {i.Name}{{get; init;}}");
            }
        }


        void WritePartial(TypeInfo typeInfo, BlockWriter partials, IEnumerable<string> path) {
            /*var Import = "Import";
            var ImportParents = "ImportParents";
            var ImportChildren = "ImportChildren";
            string[] AttributeNames = {
                Import,
                ImportParents,
                ImportChildren
            };*/

            /*if (typeInfo.Keyword != "record") {
                //error
                return;
            }*/


            if(!typeInfo.IsPartial) return;
            var attributes = typeInfo.Attributes;

            /*if (!attributes.Any()) return;

            var hasAttributes = attributes.Any(x => x.Name.ToString() == Representative);
            if (!hasAttributes) return;*/



            var typeSymbol = Compilation.GetTypeByMetadataName(typeInfo.FullyQualifiedName);


            var accessibility = "public"; //typeSymbol.DeclaredAccessibility.ToCSharpName();

            var nodeClassName = string.Join(".", path.Select(x => α + x).Prepend(CsmlRoot));
  

            var classBody = partials.AddLine(new HeaderBracesWriter($"{accessibility} partial class {typeInfo.Name} : {IRepresentative}"))
                    .Content;




            var baseClassFullName = typeSymbol.BaseType.ContainingNamespace.GetFullyQualifiedName();
            var classInTree = baseClassFullName.StartsWith(RootNamespaceName);
            var baseClassInfected = classInTree & typeSymbol.BaseType.IsPartial();


            //classBody.AddLine($"//typeSymbol.BaseType.IsPartial(): {typeSymbol.BaseType.IsPartial()}");

            {
                var virtualOrOverride = baseClassInfected ? "override" : "virtual";
                classBody.AddLine($"protected {virtualOrOverride} {ProtoNode} {VirtualNodePropertyName} => {NodePropertyName};");
            }
            classBody.AddLine($"{nodeClassName} {NodePropertyName} => new({State.ToCall()});");
            classBody.AddLine($"{INode} {IRepresentative}.{NodePropertyName} => {NodePropertyName};");




            if(baseClassInfected) {
                var constructorBody = classBody.AddLine(new HeaderBracesWriter($"public {typeInfo.Name}({State.ToConstructorParameters()}) : base({State.ToBaseCall()})"))
                    .Content;

            } else {
                WriteStateProperties(classBody);
                var constructorBody = classBody.AddLine(new HeaderBracesWriter($"public {typeInfo.Name}({State.ToConstructorParameters()})"))
                    .Content;
                foreach(var i in State) {
                    constructorBody.AddLine($"{i.Name} = {i.ParameterName};");
                }
            }


            //classBody.AddLine(new HeaderBracesWriter($"public {typeInfo.Name}({nodeClassName} {NodeParameterName})"))
            //        .Content.AddLine($"{NodePropertyName} = {NodeParameterName};");




            var attributesSymbols = typeSymbol.GetAttributes();//.Where(x => AttributeNames.Contains(x.AttributeClass.Name));

            /*var import = attributesSymbols.Where(x => x.AttributeClass.Name == Import);
            foreach (var i in import) {
                var arguments = i.ConstructorArguments;
                if (arguments.Length != 1) {
                    //error
                    continue;
                }
                var typeArgument = arguments[0];
                if (typeArgument.Value is INamedTypeSymbol typeSymbol) {
                    var fullName = typeSymbol.GetFullyQualifiedName();
                    var name = typeSymbol.Name;

                    content.AddLine($"protected virtual {fullName} {name} => new {fullName}();");
                }
            }


            var importParents = attributesSymbols.FirstOrDefault(x => x.AttributeClass.Name == ImportParents);
            if (importParents != null) {
                var className = CsmlRoot;
                foreach (var i in path) {
                    className = className + "." + α + i;
                    content.AddLine($"protected virtual {className} {i} => new {className}();");
                }
            }

            var importChildren = attributesSymbols.FirstOrDefault(x => x.AttributeClass.Name == ImportChildren);
            if (importChildren != null) {
                if (typeInfo.Parent is NamespaceInfo namespaceInfo) {
                    var className = $"{CsmlRoot}.{string.Concat(path.Select(x => α + x + '.'))}";


                    foreach (var i in namespaceInfo.Namespaces) {
                        //className = className + "." + α + i;
                        content.AddLine($"protected virtual {className}{α}{i.Key} {i.Key} => new {className}{α}{i.Key}();");
                    }
                }
            }*/
        }

        void WritePartials(NamespaceInfo namespaceInfo, BlockWriter partials, IEnumerable<string> path) {



            var contentCandidate = new BlockWriter();
            //partials = partials.AddLine(new HeaderBracesWriter($"namespace {namespaceInfo.Name}")).Content;
            foreach(var t in namespaceInfo.Types.Where(x => x.Key != "ProtoNode")) {
                WritePartial(t.Value, contentCandidate, path);
            }

            foreach(var i in namespaceInfo.Namespaces) {
                WritePartials(i.Value, contentCandidate, path.Append(i.Key));
            }
            var namespaceName = path.Any() ? namespaceInfo.Name : namespaceInfo.FullName;

            if(!contentCandidate.IsEmpty) {
                partials.AddLine(new HeaderBracesWriter($"namespace {namespaceName}"))
                    .Content.AddLine(contentCandidate);
            }

        }
    }
}