using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public class Slider : Block {
            public double Min => NotEvaluatableValue<double>();
            public double Max => NotEvaluatableValue<double>();
            public double Step => NotEvaluatableValue<double>();
            public double Value => NotEvaluatableValue<double>();
            public double ValueActual => NotEvaluatableValue<double>();
        }
    }

    namespace Gears {
        public class SliderBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
            public Binding<double> Min { set { Apply(value); } }
            public Binding<double> Max { set { Apply(value); } }
            public Binding<double> Step { set { Apply(value); } }
            public Binding<double> Value { set { Apply(value); } }
        }
    }


    [Mix(typeof(SliderBindings<Js.Slider>))]
    [ConstructorJs]
    public partial class Slider : Block {

        public static Func<Block> DefaultThumbConstructor = () => new Block() {
            BackgroundColor = new(e=>e.Hover ? Color.FromArgb(128, 0, 0, 0) : Color.FromArgb(64, 0, 0, 0)),            
            ["Radius"] = "() => Min(element.Width,element.Height) / 2"
        };

        public Block? Thumb { get; set; } = null;
        public Slider(Slider other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) { }

        public Slider([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {
            var thumb = Thumb;
            if (thumb == null) {
                thumb = DefaultThumbConstructor();
            }

            elementTag.Add(
                await thumb.GenerateHtmlAsync(context, new Role(false,"Thumb"))
                );
            await base.ModifyHtmlAsync(context, elementTag);
        }

    }

}