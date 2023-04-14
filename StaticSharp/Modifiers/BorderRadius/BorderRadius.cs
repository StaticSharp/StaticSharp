using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {
    public interface JBorderRadius : JModifier {
        public double Radius { get; set; }
        public double RadiusTopLeft { get; set; }
        public double RadiusTopRight { get; set; }
        public double RadiusBottomLeft { get; set; }
        public double RadiusBottomRight { get; set; }
    }


    [ConstructorJs]
    public partial class BorderRadius : Modifier {

    }


}


