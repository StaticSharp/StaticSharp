using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;


namespace StaticSharp {
}

namespace StaticSharp {
    public static partial class Static {

        static Static() {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        /*public static string ToInvariant(this float value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }
        public static string ToInvariant(this double value) {
            return value.ToString(CultureInfo.CurrentCulture);
        }*/

        public static string ThisFilePath([CallerFilePath] string callerFilePath = "") {
            return callerFilePath;
        }

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

        public static string MakeAbsolutePath(string subPath = "", [CallerFilePath] string callerFilePath = "") {
            var path = Path.GetFullPath(subPath, Path.GetDirectoryName(callerFilePath));
            return path;

        }
    }
}