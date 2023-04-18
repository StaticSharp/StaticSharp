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


        public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            base.ModifyTagAndScript(context, tag, script);


            foreach (var i in Children) {
                var child = i.Generate(context);
                tag.Add(child.Tag);
                if (child.Script != null) {
                    script.Add(child.Script);
                    script.Add($"{child.Tag.Id}.Parent = {tag.Id}");
                }
            }
        }


        public override string ToString() {
            throw new System.InvalidOperationException("Cast from Inline to String is forbidden.");
        }

        public override string GetPlainText(Context context) {
            return Children.GetPlainText(context);
        }
    }
}