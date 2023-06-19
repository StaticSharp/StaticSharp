using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public interface JBackdropBlur : JAbstractBackdropFilter {
        double Radius { get; set; }
    }

    [ConstructorJs]
    public partial class BackdropBlur : AbstractBackdropFilter {
        
    }


}


