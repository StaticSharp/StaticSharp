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


    [ConstructorJs]
    [Mix(typeof(ParagraphBindings<Js.Paragraph>))]
    public partial class Paragraph : Block {
        public Inlines Inlines { get; } = new();

        public Paragraph(Paragraph other,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(other, callerFilePath, callerLineNumber) {

            Inlines = new(other.Inlines);
        }


        public Paragraph(string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Inlines.AppendLiteral(text, callerFilePath, callerLineNumber);
        }

        public Paragraph(Inlines inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Inlines = new(inlines);
        }
        public Paragraph(Inline inline,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Inlines = new() { inline };
        }


        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {            

            var p = new Tag("p");
            foreach (var i in Inlines) {
                var child = await i.Value.GenerateHtmlAsync(context);
                if (i.Modifier != null)
                    await i.Modifier.Apply(child);
                //child.AddAsChild();
                p.Add(child);
            }
            elementTag.Add(p);

            await base.ModifyHtmlAsync(context, elementTag);
        }        
    }
}
