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
        public interface Paragraph : Block {
            public TextAlignmentHorizontal TextAlignmentHorizontal { get; }
        }
    }

    namespace Gears {
        public class ParagraphBindings<FinalJs> : BlockBindings<FinalJs> {
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
            Inlines.AppendLiteral(text, callerLineNumber, callerFilePath);
        }

        public Paragraph(Inlines inlines,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Inlines = new(inlines);
        }
        public Paragraph(Inline inline,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Inlines = new() { inline };
        }

        public override void ModifyTagAndScript(Context context, Tag tag, Scopes.Group script) {
            base.ModifyTagAndScript(context, tag, script);
            var inlineContainerId = context.CreateId();

            var inlineContainer = new Tag("p", inlineContainerId) {
                ["class"] = "inline-container"
            };

            foreach (var i in Inlines) {
                var child = i.Generate(context);
                inlineContainer.Add(child.Tag);
                if (child.Script != null) {
                    script.Add(child.Script);
                    script.Add($"{child.Tag.Id}.Parent = {tag.Id}");
                }                
            }
            inlineContainer.Add("\n");

            tag.Add(inlineContainer);

            script.Add($"{tag.Id}.inlineContainer = {TagToJsValue(inlineContainer)}");
        }


    }
}
