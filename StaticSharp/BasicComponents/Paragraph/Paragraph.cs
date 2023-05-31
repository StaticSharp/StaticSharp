using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;

namespace StaticSharp {
    public enum TextAlignmentHorizontal {
        Left,
        Center,
        Right,
        Justify,
        JustifyIncludingLastLine
    }


    public interface JParagraph : JBlock {
        bool NoWrap { get; set; }
        TextAlignmentHorizontal TextAlignmentHorizontal { get; set; }

        double InternalWidth { get; }
        double InternalHeight { get; }
        

    }

    [RelatedStyle]
    [ConstructorJs]
    public partial class Paragraph : Block {

        public bool CollapseSpaces { get; set; } = false;
        public Inlines Inlines { get; } = new();

        public Paragraph(Paragraph other,
            int callerLineNumber,
            string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
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
        public Paragraph(AbstractInline inline,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Inlines = new() { inline };
        }

        public override void ModifyTagAndScript(Context context, Tag tag, Scopes.Group script) {
            base.ModifyTagAndScript(context, tag, script);
            var inlineContainerId = context.CreateId();

            var inlineContainer = new Tag("p", inlineContainerId) {
                ["class"] = "inline-container",
                Style = {
                    ["letter-spacing"] = $"{context.LetterSpacing.ToStringInvariant()}em"
                }
                
            };

            if (CollapseSpaces) {
                inlineContainer.Style.Add("white-space", "pre-line");
            }

            foreach (var i in Inlines) {
                var child = i.Generate(context);
                inlineContainer.Add(child.Tag);
                if (child.Script != null) {
                    script.Add(child.Script);
                    script.Add($"{child.Tag.Id}.Parent = {tag.Id}");
                }                
            }
            //inlineContainer.Add("\n");

            tag.Add(inlineContainer);

            script.Add($"{tag.Id}.inlineContainer = {TagToJsValue(inlineContainer)}");
        }


    }
}
