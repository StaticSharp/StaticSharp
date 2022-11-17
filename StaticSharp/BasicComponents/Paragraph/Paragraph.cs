using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    public enum TextAlignmentHorizontal { 
        Left,
        Center,
        Right,
        Justify,
        JustifyIncludingLastLine
    }


    namespace Js {
        public class Paragraph : Block {
            public TextAlignmentHorizontal TextAlignmentHorizontal => NotEvaluatableValue<TextAlignmentHorizontal>();

        }
    }



    namespace Gears {
        public class ParagraphBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
            public Binding<TextAlignmentHorizontal> TextAlignmentHorizontal { set { Apply(value); } }
        }
    }

    [RelatedStyle]
    [ConstructorJs]
    [Mix(typeof(ParagraphBindings<Js.Paragraph>))]
    public partial class Paragraph : Block {
        public Inlines Inlines { get; } = new();


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


        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {            

            var p = new Tag("p");
            foreach (var i in Inlines) {
                var child = await i.Value.GenerateHtmlAsync(context, new Role(true,i.Key));
                p.Add(child);
            }
            p.Add("\n");
            elementTag.Add(p);

            await base.ModifyHtmlAsync(context, elementTag);
        }        
    }
}
