using Microsoft.CodeAnalysis;
using RoutingSg.Src.Helpers;
using Scopes.C;
using System.Collections.Generic;
using System.Linq;

namespace RoutingSg.Src {
    static class PartialsWriter {

        static StateParameter[] _state;
        static string _rootNamespace;

        public static string Write(PagesTreeNode node, StateParameter[] state) {
            _rootNamespace = node.Symbol.GetFullyQualifiedNameNoGlobal();
            _state = state;

            return new Scope($"namespace {node.Symbol.ContainingNamespace.GetFullyQualifiedNameNoGlobal()}") {
                WriteNamespace(node, new[] { node.Symbol.Name})
            }.ToString();
        }

        static Scope WriteNamespace(PagesTreeNode node, IEnumerable<string> path) {
            // TODO: if(!node.IsNamespace) {new Scope($"{<access modifiers>} class {namespaceName}")} ...
            // Temporary workaround, classes-containers ignored
            if (!node.IsNamespace) return null;

            var result = new Scope($"namespace {node.Symbol.Name}") {
                node.Children.Where(c => c.IsPage).Select(c => WritePagePartial(c.Symbol as INamedTypeSymbol, path)),
                node.Children.Where(c => !c.IsPage).Select(c => WriteNamespace(c, path.Append(c.Symbol.Name)))
            };

            return result;
        }

        static Scope WritePagePartial(INamedTypeSymbol pageSymbol, IEnumerable<string> path) {
            var accessibility = "public"; //typeSymbol.DeclaredAccessibility.ToCSharpName();
            var nodeClassName = string.Join(".", path.Select(x => Kw.α + x));
            var baseClassFullName = pageSymbol.BaseType.ContainingNamespace.GetFullyQualifiedNameNoGlobal();
            var classInTree = baseClassFullName.StartsWith(_rootNamespace);
            var baseClassInfected = classInTree & pageSymbol.BaseType.IsPartial();

            var result = new Scope($"{accessibility} partial class {pageSymbol.Name} : {Kw.IRepresentative}") {
                $"public override {Kw.Protonode} {Kw.VirtualNodePropertyName} => {Kw.NodePropertyName};",
                $"{nodeClassName} {Kw.NodePropertyName} => new({_state.ToCall()});",
                $"{Kw.Node} {Kw.IRepresentative}.{Kw.NodePropertyName} => {Kw.NodePropertyName};" };

            if (baseClassInfected) {
                result.Add($"public {pageSymbol.Name}({_state.ToConstructorParameters()}) : base({_state.ToBaseCall()}) {{}}");
            } else {
                result.Add(_state.Select(p => $"public {p.TypeName} {p.Name}{{get; init;}}"));
                result.Add(new Scope($"public {pageSymbol.Name}({_state.ToConstructorParameters()})") {
                    _state.Select(p => $"{p.Name} = {p.ParameterName};")
                });
            }

            return result;
        }
    }
}
