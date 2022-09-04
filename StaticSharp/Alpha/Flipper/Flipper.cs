using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        [System.Diagnostics.DebuggerNonUserCode]
        public class FlipperJs : BlockJs {
            //public float Before => throw new NotEvaluatableException();
        }



        public class FlipperBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
            public FlipperBindings(Dictionary<string, string> properties) : base(properties) {
            }
            //public Expression<Func<SpaceJs, float>> Before { set { AssignProperty(value); } }
        }
    }

    [RelatedScript]
    public sealed class Flipper : Block, IBlock {

        public new FlipperBindings<FlipperJs> Bindings => new(Properties);

        protected override string TagName => "flipper";

        public Block First { get; init; }
        public Block Second { get; init; }

        public Flipper([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag(null) {
                await First.GenerateHtmlAsync(context,"first"),
                await Second.GenerateHtmlAsync(context,"second"),
            };
        }
    }
}
