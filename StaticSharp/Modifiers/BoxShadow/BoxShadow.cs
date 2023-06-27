using StaticSharp.Gears;

namespace StaticSharp {

    public interface JBoxShadow : JAbstractBoxShadow {
        public bool Inset { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Spread { get; set; }
        public double Blur { get; set; }
        public Color Color { get; set; }
    }


    [ConstructorJs]
    public partial class BoxShadow : AbstractBoxShadow {

    }


}


