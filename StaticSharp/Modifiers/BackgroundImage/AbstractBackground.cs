using StaticSharp.Gears;

namespace StaticSharp {



    public enum BackgroundImageRepeat {
        Repeat,     // The background image is repeated both vertically and horizontally
        RepeatX,    // The background image is repeated only horizontally
        RepeatY,    // The background image is repeated only vertically
        NoRepeat,   // The background-image is not repeated. The image will only be shown once
        Space,      // The background-image is repeated as much as possible without clipping. The first and last image is pinned to either side of the element, and whitespace is distributed evenly between the images
        Round       // The background-image is repeated and squished or stretched to fill the space (no gaps)
    }

    public enum BlendMode {
        Normal,
        Multiply,
        Screen,
        Overlay,
        Darken,
        Lighten,
        ColorDodge,
        ColorBurn,
        HardLight,
        SoftLight,
        Difference,
        Exclusion,
        Hue,
        Saturation,
        Color,
        Luminosity
    }


    public interface JAbstractBackground : JModifier {
        bool Enabled { get; set; }
        string RawImage { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Width { get; set; }
        double Height { get; set; }

        
        BackgroundImageRepeat Repeat { get; set; }
        BlendMode BlendMode { get; set; }
    }


    [ConstructorJs]
    public partial class AbstractBackground : Modifier {
    }

}
