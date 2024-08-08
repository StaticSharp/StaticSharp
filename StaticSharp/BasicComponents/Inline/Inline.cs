using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp {

    public interface JInline : JAbstractInline {
    }

    public sealed partial class Inline : AbstractInline {

        public Inlines Children { get; init; } = new();

        public Inline(Inline other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerLineNumber, callerFilePath) {
            Children = new(other.Children);
        }
        public Inline(
            string text,
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Children.Add(text);
        }
        public Inline(
            Inlines inlines,
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Children.Add(inlines);
        }


        public override void ModifyTagAndScript(Context context, Tag tag, Group scriptBeforeConstructor, Group scriptAfterConstructor) {
            base.ModifyTagAndScript(context, tag, scriptBeforeConstructor, scriptAfterConstructor);


            foreach (var i in Children) {
                var child = i.Generate(context);
                tag.Add(child.Tag);
                if (child.Script != null) {
                    scriptBeforeConstructor.Add(child.Script);
                    scriptBeforeConstructor.Add($"{child.Tag.Id}.Parent = {tag.Id}");
                }
            }
        }


        public override string ToString() {
            throw new System.InvalidOperationException("Cast from Inline to String is forbidden.");
        }

        public override string GetPlainText() {
            return Children.GetPlainText();
        }
    }
}