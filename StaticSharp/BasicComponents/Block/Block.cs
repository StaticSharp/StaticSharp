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
            public float X => NotEvaluatableValue<float>();
            public float Y => NotEvaluatableValue<float>();
            public float Width => NotEvaluatableValue<float>();
            public float Height => NotEvaluatableValue<float>();
            public float MarginLeft => NotEvaluatableValue<float>();
            public float MarginRight => NotEvaluatableValue<float>();
            public float MarginTop => NotEvaluatableValue<float>();
            public float MarginBottom => NotEvaluatableValue<float>();
            public float PaddingLeft => NotEvaluatableValue<float>();
            public float PaddingRight => NotEvaluatableValue<float>();
            public float PaddingTop => NotEvaluatableValue<float>();
            public float PaddingBottom => NotEvaluatableValue<float>();
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

    public static partial class Static {
        public static T ConsumeParentHorizontalMargins<T>(this T _this) where T : Block {
            _this.Bindings.X = e => -e.ParentBlock.MarginLeft;
            _this.Bindings.Width = e => e.ParentBlock.Width + e.ParentBlock.MarginLeft + e.ParentBlock.MarginRight;
            _this.Bindings.PaddingLeft = e => e.ParentBlock.MarginLeft;
            _this.Bindings.PaddingRight = e => e.ParentBlock.MarginRight;
            return _this;
        }
    
    }


}