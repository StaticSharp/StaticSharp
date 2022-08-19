using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [System.Diagnostics.DebuggerNonUserCode]
    public class SliderJs : BlockJs {
        public float Min =>     NotEvaluatableValue<float>();
        public float Max =>     NotEvaluatableValue<float>();
        public float Step =>    NotEvaluatableValue<float>();
        public float Value =>   NotEvaluatableValue<float>();
    }


    public class SliderBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
        public SliderBindings(Dictionary<string, string> properties) : base(properties) {}

        public Expression<Func<FinalJs, float>> Min { set { AssignProperty(value); } }
        public Expression<Func<FinalJs, float>> Max { set { AssignProperty(value); } }
        public Expression<Func<FinalJs, float>> Step { set { AssignProperty(value); } }
        public Expression<Func<FinalJs, float>> Value { set { AssignProperty(value); } }
    }





    [RelatedScript]
    public class Slider : Block {

        public new SliderBindings<SliderJs> Bindings => new(Properties);
        public override string TagName => "slider";

        


        public Slider(Slider other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) { }

        public Slider([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        /*public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }*/

        protected override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            context.Includes.Require(new Style(AbsolutePath("Slider.scss")));

            return Task.FromResult<Tag?>(new Tag("input") {
                    ["type"] = "range"
                });
        }

    }

    
    /*public sealed class Slider : Slider<SliderJs> {
        

        public Slider(Slider other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {

            Min = other.Min;
            Max = other.Max;
            Step = other.Step;
            Value = other.Value;
        }
        public Slider([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        

        
    }*/
}