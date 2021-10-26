using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CsmlGenerator {
    public class Named {
        public string Name { get; private set; }
        public Named(string name) {
            Name = name;
        }
    }
    public class TypeContainer: Named {

        public TypeContainer Parent { get; private set; }
        public NamespaceInfo NamespaceParent => (Parent is NamespaceInfo namespaceInfo) ?
           namespaceInfo :
           Parent?.NamespaceParent;

        public IEnumerable<TypeContainer> Path {
            get {
                if (Parent == null) return Enumerable.Repeat(this, 1);
                return Parent.Path.Append(this);                
            }
        }


        public Dictionary<string, TypeInfo> Types { get; } = new Dictionary<string, TypeInfo>();


        public TypeContainer(string name, TypeContainer parent) : base(name) {
            Parent = parent;
        }






        private TypeInfo GetOrCreateType(string name) {
            TypeInfo result;
            if (!Types.TryGetValue(name, out result)) {
                result = new TypeInfo(name, this);
                Types.Add(name, result);
            }
            return result;
        }

        public void AddType(TypeDeclarationSyntax typeDeclaration) {
            var t = GetOrCreateType(typeDeclaration.Identifier.ValueText);
            t.TypeDeclarations.Add(typeDeclaration);
        }

    }

    public class NamespaceInfo : TypeContainer {

        
        public Dictionary<string, NamespaceInfo> Namespaces { get; } = new Dictionary<string, NamespaceInfo>();
        public NamespaceInfo(string name, TypeContainer parent) : base(name, parent) {
        }

        public string FullName => string.Join(".", Path.Skip(1).Select(x => x.Name));


        public IEnumerable<NamespaceInfo> GetAllNamespaces() {
            yield return this;
            foreach(var i in Namespaces.Values.SelectMany(n => n.GetAllNamespaces())) {
                yield return i;
            }
        }
        


        public NamespaceInfo AddNamespaceBranch(IEnumerable<string> names) {
            var node = this;
            foreach (var n in names) {
                node = node.GetOrCreateNamespace(n);
            }
            return node;
        }

        public NamespaceInfo GetOrCreateNamespace(string name) {
            if(!Namespaces.TryGetValue(name, out NamespaceInfo result)) {
                result = new NamespaceInfo(name, this);
                Namespaces.Add(name, result);
            }
            return result;
        }

        
    }
}
