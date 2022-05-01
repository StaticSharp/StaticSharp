using System.IO;
using System.Runtime.CompilerServices;

namespace StaticSharpWeb {
    public static partial class Static {
        public static string AbsolutePath(string subPath = "", [CallerFilePath] string callerFilePath = "") {
            var path = Path.GetFullPath(subPath, Path.GetDirectoryName(callerFilePath));
            return path;

        }
    }
}