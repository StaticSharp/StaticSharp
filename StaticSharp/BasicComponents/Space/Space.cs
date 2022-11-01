using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public class Space : Hierarchical {
            public double Before => NotEvaluatableValue<double>();
            public double Between => NotEvaluatableValue<double>();
            public double After => NotEvaluatableValue<double>();
        }
    }


    namespace Gears {
        public class SpaceBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
            public Binding<double> Before { set { Apply(value); } }
            public Binding<double> Between { set { Apply(value); } }
            public Binding<double> After { set { Apply(value); } }
        }
    }

    [Mix(typeof(SpaceBindings<Js.Space>))]
    [ConstructorJs]
    public sealed partial class Space : Hierarchical, IBlock {
        protected override string TagName => "ws";
        /*public Space([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }*/

        public Space(double before = 1, double between = 1, double after = 1, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
            if (before != 1) Before = before;
            if (between != 1) Between = between;
            if (after != 1) After = after;
        }

        /*public async Task<Tag> GenerateHtmlAsync(Context context) {
            await AddRequiredInclues(context);
            var tag = new Tag(TagName) {
                await CreateConstructorScriptAsync(context)
            };
            AddSourceCodeNavigationData(tag,context);

            return tag;
        }*/

        /*public async Task<Node> GenerateConstructor(Context context, string? id) {
            return "";
        }*/
    }
}
