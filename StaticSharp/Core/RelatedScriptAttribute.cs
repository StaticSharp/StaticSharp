using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace StaticSharp.Gears {


    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = true,
        Inherited = false)]
    public class RelatedScriptAttribute : RelatedFileAttribute {

        const string Extension = ".js";
        public RelatedScriptAttribute(string filePathOrExtension = Extension, [CallerFilePath] string callerFilePath = "")
            : base(Path.HasExtension(filePathOrExtension)? filePathOrExtension: filePathOrExtension+ Extension, callerFilePath) { }
    
    }

    [AttributeUsage(AttributeTargets.Class,
       AllowMultiple = true,
       Inherited = false)]
    public class RelatedStyleAttribute : RelatedFileAttribute {
        
        const string Extension = ".css";
        public RelatedStyleAttribute(string filePathOrExtension = Extension, [CallerFilePath] string callerFilePath = "")
            : base(Path.HasExtension(filePathOrExtension) ? filePathOrExtension : filePathOrExtension + Extension, callerFilePath) { }
    }


    public class ConstructorJsAttribute : RelatedScriptAttribute {
        public string TypeFullName { get; }
        public ConstructorJsAttribute(string typeFullName = "", [CallerFilePath] string callerFilePath = "") : base(typeFullName + ".js", callerFilePath){
            TypeFullName = typeFullName;
        }        
    }


}