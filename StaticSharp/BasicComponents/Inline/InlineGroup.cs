using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp {
    
    public sealed partial class InlineGroup : Inline {

        public Inlines Children { get; init; } = new();

        protected InlineGroup(InlineGroup other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerLineNumber, callerFilePath) {
            Children = new(other.Children);
        }
        public InlineGroup(
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        public InlineGroup(
            string text,
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Children.Add(text);
        }

        public InlineGroup(
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

    }
}