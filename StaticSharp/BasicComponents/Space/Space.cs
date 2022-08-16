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
        public class SpaceJs : HierarchicalJs {
            public float Before => throw new NotEvaluatableException();
            public float Between => throw new NotEvaluatableException();
            public float After => throw new NotEvaluatableException();
        }



        public class SpaceBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
            public SpaceBindings(Dictionary<string, string> properties) : base(properties) {
            }
            public Expression<Func<SpaceJs, float>> Before { set { AssignProperty(value); } }
            public Expression<Func<SpaceJs, float>> Between { set { AssignProperty(value); } }
            public Expression<Func<SpaceJs, float>> After { set { AssignProperty(value); } }
        }
    }

    [RelatedScript]
    public sealed class Space : Hierarchical, IBlock {

        public new SpaceBindings<HierarchicalJs> Bindings => new(Properties);

        public override string TagName => "ws";
        public Space([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        public Space(float before, float between = 1, float after = 0, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
            if (before != 0) Bindings.Before = e => before;
            if (between != 1) Bindings.Between = e => between;
            if (after != 0) Bindings.After = e => after;
        }

        protected override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return Task.FromResult<Tag?>(null);
        }
    }
}
