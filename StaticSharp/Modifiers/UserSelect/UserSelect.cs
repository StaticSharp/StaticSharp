using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    public enum UserSelectOption {
        None,
        Text,
        All,
        Auto
    }

    public interface JUserSelect : JModifier {
        public UserSelectOption Option { get; set; }
    }


    [ConstructorJs]
    public partial class UserSelect : Modifier {
        public UserSelect(UserSelectOption option, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : this(callerLineNumber, callerFilePath) {
            Option = option;
        }
    }


}


