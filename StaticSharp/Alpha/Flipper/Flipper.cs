using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public class Flipper : Block {
            public double FlipWidth => NotEvaluatableValue<double>();
        }
    }


    namespace Gears {
        public class FlipperBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
            public Binding<double> FlipWidth { set { Apply(value); } }
        }
    }


    [Mix(typeof(FlipperBindings<Js.Flipper>))]
    [ConstructorJs]
    public partial class Flipper : Block, IBlock {

        public required Block First { get; init; }
        public required Block Second { get; init; }

        public Flipper([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag) {  
            elementTag.Add(                
                First.GenerateHtml(context,new Role(false,"First"))
            );
            elementTag.Add(
                Second.GenerateHtml(context, new Role(false, "Second"))
            );
            base.ModifyHtml(context, elementTag);
        }
    }
}
