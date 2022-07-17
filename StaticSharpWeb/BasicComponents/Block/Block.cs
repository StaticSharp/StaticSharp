using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    public static class W {
        public static T MakeNotEvaluatable<T>(this T _this) {
            return _this;
        }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public class BlockJs : HierarchicalJs {
        public BlockJs() { }
        public float X =>             throw new NotEvaluatableException();
        public float Y =>             throw new NotEvaluatableException();
        public float Width =>         throw new NotEvaluatableException();
        public float Height =>        throw new NotEvaluatableException();
        public float MarginLeft =>    throw new NotEvaluatableException();
        public float MarginRight =>   throw new NotEvaluatableException();
        public float MarginTop =>     throw new NotEvaluatableException();
        public float MarginBottom =>  throw new NotEvaluatableException();
        public float PaddingLeft =>   throw new NotEvaluatableException();
        public float PaddingRight =>  throw new NotEvaluatableException();
        public float PaddingTop =>    throw new NotEvaluatableException();
        public float PaddingBottom => throw new NotEvaluatableException();
    }
    

    namespace Gears {

        public class BlockList : List<KeyValuePair<string?, IBlock>> {
            public void Add(IBlock block, string? id) {
                Add(new KeyValuePair<string?, IBlock>(id, block));
            }
        }


        [ScriptBefore]
        [ScriptAfter]
        public abstract class Block<Js> : Hierarchical<Js>, IBlock where Js : BlockJs, new() {



            public Binding<float> X { set; protected get; }
            public Binding<float> Y { set; protected get; }
            public Binding<float> Width { set; protected get; }
            public Binding<float> Height { set; protected get; }
            public Binding<float> MarginLeft { set; protected get; }
            public Binding<float> MarginRight { set; protected get; }
            public Binding<float> MarginTop { set; protected get; }
            public Binding<float> MarginBottom { set; protected get; }
            public Binding<float> PaddingLeft { set; protected get; }
            public Binding<float> PaddingRight { set; protected get; }
            public Binding<float> PaddingTop { set; protected get; }
            public Binding<float> PaddingBottom { set; protected get; }

            /*protected Block(Block<Js> other,
                string callerFilePath = "",
                int callerLineNumber = 0) { 
                
            }*/
            public Block(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

            public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(ThisFilePathWithNewExtension("js")));
            }

            

        }
    }

    /*public sealed class Item : Item<Symbolic.Item> {
        public Item([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }
    }*/
}