

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exo.CSharpSyntaxTreeInspector {


    public class TypeInfo : TypeContainer {
        

        public List<BaseTypeDeclarationSyntax> Parts { get; } = new List<BaseTypeDeclarationSyntax>();

        public bool IsPartial => Parts[0].Modifiers.Any(x => x.IsKind(SyntaxKind.PartialKeyword));

        public bool IsClass => Parts[0] is ClassDeclarationSyntax;
        public bool IsInterface => Parts[0] is InterfaceDeclarationSyntax;
        public bool IsEnum => Parts[0] is EnumDeclarationSyntax;
        public bool IsStruct => Parts[0] is StructDeclarationSyntax;
        public bool IsRecord => Parts[0] is RecordDeclarationSyntax;

        //TODO: test me
        //public bool IsDelegate => TypeDeclarations[0].Keyword.IsKind(SyntaxKind.DelegateKeyword);

        //Record Struct ?????

        public string Keyword {
            get {
                if (Parts[0] is TypeDeclarationSyntax typeDeclarationSyntax) {
                    return typeDeclarationSyntax.Keyword.ValueText;
                }
                if (Parts[0] is EnumDeclarationSyntax) {
                    return "enum";
                }
                throw new NotImplementedException();
            }
        }
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
                if (part is EnumDeclarationSyntax enumDeclarationSyntax) {
                    foreach (var member in enumDeclarationSyntax.Members) {
                        yield return member;
                    }
                    continue;
                }

                if (part is TypeDeclarationSyntax typeDeclarationSyntax) {
                    foreach (var member in typeDeclarationSyntax.Members) {
                        yield return member;
                    }
                    continue;
                }

                throw new NotImplementedException();
            }

        }
        public ITypeSymbol GetTypeSymbol(CSharpCompilation compilation) {            
            SemanticModel semanticModel = compilation.GetSemanticModel(Parts.First().SyntaxTree);
            var typeInfo = semanticModel.GetTypeInfo(Parts.First());
            return typeInfo.Type;
        }
    }
}