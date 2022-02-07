using System.IO;
using System.Runtime.CompilerServices;

namespace StaticSharpWeb {
    public class AbsolutePath {
        string Value { get; init; }
        public AbsolutePath(string subPath = "", [CallerFilePath] string callerFilePath = "") => 
            Value = Path.Combine(Path.GetDirectoryName(callerFilePath), subPath);

        public static implicit operator string(AbsolutePath relativePath) => relativePath.Value;

        public override string ToString() => Value;

    }
}