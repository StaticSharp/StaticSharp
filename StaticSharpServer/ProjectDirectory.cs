using System.Runtime.CompilerServices;

internal static class ProjectDirectory {
    private static string GetPath([CallerFilePath] string path = "") {
        return System.IO.Path.GetDirectoryName(path);
    }
    public static string Path => GetPath();
}