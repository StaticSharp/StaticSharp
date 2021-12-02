using CsmlWeb.Components;
using CsmlWeb.Html;

public static class ApiStatic {
    public static INode Keyword(string x) => new Tag("span", new { Class = "Keyword" }) { x };
    //<span clas="Keyword">{x}</span>
    public static INode Interface => Keyword("interface");
    public static INode Struct => Keyword("struct");
    public static INode Enum => Keyword("enum");

    public static INode Namespace(string name) => new Tag("span", new { Class = "Namespace" }) { name };
    public static INode Type(string name) => new Tag("span", new { Class = "Type" }) { name };
    public static INode Method(INode reference) => new Tag("span", new { Class = "Method" }) { reference };

    public static INode CodeBlock(INode reference) => new Tag("code", new { Class = "CodeBlock" }) { reference };

    public static INode CodeInline(INode reference) => new Tag("code", new { Class = "CodeInline" }) { reference };

    public static readonly string Nbsp = "\x00a0";

    public static readonly string Wbr = "\x200b";

    public static INode CodeIndentation(INode content) => new Tag("div", new { Class = "CodeIdented" }) { content };
}