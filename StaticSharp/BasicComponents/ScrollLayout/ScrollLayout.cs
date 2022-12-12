using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    namespace Js {
        public class ScrollLayout : Block {
            public Block Content => NotEvaluatableObject<Block>();

        }
    }

    namespace Gears {
        public class ScrollLayoutBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {

        }
    }

    [Mix(typeof(ScrollLayoutBindings<Js.ScrollLayout>))]
    [ConstructorJs]
    public partial class ScrollLayout : Block {
        public required Block Content { get; set; }
        public ScrollLayout(ScrollLayout other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            Content = other.Content;
        }
        public ScrollLayout([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag) {            
            var content = Content.GenerateHtml(context,new Role(false, "Content"));
            elementTag.Add(content);
            base.ModifyHtml(context, elementTag);
        }

    }

}