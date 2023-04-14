using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exo.CSharpSyntaxTreeInspector {

    public class TypeContainer : Named {

        public TypeContainer Parent { get; private set; }
        /*public NamespaceInfo NamespaceParent => (Parent is NamespaceInfo namespaceInfo) ?
           namespaceInfo :
           Parent?.NamespaceParent;*/

        public IEnumerable<TypeContainer> Path {
            get {
                if (Parent == null) return Enumerable.Repeat(this, 1);
                return Parent.Path.Append(this);
            }
        }

        public string PathString => string.Join(".", Path.Skip(1).Select(x => x.Name));


        //public Dictionary<string, TypeInfo> Types { get; } = new Dictionary<string, TypeInfo>();
        public List<TypeInfo> Types { get; } = new List<TypeInfo>();

        public TypeContainer(string name, TypeContainer parent) : base(name) {
            Parent = parent;
        }
        private TypeInfo GetOrCreateType(string name, string[] parameters) {
            var existing = Types.FirstOrDefault(x => (x.Name == name) && x.Parameters.AsSpan().SequenceEqual(parameters));
            if (existing != null)
                return existing;

            var result = new TypeInfo(name, parameters, this);
            Types.Add(result);
            return result;
        }
        public void AddTypeDeclaration(TypeDeclarationSyntax typeDeclaration) {
            var parameters = typeDeclaration.TypeParameterList?.Parameters.Select(x => x.Identifier.ValueText).ToArray();
            var t = GetOrCreateType(typeDeclaration.Identifier.ValueText, parameters);
            t.Parts.Add(typeDeclaration);
        }

        public void AddTypeDeclaration(EnumDeclarationSyntax typeDeclaration) {
            //var parameters = typeDeclaration.TypeParameterList?.Parameters.Select(x => x.Identifier.ValueText).ToArray();
            var t = GetOrCreateType(typeDeclaration.Identifier.ValueText, null);
            t.Parts.Add(typeDeclaration);
        }
        

    }
}
