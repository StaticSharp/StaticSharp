using System.Runtime.CompilerServices;

internal static class ProjectDirectory {
    public static string GetPath([CallerFilePath] string path = "") {
        return System.IO.Path.GetDirectoryName(path);
    }
    internal static string Path => GetPath();
}