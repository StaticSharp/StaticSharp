using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {




    public interface JToggle : JModifier {
        public bool Value { get; }
    }

    namespace Js {

        [Javascriptifier.JavascriptClass("")]
        public static partial class CastExtensions {
            [Javascriptifier.JavascriptOnlyMember]
            [Javascriptifier.JavascriptMethodFormat("as({0},\"Toggle\")")]
            public static JToggle AsToggle(this JEntity _this) => throw new Javascriptifier.JavascriptOnlyException();
        }

    }

    namespace Gears {
        public class ToggleBindings<FinalJs> : ModifierBindings<FinalJs> {
            public Binding<bool> Value { set { Apply(value); } }
        }
    }



    [ConstructorJs]
    [Mix(typeof(HoverBindings<JToggle>))]
    public partial class Toggle : Modifier {
        public Toggle([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
        }

        public Toggle(Toggle other, int callerLineNumber = 0, string callerFilePath = "") : base(other, callerLineNumber, callerFilePath) {
        }
    }

    


}


