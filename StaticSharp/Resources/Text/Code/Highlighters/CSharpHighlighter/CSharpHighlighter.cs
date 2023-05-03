using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace StaticSharp;

public class CSharpHighlighter : IHighlighter {
    public Inlines Highlight(string text, string? fileExtension = null) {

        SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
        var root = tree.GetRoot();
        var result = new Inlines();
        var walker = new Walker();
        walker.Visit(root);

        var sortedUnits = walker.Units.OrderBy(x => x.Span.Start).ToArray();
        var endOfPrevious = 0;
        for (int i = 0; i < sortedUnits.Length; i++) { 
            var start = sortedUnits[i].Span.Start;
            var length = sortedUnits[i].Span.Length;
            if ((start - endOfPrevious) > 0) {
                result.Add(text.Substring(endOfPrevious, start - endOfPrevious));
            }
            if (length > 0) {
                var subText = text.Substring(start, length);
                result.Add(Style(sortedUnits[i].Kind, subText));
            }
            endOfPrevious = start + length;
        }
        result.Add(text.Substring(endOfPrevious, text.Length - endOfPrevious));
        return result;
    }


    public Func<Kind, string, Inline> Style { get; set; } = (kind, text) => {
        return kind switch {
            Kind.Comment => new Inline(text) { ForegroundColor = Color.Green },
            Kind.PreprocessorDirectives => new Inline(text) { ForegroundColor = Color.Gray },
            Kind.String => new Inline(text) { ForegroundColor = new Color("a31515") },
            Kind.Number => new Inline(text) { ForegroundColor = Color.OrangeRed },
            Kind.Keyword => new Inline(text) { ForegroundColor = Color.Blue },
            _ => new Inline(text)
        };
    };


    public enum Kind { 
        Comment,
        PreprocessorDirectives,
        String,
        Number,
        Keyword,
    }

    record Unit(Kind Kind, TextSpan Span) { }

    class Walker : CSharpSyntaxWalker {
        public List<Unit> Units { get; } = new ();

        public Walker() : base(SyntaxWalkerDepth.StructuredTrivia) {
        }
        public override void VisitTrivia(SyntaxTrivia node) {

            var kind = node.Kind();
            switch (kind) {
                case (>=SyntaxKind.SingleLineCommentTrivia)
                and (<= SyntaxKind.MultiLineDocumentationCommentTrivia):

                Units.Add(new Unit(Kind.Comment, node.Span));
                return;

                case (>= SyntaxKind.IfDirectiveTrivia)
                and (<= SyntaxKind.BadDirectiveTrivia):
                Units.Add(new Unit(Kind.PreprocessorDirectives, node.Span));
                return;
            }
            base.VisitTrivia(node);
        }
        public override void VisitToken(SyntaxToken node) {
            var kind = node.Kind();
            if (IsReservedKeyword(kind) || IsContextualKeyword(kind)) {
                Units.Add(new Unit(Kind.Keyword, node.Span));
            } else
            if (IsString(kind)) {
                Units.Add(new Unit(Kind.String, node.Span));
            } else
            if (IsNumber(kind)) {
                Units.Add(new Unit(Kind.Number, node.Span));
            }
            base.VisitToken(node);
        }




        public static bool IsReservedKeyword(SyntaxKind kind) {
            return kind >= SyntaxKind.BoolKeyword && kind <= SyntaxKind.ImplicitKeyword;
        }
        
        public static bool IsContextualKeyword(SyntaxKind kind) {
            return kind >= SyntaxKind.YieldKeyword && kind <= SyntaxKind.FileKeyword;
        }

        public static bool IsString(SyntaxKind kind) {
            switch (kind) {
                case SyntaxKind.StringLiteralToken:
                case SyntaxKind.SingleLineRawStringLiteralToken:
                case SyntaxKind.MultiLineRawStringLiteralToken:

                case SyntaxKind.InterpolatedSingleLineRawStringStartToken:
                case SyntaxKind.InterpolatedRawStringEndToken:

                case SyntaxKind.InterpolatedStringStartToken:
                case SyntaxKind.InterpolatedVerbatimStringStartToken:
                case SyntaxKind.InterpolatedMultiLineRawStringStartToken:

                case SyntaxKind.InterpolatedStringEndToken:
                case SyntaxKind.InterpolatedStringTextToken:

                case SyntaxKind.CharacterLiteralToken:

                return true;
            }
            return false;
        }
        public static bool IsNumber(SyntaxKind kind) {
            return kind == SyntaxKind.NumericLiteralToken;
        }


    }


}