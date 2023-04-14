using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {
    public interface JToggle : JModifier {
        public bool Value { get; set; }
    }


    [ConstructorJs]
    public partial class Toggle : Modifier {
    }

    


}


