

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Exo.CSharpSyntaxTreeInspector {


    public class TypeInfo : TypeContainer {
        

        public List<TypeDeclarationSyntax> Parts { get; } = new List<TypeDeclarationSyntax>();

        public bool IsPartial => Parts[0].Modifiers.Any(x => x.IsKind(SyntaxKind.PartialKeyword));

        public bool IsClass => Parts[0].Keyword.IsKind(SyntaxKind.ClassKeyword);
        public bool IsInterface => Parts[0].Keyword.IsKind(SyntaxKind.InterfaceKeyword);
        public bool IsEnum => Parts[0].Keyword.IsKind(SyntaxKind.EnumKeyword);
        public bool IsStruct => Parts[0].Keyword.IsKind(SyntaxKind.StructKeyword);
        public bool IsRecord => Parts[0].Keyword.IsKind(SyntaxKind.RecordKeyword);

        //TODO: test me
        //public bool IsDelegate => TypeDeclarations[0].Keyword.IsKind(SyntaxKind.DelegateKeyword);

        //Record Struct ?????

        public string Keyword => Parts[0].Keyword.ValueText;
        public string[] Parameters { get; }
        public IEnumerable<AttributeSyntax> Attributes {
            get {
                foreach (var d in Parts) {
                    foreach (var al in d.AttributeLists) {
                        foreach (var a in al.Attributes) {
                            yield return a;
                        }
                    }
                }
            }
        }
        public override string ToString() {
            if (Parameters == null) {
                return $"{Keyword} {Name}";
            } else {
                return $"{Keyword} {Name} <{string.Join(",", Parameters)}>";
            }
        }
        public TypeInfo(string name, string[] parameters, TypeContainer parent) : base(name, parent) {
            Parameters = parameters;
        }
        public IEnumerable<MemberDeclarationSyntax> GetMembers() {
            foreach (var part in Parts) {
                foreach (var member in part.Members) {
                    yield return member;
                }
            }
        }
        public ITypeSymbol GetTypeSymbol(CSharpCompilation compilation) {            
            SemanticModel semanticModel = compilation.GetSemanticModel(Parts.First().SyntaxTree);
            var typeInfo = semanticModel.GetTypeInfo(Parts.First());
            return typeInfo.Type;
        }
    }
}