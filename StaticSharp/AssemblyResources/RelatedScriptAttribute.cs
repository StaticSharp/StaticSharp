using System;
using System.Runtime.CompilerServices;

namespace StaticSharp.Gears {

    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = true,
        Inherited = false)]
    public class RelatedScriptAttribute : RelatedFileAttribute {
        //public RelatedScriptAttribute([CallerFilePath] string callerFilePath = "") : base(null,callerFilePath) { }

        public RelatedScriptAttribute(string? fileName=null, [CallerFilePath] string callerFilePath = "") : base(fileName,callerFilePath) { }
        public override string Extension => ".js";
    }



}