using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

public class GeneratorSyntaxWalker : CSharpSyntaxWalker {
    public ISyntaxReceiver SyntaxReceiver { get; private set; }

    public GeneratorSyntaxWalker(ISyntaxReceiver syntaxReceiver) {
        SyntaxReceiver = syntaxReceiver;
    }

    public override void DefaultVisit(SyntaxNode node) {
        SyntaxReceiver.OnVisitSyntaxNode(node);
        base.DefaultVisit(node);
    }
}
//}
