using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exo.CSharpSyntaxTreeInspector {
    public class NamespaceInfo : TypeContainer {

        public List<NamespaceInfo> Namespaces { get; } = new List<NamespaceInfo>();


        public NamespaceInfo(string name, NamespaceInfo parent = null) : base(name, parent) {
        }
        public NamespaceInfo AddNamespaceBranch(IEnumerable<string> names) {
            var node = this;
            foreach (var n in names) {
                node = node.GetOrCreateNamespace(n);
            }
            return node;
        }

        public NamespaceInfo GetOrCreateNamespace(NameSyntax name) {
            if (name is SimpleNameSyntax simpleName) {
                return GetOrCreateNamespace(simpleName.Identifier.ValueText);
            }
            if (name is QualifiedNameSyntax qualifiedName) {
                var left = GetOrCreateNamespace(qualifiedName.Left);
                return left.GetOrCreateNamespace(qualifiedName.Right.Identifier.ValueText);
            }
            throw new NotImplementedException();
        }


        public NamespaceInfo GetOrCreateNamespace(string name) {
            var existing = Namespaces.FirstOrDefault(x => x.Name == name);
            if (existing != null)
                return existing;

            var result = new NamespaceInfo(name, this);
            Namespaces.Add(result);
            return result;
        }

        public override string ToString() {
            return $"namespace {Name}";
        }


        public IEnumerable<TypeInfo> GetTypesRecursively() {
            foreach (var i in Types) {
                yield return i;
            }
            foreach (var i in Namespaces.SelectMany(x=>x.GetTypesRecursively())) {
                yield return i;
            }
    }


    }
}