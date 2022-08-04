using System.IO;
using System.Runtime.CompilerServices;


namespace StaticSharp {
}

namespace StaticSharp {
    public static partial class Static {

        public static string ThisFileNameWithoutExtension([CallerFilePath] string callerFilePath = "") {
            return Path.GetFileNameWithoutExtension(callerFilePath);
        }

        public static string ThisFilePathWithNewExtension(string extension = "", [CallerFilePath] string callerFilePath = "") {
            var indexOfDot = callerFilePath.LastIndexOf('.');
            if (indexOfDot == -1)
                return callerFilePath;
            var path = callerFilePath[..(indexOfDot+1)] + extension;
            return path;
        }

        public static string AbsolutePath(string subPath = "", [CallerFilePath] string callerFilePath = "") {
            var path = Path.GetFullPath(subPath, Path.GetDirectoryName(callerFilePath));
            return path;

        }
    }
}