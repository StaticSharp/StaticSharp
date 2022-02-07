using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpGenerator {
    partial class Builder {


        void WriteClassTree(string className, NamespaceInfo namespaceInfo, BlockWriter place, string parentClassName, IEnumerable<string> path) {
            var nodeClass = BeginNodeClass(className);// {ITypedRepresentativeProvider}<{}>
            var body = nodeClass.Content;

            if(parentClassName == null) {
                body.AddLine($"public override {ProtoNode} Parent => null;");
            } else {
                body.AddLine($"public override {ProtoNode} Parent => new {parentClassName}({State.ToCall()});");
            }
            body.AddLine($"public override {StaticSharpRoot} Root => new {StaticSharpRoot}({State.ToCall()});");

            body.AddLine($"public override string[] Path => new string[]{{{string.Join(",", path.Select(x => $"\"{x}\""))}}};");

            body.AddLine($"public override string Name => \"{namespaceInfo.Name}\";");                        

            var commonRepresentativeType = WriteRepresentatives(namespaceInfo, body);

            nodeClass.Header += commonRepresentativeType == null ? "" : $", {ITypedRepresentativeProvider}<{commonRepresentativeType}>";

            WriteChildren(namespaceInfo, body);

            foreach(var n in namespaceInfo.Namespaces) {
                WriteAlphaProperty(body, n.Value.Name);
                WriteClassTree(α + n.Value.Name, n.Value, body, className, path.Append(n.Value.Name));
            }

            place.AddLine(nodeClass);
        }






        HeaderBracesWriter BeginNodeClass(string name) {
            var result = new HeaderBracesWriter($"public class {name} : {ProtoNode}");
            var body = result.Content;

            var constructorBody = body.AddLine(new HeaderBracesWriter($"public {name}({State.ToConstructorParameters()}) : base({State.ToBaseCall()})"))
                .Content;

            for(int i = 0; i < State.Length; i++) {
                var methodBody = body.AddLine(new HeaderBracesWriter($"public override {name} With{State[i].Name}({State[i].TypeName} {State[i].ParameterName})")).Content;
                var parametersWithReplacement = State.Select(x => x.Name).ToArray();
                parametersWithReplacement[i] = State[i].ParameterName;

                methodBody.AddLine($"return new {name}({string.Join(",", parametersWithReplacement)});");
            }


            return result;
        }

        void WriteAlphaProperty(BlockWriter place, string name) {
            place.AddLine($"public virtual {α}{name} {name} => new({State.ToCall()});");
        }

        string WriteRepresentatives(NamespaceInfo namespaceInfo, BlockWriter place) {

            var representatives = namespaceInfo.Types
                .Select(x => x.Value)
                .Where(x => x.Attributes.Any(a => a.Name.ToString() == Representative));
            var numRepresentatives = representatives.Count();

            if(numRepresentatives == 0) {
                //place.AddLine($"{IRepresentative} {INode}.{Representative} => null;");
                place.AddLine($"public override {IRepresentative} {Representative} => null;");
            } else {
                var symbols = representatives.Select(x => Compilation.GetTypeByMetadataName(x.FullyQualifiedName));

                foreach(var s in symbols) {
                    if(s.IsAbstract) {
                        Log.AbstractRepresentative(s.DeclaringSyntaxReferences.First().GetSyntax().GetLocation());
                        return null;
                    }
                }

                if(symbols.Any(x => x.IsAbstract)) {



                    return null;
                }
                if(symbols.Any(x => x.IsStatic)) {
                    //error
                    return null;
                }
                if(symbols.Any(x => x.TypeKind != TypeKind.Class)) {
                    //error
                    return null;
                }
                var first = symbols.First();


                //place.AddLine($"{IRepresentative} {INode}.{Representative} => {Representative} as {IRepresentative};");

                var baseTypeFullName = first.BaseType.GetFullyQualifiedName();

                ///TODO: check if baseTypeFullName==object
                ///Error CS0553  'StaticSharpRoot.αIndex.αComponents.implicit operator object(StaticSharpRoot.αIndex.αComponents)': user - defined conversions to or from a base type are not allowed  StaticSharpDemo D:\StaticSharp\StaticSharpDemo\StaticSharpGenerator\StaticSharpGenerator.StaticSharpGenerator\classTree.cs  100 Active

                place.AddLine($"public static implicit operator {baseTypeFullName}({α}{namespaceInfo.Name} {α}) => {α}.Representative;");

                //place.AddLine($"{IRepresentative} {INode}.{Representative} => {Representative};");

                if(numRepresentatives == 1) {
                    place.AddLine($"public override {baseTypeFullName} {Representative} => new {first.GetFullyQualifiedName()}({State.ToCall()});");

                } else {

                    var baseType = first.BaseType;
                    var sameBaseTypes = symbols.Skip(1).All(x => SymbolEqualityComparer.Default.Equals(x.BaseType, baseType));
                    if(!sameBaseTypes) {
                        //error
                        return null;
                    }

                    place.AddLine($"public override {baseTypeFullName} {Representative} => SelectRepresentative({Representatives});");

                    var prop = place.AddLine(new HeaderBracesWriter($"public IEnumerable<{baseTypeFullName}> Representatives")).Content;
                    var get = prop.AddLine(new HeaderBracesWriter($"get")).Content;
                    foreach(var s in symbols) {
                        get.AddLine($"yield return new {s.GetFullyQualifiedName()}({State.ToCall()});");
                    }

                }
                return baseTypeFullName;
            }
            return null;
        }

        void WriteChildren(NamespaceInfo namespaceInfo, BlockWriter classTree) {

            var names = namespaceInfo.Namespaces.Select(x => x.Key).ToArray();
            var property = $"public override IEnumerable<{ProtoNode}> Children";
            if(names.Length == 0) {
                classTree.AddLine(property + $" => Enumerable.Empty<{ProtoNode}>();");
            } else {
                var getBody = classTree.AddLine(new HeaderBracesWriter(property))
                    .Content.AddLine(new HeaderBracesWriter($"get"))
                    .Content;

                foreach(var i in names) {
                    getBody.AddLine($"yield return {i};");
                }
            }
        }
    }
}