
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;


namespace StaticSharp;


public interface JFlipper : JBlock {
    JBlock First { get; }
    JBlock Second { get; }
    bool Vertical { get; set; }

    bool Reverse { get; set; }
    double Proportion { get; set; }

    double Gap { get; set; }

    double MinContactMargin { get; }
    double MaxContactMargin { get; }

    double InternalWidth { get; }
    double InternalHeight { get; }
}

[ConstructorJs]
public partial class Flipper : Block {
    [Socket]
    public required Block First { get; init; }
    [Socket]
    public required Block Second { get; init; }

    
}