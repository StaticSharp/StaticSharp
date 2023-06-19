using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public interface JDebugModifier : JModifier {
        
    }

    [ConstructorJs]
    public partial class DebugModifier : Modifier {

        public override string? TagRename => "dialog";

        protected override Context ModifyContext(Context context) {
            return base.ModifyContext(context);
        }

        
    }


}


