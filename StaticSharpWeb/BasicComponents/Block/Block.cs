using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    public class BlockJs : HierarchicalJs {
        public BlockJs() { }
        public BlockJs(string value) : base(value) {
        }

        public NumberJs X => new($"{value}.X");
        public NumberJs Y => new($"{value}.Y");
        public NumberJs Width => new($"{value}.Width");
        public NumberJs Height => new($"{value}.Height");
        public BorderJs Margin => new($"{value}.Margin");

    }
    

    namespace Gears {

        [ScriptBefore][ScriptAfter]
        public abstract class Block<Js> : Hierarchical<Js>, IBlock where Js : BlockJs, new() {
            
            public Binding<NumberJs> X { set; protected get; } = null!;
            public Binding<NumberJs> Y { set; protected get; } = null!;

            public Binding<NumberJs> Width { set; protected get; } = null!;
            public Binding<NumberJs> Height { set; protected get; } = null!;

            //public Border<Js> Padding { set; protected get; } = null!;
            public Border<Js> Margin { set; protected get; } = null!;

            public Block(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

            public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(ThisFilePathWithNewExtension("js")));
            }

            


            public abstract Task<Tag> GenerateHtmlAsync(Context context);

        }
    }

    /*public sealed class Item : Item<Symbolic.Item> {
        public Item([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }
    }*/
}