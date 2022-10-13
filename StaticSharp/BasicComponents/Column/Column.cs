using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    [ConstructorJs]
    public class Column : Block {
        protected override string TagName => "column";
        

        public Column(Column other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
                  
        }
        public Column([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }


    }

}