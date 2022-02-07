using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace StaticSharpWeb.Components {

    public class CSharpCode : Code {

        protected override ProgrammingLanguage ProgrammingLanguage => ProgrammingLanguage.CSharp;

        public SyntaxNode AstNode { get; private set; }

        public TextSpan Roi { get; private set; }

        public CSharpCode(string source) : base(source) {
            AstNode = CSharpSyntaxTree.ParseText(source).GetRoot();
            Roi = AstNode.FullSpan;
        }

        public CSharpCode(GitHub.GithubResource resource) : base(resource.Source) { }

        private CSharpCode(CSharpCode origin, SyntaxNode astNode, TextSpan roi) : base(origin.Source) {
            AstNode = astNode;
            Roi = roi;
        }

        private CSharpCode(CSharpCode origin, SyntaxNode astNode) : base(origin.Source) {
            AstNode = astNode;
            Roi = astNode.FullSpan;
        }

        protected override string FinalSourceCode {
            get {

                var relativeSpan = new TextSpan(Roi.Start - AstNode.FullSpan.Start, Roi.Length);
                var nodeFragment = AstNode.ToFullString();

                int i = relativeSpan.Start;
                while (i > 0 && char.IsWhiteSpace(nodeFragment[i]) && nodeFragment[i] != '\n') {
                    i--;
                }
                var roiFragment = nodeFragment[i..relativeSpan.End];
                return Untab(TrimEmptyLines(roiFragment));
            }
        }

        protected override Range? LineSpan {
            get {
                if (Roi.Start == 0 && Roi.End == AstNode.SyntaxTree.Length) // -1 ??
                    return null;

                var span = AstNode.SyntaxTree.GetLineSpan(Roi);
                return new System.Range(span.StartLinePosition.Line, span.EndLinePosition.Line);
            }
        }

        public CSharpCode GetClass(string className, bool fullyQualified = false) {

            bool verifyParents(ClassDeclarationSyntax classDeclarationNode) {
                if (!fullyQualified)
                    return true;

                var expected = className.Split(".+".ToCharArray());
                var actual = GetClassRelativeNameComponents(AstNode, classDeclarationNode);

                if (expected.Length != actual.Length)
                    return false;

                return Enumerable.SequenceEqual(expected, actual);
            }

            string shortClassName = className;
            if (fullyQualified) {
                int dotIdx = className.IndexOfAny(".+".ToCharArray());
                if (dotIdx >= 0)
                    shortClassName = className.Substring(dotIdx + 1);
            }

            var classDeclarationNodes = AstNode
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(node => node.Identifier.ValueText == shortClassName)
                .Where(node => verifyParents(node))
                .ToArray(); ;

            if (classDeclarationNodes.Length == 0)
                Log.Error.OnCaller("Class not found: {className}");

            if (classDeclarationNodes.Length > 1)
                Log.Error.OnCaller("Class not unique: {className}");

            return new CSharpCode(this, classDeclarationNodes[0]);
        }

        public CSharpCode GetMethod(string methodName) {

            var methodDeclarationNode = AstNode
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(node => node.Identifier.ValueText == methodName)
                .FirstOrDefault();

            if (methodDeclarationNode == null)
                Log.Error.OnCaller($"Method not found: {methodName}");

            return new CSharpCode(this, methodDeclarationNode);
        }

        public CSharpCode GetConstructor(string constructorName) {

            var constructorDeclarationNode = AstNode
                .DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>()
                .Where(node => node.Identifier.ValueText == constructorName)
                .FirstOrDefault();

            if (constructorDeclarationNode == null)
                Log.Error.OnCaller($"Constructor not found: {constructorName}");

            return new CSharpCode(this, constructorDeclarationNode);
        }

        public CSharpCode GetDestructor(string destructorName) {

            var destructorDeclarationNode = AstNode
                .DescendantNodes()
                .OfType<DestructorDeclarationSyntax>()
                .Where(node => node.Identifier.ValueText == destructorName)
                .FirstOrDefault();

            if (destructorDeclarationNode == null)
                Log.Error.OnCaller($"Destructor not found: {destructorName}");

            return new CSharpCode(this, destructorDeclarationNode);
        }

        public CSharpCode GetRegion(string regionLabel) {

            bool found = FindRegion(regionLabel,
                out RegionDirectiveTriviaSyntax regionNode,
                out EndRegionDirectiveTriviaSyntax endRegionNode);

            if (!found)
                Log.Error.OnCaller($"Cannot find #region: {regionLabel}");

            var roi = TextSpan.FromBounds(regionNode.Span.End, endRegionNode.Span.Start);
            return new CSharpCode(this, AstNode, roi);
        }


        private static string[] GetClassRelativeNameComponents(
                    SyntaxNode root, ClassDeclarationSyntax classDeclarationNode) {

            var result = new List<string>();
            foreach (SyntaxNode node in classDeclarationNode.AncestorsAndSelf()) {

                if (node == root)
                    break;

                switch (node) {
                    case ClassDeclarationSyntax n:
                        result.Add(n.Identifier.ValueText);
                        break;

                    case NamespaceDeclarationSyntax n:
                        switch (n.Name) {
                            case IdentifierNameSyntax ident:
                                result.Add(ident.Identifier.ValueText);
                                break;

                            case QualifiedNameSyntax qname:
                                foreach (var ident in qname.DescendantNodes().OfType<IdentifierNameSyntax>().Reverse())
                                    result.Add(ident.Identifier.ValueText);
                                break;
                        }
                        break;
                }
            }

            result.Reverse();
            return result.ToArray();
        }

        private bool FindRegion(string regionLabel,
                out RegionDirectiveTriviaSyntax regionNode,
                out EndRegionDirectiveTriviaSyntax endRegionNode) {

            static string extractRegionLabel(RegionDirectiveTriviaSyntax node) {
                int prefixStart = node.FullSpan.Start;
                int prefixEnd = node.RegionKeyword.FullSpan.End;
                int prefixLength = prefixEnd - prefixStart;
                return node.ToFullString().Substring(prefixLength).Trim();
            }

            regionNode = null;
            endRegionNode = null;

            int currentLevel = 0;
            int regionFoundAtLevel = -1;
            foreach (var node in AstNode.DescendantNodes(descendIntoTrivia: true)) {
                switch (node) {
                    case RegionDirectiveTriviaSyntax n:

                        if (regionNode == null && extractRegionLabel(n) == regionLabel) {
                            regionNode = n;
                            regionFoundAtLevel = currentLevel;
                        }

                        currentLevel++;
                        break;

                    case EndRegionDirectiveTriviaSyntax n:
                        currentLevel--;
                        if (regionNode != null && endRegionNode == null && currentLevel == regionFoundAtLevel)
                            endRegionNode = n;

                        break;
                }
            }

            return (regionNode != null && endRegionNode != null);
        }
    }
}