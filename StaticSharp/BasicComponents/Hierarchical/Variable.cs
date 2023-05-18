using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    namespace Js {
        public class Variable<T> : Javascriptifier.IStringifiable where T: JEntity {

            [Javascriptifier.JavascriptPropertyGetFormat("{0}")]
            [Javascriptifier.JavascriptOnlyMember]
            public T Value => throw new Javascriptifier.JavascriptOnlyException();
            public string Name { get; set; }
            public Variable([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
                Name = "_"+Hash.CreateFromString($"{callerLineNumber}\0{callerFilePath}").ToString(8);
            }
            public string ToJavascriptString() {
                return Name;
            }
        }
    }

}

