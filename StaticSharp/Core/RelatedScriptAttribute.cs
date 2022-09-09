using System;
using System.Runtime.CompilerServices;

namespace StaticSharp.Gears {

    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = true,
        Inherited = false)]


    public class RelatedScriptAttribute : RelatedFileAttribute {
        public RelatedScriptAttribute(string? fileName = null, [CallerFilePath] string callerFilePath = "") : base(fileName, callerFilePath) { }
        public override string Extension => ".js";
    }


    public class ConstructorJsAttribute : RelatedScriptAttribute {
        public ConstructorJsAttribute(string? fileName = null, [CallerFilePath] string callerFilePath = "") : base(fileName, callerFilePath) { }

    }


    /*public class ConstructorJs: RelatedScriptAttribute {
        public ConstructorJs(string? fileName = null, [CallerFilePath] string callerFilePath = "") : base(fileName, callerFilePath) { }
    }*/



}