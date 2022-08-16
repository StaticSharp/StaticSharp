using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {
    namespace Gears {
        [System.Diagnostics.DebuggerNonUserCode]
        public class BlockJs : HierarchicalJs {
            public BlockJs() { }
            public float X => throw new NotEvaluatableException();
            public float Y => throw new NotEvaluatableException();
            public float Width => throw new NotEvaluatableException();
            public float Height => throw new NotEvaluatableException();
            public float MarginLeft => throw new NotEvaluatableException();
            public float MarginRight => throw new NotEvaluatableException();
            public float MarginTop => throw new NotEvaluatableException();
            public float MarginBottom => throw new NotEvaluatableException();
            public float PaddingLeft => throw new NotEvaluatableException();
            public float PaddingRight => throw new NotEvaluatableException();
            public float PaddingTop => throw new NotEvaluatableException();
            public float PaddingBottom => throw new NotEvaluatableException();
        }
        public class BlockBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
            public BlockBindings(Dictionary<string, string> properties) : base(properties) {
            }
            public Expression<Func<FinalJs, float>> X { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> Y { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> Width { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> Height { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> MarginLeft { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> MarginRight { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> MarginTop { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> MarginBottom { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> PaddingLeft { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> PaddingRight { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> PaddingTop { set { AssignProperty(value); } }
            public Expression<Func<FinalJs, float>> PaddingBottom { set { AssignProperty(value); } }

        }

    }


    [RelatedScript]
    public partial class Block : Hierarchical, IBlock {
        public new BlockBindings<BlockJs> Bindings => new(Properties);

        protected Block(Block other,
            string callerFilePath = "",
            int callerLineNumber = 0) : base(other, callerFilePath, callerLineNumber) {
        }
        public Block([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

    }

}