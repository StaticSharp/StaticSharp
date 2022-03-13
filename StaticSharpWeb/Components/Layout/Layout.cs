
namespace StaticSharpWeb;

public static class Layout {

    public static string ColumnJsPath => AbsolutePath("Column.js");

    public static string TextJsPath => AbsolutePath("Text.js");
    public static string ReduceFontSizeOnOverflowJsPath => AbsolutePath("ReduceFontSizeOnOverflow.js");
    public static string AbsolutePath(string subPath = "") {
        return Static.AbsolutePath(subPath);
    }
}