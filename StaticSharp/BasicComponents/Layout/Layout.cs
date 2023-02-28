using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    [ConstructorJs]
    public class Layout : Block {
        public Layout(Layout other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
        }
        public Layout([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }
    }
}