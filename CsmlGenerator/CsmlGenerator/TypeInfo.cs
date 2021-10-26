using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CsmlGenerator {
    public class TypeInfo : TypeContainer {
        
       

        public List<TypeDeclarationSyntax> TypeDeclarations { get; } = new List<TypeDeclarationSyntax>();

        public bool IsPartial => TypeDeclarations[0].IsPartial();
        public string Keyword => TypeDeclarations[0].Keyword.ValueText;



        public IEnumerable<TypeInfo> TypePath {
            get {
                if (Parent is TypeInfo parentTypeInfo) {
                    return parentTypeInfo.TypePath.Append(this);
                }
                return Enumerable.Repeat(this,1);            
            }
        }




        public string FullyQualifiedName {
            get {
                var namespacePath = NamespaceParent?.Path;
                var typePath = string.Join("+", TypePath.Select(x => x.Name));
                if (namespacePath != null) {
                    return string.Join(".", namespacePath.Skip(1).Select(x => x.Name))+ "." + typePath;
                }
                return typePath;
            }
        
        }


        public IEnumerable<AttributeSyntax> Attributes {
            get {
                foreach (var d in TypeDeclarations) {
                    foreach (var al in d.AttributeLists) {
                        foreach (var a in al.Attributes) {
                            yield return a;
                        }
                    }
                }
            }
        }


        public TypeInfo(string name, TypeContainer parent): base(name, parent) {
        }

    }
}
