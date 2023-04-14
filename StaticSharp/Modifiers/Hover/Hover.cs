using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    public interface JHover : JModifier {
        public bool Value { get; }
    }


    [ConstructorJs]
    public partial class Hover : Modifier {

    }


}


