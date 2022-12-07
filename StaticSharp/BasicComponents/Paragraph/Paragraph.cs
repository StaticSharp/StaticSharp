using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    



    [RelatedStyle]
    [ConstructorJs]
    //[Mix(typeof(ParagraphBindings<Js.Paragraph>))]
    public partial class Paragraph : ParagraphBase {
        public Inlines Inlines { get; } = new();
        protected override Task<Inlines> GetInlinesAsync() => Task.FromResult(Inlines);

        public Paragraph(
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
        }

        public Paragraph(Paragraph other,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(other, callerLineNumber, callerFilePath) {

            Inlines = new(other.Inlines);
        }


        public Paragraph(string text,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Inlines.AppendLiteral(text, callerFilePath, callerLineNumber);
        }

        public Paragraph(Inlines inlines,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Inlines = new(inlines);
        }
        public Paragraph(Inline inline,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Inlines = new() { { null, inline } };
        }


               
    }
}
