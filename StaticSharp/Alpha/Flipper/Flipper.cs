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
        public class FlipperJs : Block {
            //public float Before => throw new NotEvaluatableException();
        }



        public class MFlipperBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
        }
    }


    [Mix(typeof(MFlipperBindings<FlipperJs>))]
    [ConstructorJs]
    public sealed class Flipper : Block, IBlock {

        public Block First { get; init; }
        public Block Second { get; init; }

        public Flipper([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        protected override async ValueTask ModifyHtmlAsync(Context context, Tag elementTag) {
            elementTag.Add(                
                (await First.GenerateHtmlAsync(context)).AssignParentProperty("first")
            );
            elementTag.Add(
                (await Second.GenerateHtmlAsync(context)).AssignParentProperty("second")
            );
        }
    }
}
