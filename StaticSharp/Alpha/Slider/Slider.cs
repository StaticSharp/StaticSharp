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
        public interface Slider : Block {
            public double Min  { get; }
            public double Max  { get; }
            public double Step  { get; }
            public double Value  { get; }
            public double ValueActual  { get; }
        }
    }

    namespace Gears {
        public class SliderBindings<FinalJs> : BlockBindings<FinalJs> {
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
            BackgroundColor = new(e => e.Hover ? new Color(0, 0, 0, 0.5) : new Color(0, 0, 0, 0.25)),
            ["Radius"] = "() => Min(element.Width,element.Height) / 2"
        };

        public Block? Thumb { get; set; } = null;
        public Slider(Slider other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) { }

        public Slider([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag) {
            var thumb = Thumb;
            if (thumb == null) {
                thumb = DefaultThumbConstructor();
            }

            elementTag.Add(
                thumb.GenerateHtml(context, new Role(false,"Thumb"))
                );
            base.ModifyHtml(context, elementTag);
        }

    }

}