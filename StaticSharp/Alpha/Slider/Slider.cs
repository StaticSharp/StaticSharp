using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public class Slider : Block {
            public float Min => NotEvaluatableValue<float>();
            public float Max => NotEvaluatableValue<float>();
            public float Step => NotEvaluatableValue<float>();
            public float Value => NotEvaluatableValue<float>();
            public float ValueActual => NotEvaluatableValue<float>();
        }
    }

    namespace Gears {
        public class SliderBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
            public Binding<float> Min { set { Apply(value); } }
            public Binding<float> Max { set { Apply(value); } }
            public Binding<float> Step { set { Apply(value); } }
            public Binding<float> Value { set { Apply(value); } }
        }
    }


    [Mix(typeof(SliderBindings<Js.Slider>))]
    [ConstructorJs]
    public partial class Slider : Block {

        protected override string TagName => "slider";
        public Slider(Slider other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) { }

        public Slider([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            context.Includes.Require(new Style(AbsolutePath("Slider.scss")));

            return Task.FromResult<Tag?>(new Tag("input") {
                    ["type"] = "range"
                });
        }

    }

}