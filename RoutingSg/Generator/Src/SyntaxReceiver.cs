using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class SyntaxReceiver : ISyntaxReceiver {
    //public Tree<string, NamespaceDeclarationSyntax> namespaceTree = new(null);


    public List<ClassDeclarationSyntax> CandidateClasses { get; } =
        new List<ClassDeclarationSyntax>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode) {

        /*if (syntaxNode is NamespaceDeclarationSyntax namespaceDeclarationSyntax) {

            var items = namespaceDeclarationSyntax.Name.Split();


            Console.WriteLine(namespaceDeclarationSyntax.Name.GetType().Name," - ",  namespaceDeclarationSyntax.Name);
        }*/

         /*   //classDeclarationSyntax.AttributeLists.Count > 0
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax) {

            var identifier = classDeclarationSyntax.Identifier;


            if (classDeclarationSyntax.IsPartial())
                CandidateClasses.Add(classDeclarationSyntax);

            NodeVisited.Add(syntaxNode.GetType().Name+" "+ classDeclarationSyntax.Identifier.Text);
        } else {
            NodeVisited.Add(syntaxNode.GetType().Name);
        }*/
    }
}
