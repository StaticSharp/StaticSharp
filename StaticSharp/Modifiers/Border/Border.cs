using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public struct Sides : Javascriptifier.IStringifiable {
        public static readonly Sides Top = 1;
        public static readonly Sides Right = 2;
        public static readonly Sides Bottom = 4;
        public static readonly Sides Left = 8;
        public static readonly Sides All = 15;

        private int value;

        public Sides(int value) {
            this.value = value;
        }

        public static Sides operator | (Sides left, Sides right) {
            return new Sides(left.value | right.value);
        }

        public static implicit operator Sides(int value) {
            return new Sides(value);
        }

        public string ToJavascriptString() {
            return Javascriptifier.ValueStringifier.Stringify(value);
        }
    }

    public enum BorderStyle {
        None,
        Dotted,
        Dashed,
        Solid,
        Double
    }

    public interface JBorder : JModifier {

        public Sides Sides { get; set; }

        public BorderStyle Style { get; set; }
        public Color Color { get; set; }
        public double Width { get; set; }

        /*public double Radius { get; set; }
        public double RadiusTopLeft { get; set; }
        public double RadiusTopRight { get; set; }
        public double RadiusBottomLeft { get; set; }
        public double RadiusBottomRight { get; set; }*/
    }


    [ConstructorJs]
    public partial class Border : Modifier {

    }


}


