using Microsoft.CodeAnalysis;
using RoutingSg.Src.Helpers;
using Scopes;
using Scopes.C;
using System.Linq;

namespace RoutingSg.Src {
    public static class ClassTreeWriter {

        static StateParameter[] _state;
        static string _rootNamespace;

        public static string Write(PagesTreeNode pageNode, StateParameter[] state) {
            _rootNamespace = pageNode.Symbol.GetFullyQualifiedNameNoGlobal();
            _state = state;

            var result = new Group {
                "using System.Collections.Generic;",
                "using System.Linq;",
                "using System;",
                "#pragma warning disable IDE1006 // Naming Styles",
                $"namespace {_rootNamespace};",
                WriteClassTreeNode(pageNode, Kw.Protonode)
            };

            return result.ToString();
        }

        static Scope WriteClassTreeNode(PagesTreeNode node, string parentClassName /*TODO: likely redundant*/) {
            var className = $"{Kw.α}{node.Symbol.Name}";
            var path = node.Symbol.GetFullyQualifiedNameNoGlobal().Substring(_rootNamespace.Length).Split('.');

            var nodeClass = new Scope($"public class {className} : {Kw.Protonode}") {
                new Scope(
                    $"public {className}({_state.ToConstructorParameters()}) : base({_state.ToBaseCall()})"),

                _state.Select(p =>
                    new Scope($"public override {className} With{p.Name}({p.TypeName} {p.ParameterName})") {
                        $"return new {className}({string.Join(",", _state.Select(p2 => p == p2 ? p2.ParameterName : p2.Name))});"
                    }),

                parentClassName == Kw.Protonode ? // TODO:
                    $"public override {parentClassName} Parent => null;" :
                    $"public override {parentClassName} Parent => new {parentClassName}({_state.ToCall()});",
                $"public override {Kw.AlphaRoot} Root => new {Kw.AlphaRoot}({_state.ToCall()});",
                $"public override string[] Path => new string[]{{{string.Join(",", path.Select(x => $"\"{x}\""))}}};",
                $"public override string Name => \"{node.Symbol.Name}\";",

                WriteRepresentatives(node, className),
                WriteChildren(node),

                // TODO: what to do with classes (not namespaces) and pages, that contain representatives?
                node.Children.Where(c => c.ContainsRepresentatives).Select(c =>
                    new Group {
                        $"public virtual {Kw.α}{c.Symbol.Name} {c.Symbol.Name} => new({_state.ToCall()});",
                        WriteClassTreeNode(c,  className)
                    })
            };

            return nodeClass;
        }

        static Group WriteRepresentatives(PagesTreeNode pageNode, string alphaClassName) {
            var representatives = pageNode.Children.Where(c => c.IsRepresentative).Select(c => c.Symbol as INamedTypeSymbol);
            ///TODO: check if baseTypeFullName==object
            ///Error CS0553  'StaticSharpRoot.αIndex.αComponents.implicit operator object(StaticSharpRoot.αIndex.αComponents)': user - defined conversions to or from a base type are not allowed  StaticSharpDemo D:\StaticSharp\StaticSharpDemo\StaticSharpGenerator\StaticSharpGenerator.StaticSharpGenerator\classTree.cs  100 Active

            var representative = representatives.FirstOrDefault();
            var baseTypeFullName = representative?.BaseType.GetFullyQualifiedName();

            switch (representatives.Count()) {
                case 0:
                    return new Group {
                        $"public override {Kw.Page} {Kw.Representative} => null;"
                    };
                case 1:
                    return new Group {
                        $"public static implicit operator {baseTypeFullName}({alphaClassName} {Kw.α}) => {Kw.α}.{Kw.Representative};",
                        $"public override {baseTypeFullName} {Kw.Representative} => new {representative.GetFullyQualifiedName()}({_state.ToCall()});"
                    };
                default:
                    return new Group {
                        $"public static implicit operator {baseTypeFullName}({alphaClassName} {Kw.α}) => {Kw.α}.{Kw.Representative};",
                        $"public override {baseTypeFullName} {Kw.Representative} => SelectRepresentative({Kw.Representatives});",
                        new Scope($"public IEnumerable<{baseTypeFullName}> {Kw.Representatives}") {
                            new Scope("get") {
                                representatives.Select(s => $"yield return new {s.GetFullyQualifiedName()}({_state.ToCall()});")
                            }
                        }
                    };
            }
        }

        static Scope WriteChildren(PagesTreeNode pageNode) {
            var childContainersNames = pageNode.Children.Where(c => c.ContainsRepresentatives).Select(c => c.Symbol.Name);
            return new Scope($"public override IEnumerable<{Kw.Protonode}> Children") {
                new Scope("get") {
                    childContainersNames.Any() ?
                        childContainersNames.Select(n => $"yield return {n};") :
                        new [] { $"return Enumerable.Empty<{Kw.Protonode}>();" }
                }
            };
        }
    }
}
