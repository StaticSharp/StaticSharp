using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


using System;
using System.Collections.Generic;

namespace Exo.CSharpSyntaxTreeInspector {

    public class AssemblyInfo {

        public Compilation Compilation { get; }

        public NamespaceInfo GlobalNamespace { get; }

        public AssemblyInfo(Compilation compilation) {
            Compilation = compilation;

            GlobalNamespace = new NamespaceInfo("");

            foreach (var i in Compilation.SyntaxTrees) {
                ParseSyntaxTree(i);
            }
        }

        private void ParseSyntaxTree(SyntaxTree syntaxTree) {
            var root = syntaxTree.GetRoot();
            //var usings = new List<UsingDirectiveSyntax>();

            var currentNamespace = GlobalNamespace;

            foreach (var i in root.ChildNodes()) {
                var known =
                        ParseFileScopedNamespaceDeclaration(i, ref currentNamespace) ||
                        ParseUsing(i) ||
                        ParseNamespace(i, currentNamespace) ||
                        ParseType(i, currentNamespace);

                if (!known) {
                    Console.WriteLine($"unknown node {i}");
                }


            }
        }

        private bool ParseUsing(SyntaxNode node) {
            if (node is UsingDirectiveSyntax usingDirectiveSyntax) {
                //usings.Add(usingDirectiveSyntax);
                return true;
            }
            return false;
        }

        private bool ParseFileScopedNamespaceDeclaration(SyntaxNode node, ref NamespaceInfo currentNamespace) {
            if (node is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclarationSyntax) {
                currentNamespace = currentNamespace.GetOrCreateNamespace(fileScopedNamespaceDeclarationSyntax.Name);
                return true;
            }
            return false;
        }

        private bool ParseNamespace(SyntaxNode node, NamespaceInfo currentNamespace) {
            if (node is NamespaceDeclarationSyntax namespaceDeclarationSyntax) {

                //var nameItems = namespaceDeclarationSyntax.Name.Split();
                //currentNamespace = currentNamespace.AddNamespaceBranch(nameItems.Select(n => n.Identifier.ValueText));
                currentNamespace = currentNamespace.GetOrCreateNamespace(namespaceDeclarationSyntax.Name);


                foreach (var i in namespaceDeclarationSyntax.Members) {
                    var known =
                        ParseUsing(i) ||
                        ParseNamespace(i, currentNamespace) ||
                        ParseType(i, currentNamespace);

                    if (!known) {
                        throw new NotImplementedException();
                    }
                }

                return true;
            }
            return false;
        }

        private bool ParseType(SyntaxNode node, TypeContainer typeContainer) {

            if (node is EnumDeclarationSyntax enumDeclarationSyntax) {
                typeContainer.AddTypeDeclaration(enumDeclarationSyntax);
                return true;
            }

            if (node is TypeDeclarationSyntax typeDeclarationSyntax) {
                typeContainer.AddTypeDeclaration(typeDeclarationSyntax);
                return true;
            }
            return false;
        }



    }
}
