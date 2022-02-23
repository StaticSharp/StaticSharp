
namespace StaticSharpWeb;

public static class Anchors {

    public static string FillTextAnchorsJsPath => AbsolutePath("FillTextAnchors.js");
    public static string ReduceFontSizeOnOverflowJsPath => AbsolutePath("ReduceFontSizeOnOverflow.js");
    public static string AbsolutePath(string subPath = "") {
        return Static.AbsolutePath(subPath);
    }
}