using System.IO;
using System.Runtime.CompilerServices;

namespace StaticSharpWeb {
    public static partial class Static {

        public static string AbsolutePath(string subPath = "", [CallerFilePath] string callerFilePath = "") => 
            Path.Combine(Path.GetDirectoryName(callerFilePath), subPath);



    }
}