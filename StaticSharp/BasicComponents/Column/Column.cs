using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    [ConstructorJs]
    public class Column : Block {
        //protected override string TagName => "column";        

        public Column(Column other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
                  
        }
        public Column([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }
    }
}