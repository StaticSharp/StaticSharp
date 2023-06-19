using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public interface JModal : JModifier {
        
    }

    [ConstructorJs]
    public partial class Modal : Modifier {

        public override string? TagRename => "dialog";


        
    }


}


