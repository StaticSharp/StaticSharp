using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public interface JBackdropGrayscale : JAbstractBackdropFilter {
        double Amount { get; set; }
    }

    [ConstructorJs]
    public partial class BackdropGrayscale : AbstractBackdropFilter {
        
    }


}


