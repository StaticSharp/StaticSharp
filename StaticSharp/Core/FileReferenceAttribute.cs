using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp.Gears {


    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = true,
        Inherited = false)]
    public abstract class FileReferenceAttribute : Attribute {

        private string filePath;
        public static string GetThisFilePath(string? extensionReplacement = null, [CallerFilePath] string callerFilePath = "") {
            if (extensionReplacement == null) {
                return callerFilePath;
            }
            return Path.ChangeExtension(callerFilePath, extensionReplacement);
        }
        public FileReferenceAttribute(string filePath) {
            this.filePath = filePath;
        }

        public Genome<IAsset> GetGenome() {
            var type = GetType();
            var assembly = type.Assembly;

            return new FileOfAssemblyResourceGenome(assembly, filePath).Result;
        }
    }
}


namespace StaticSharp.Scripts {
    public abstract class ScriptReferenceAttribute : FileReferenceAttribute {
        const string Extension = ".js";

        protected ScriptReferenceAttribute(string filePath) : base(filePath) {}

        public static string GetScriptFilePath([CallerFilePath] string callerFilePath = "") {
            return GetThisFilePath(Extension, callerFilePath);
        }
    }

}