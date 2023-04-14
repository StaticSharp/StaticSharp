using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    public interface JSlider : JBlock {
        public double Min { get; }
        public double Max { get; }
        public double Step { get; }
        public double Value { get; }
        public double ValueActual { get; }
    }




    [ConstructorJs]
    public partial class Slider : Block {

        public static Func<Block> DefaultThumbConstructor = () => new Block() {
            BackgroundColor = new(e => e.Hover ? new Color(0, 0, 0, 0.5) : new Color(0, 0, 0, 0.25)),
            ["Radius"] = "() => Min(element.Width,element.Height) / 2"
        };

        public Block? Thumb { get; set; } = null;

    }

}