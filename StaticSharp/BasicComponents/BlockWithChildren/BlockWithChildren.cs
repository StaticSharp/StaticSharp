using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Js {
        public interface BlockWithChildren : Block {
            /*public Block First => NotEvaluatableObject<Block>();
            public Block Second => NotEvaluatableObject<Block>();
            public bool Flipped { get; }

            public bool RightToLeft { get; }
            public bool BottomToTop { get; }*/

           public Enumerable<Block> Children { get; }
        }
    }


    namespace Gears {
        public class BlockWithChildrenBindings<FinalJs> : BlockBindings<FinalJs> {
            /*public Binding<bool> Flipped { set { Apply(value); } }
            public Binding<bool> RightToLeft { set { Apply(value); } }
            public Binding<bool> BottomToTop { set { Apply(value); } }*/
        }
    }


    [Mix(typeof(BlockWithChildrenBindings<Js.BlockWithChildren>))]
    [ConstructorJs]
    public partial class BlockWithChildren : Block {

        [Socket]
        public virtual Blocks Children { get; } = new();

        public BlockWithChildren(BlockWithChildren other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
        }
        public BlockWithChildren([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag) {
            var array = Children.ToArray();
            if (array.Length > 0) {
                elementTag.Add(CreateScript_SetCurrentSocket("ChildrenFirst"));
                elementTag.Add(Children.GenerateHtml(context));
            }
            base.ModifyHtml(context, elementTag);
        }
    }
}