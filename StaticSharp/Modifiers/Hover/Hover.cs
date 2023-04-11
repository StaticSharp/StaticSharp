using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    namespace Js {
        public interface Hover {
            public bool Value{ get; }
        }
    }
    namespace Gears {
        public class HoverBindings<FinalJs> : ModifierBindings<FinalJs> {
        }
    }



    [ConstructorJs]
    [Mix(typeof(HoverBindings<Js.Hover>))]
    public partial class Hover : Modifier {
        public Hover([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
        }

        public Hover(Hover other, int callerLineNumber = 0, string callerFilePath = "") : base(other, callerLineNumber, callerFilePath) {
        }
    }


}


