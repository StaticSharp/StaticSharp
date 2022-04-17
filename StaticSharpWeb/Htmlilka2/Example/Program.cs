using StaticSharp.Html;
using System.IO;
using System.Runtime.CompilerServices;



internal class Program {

    private static string ThisFilePath([CallerFilePath] string callerFilePath = "") {
        return callerFilePath;
    }

    private static void Main(string[] args) {

        var body = new Tag("body"){
                        new Tag("pre"){ "a"," = ", "B"},



                        new Tag("code", new {Class = "AAAA", src = "url"}),
                        "<Hello>",
                        new Tag("br"),
                        new Tag("p"){
                            "paragraph",
                            new Tag("br"),
                            "paragraph",
                        }
                    };

        body.Attributes["style"] = "";

        var result =
            new Tag(null) {
                new Tag("!DOCTYPE",new { html = "" }),

                new Tag("html", new { lang = "en"}) {
                    new Tag("head"){
                        new Tag("title") {
                            "HTMLilka 2"
                        },
                        new Tag("link", new{ sizes = "16x16", rel = "icon", href = "https://developers.antilatency.com/Favicon/CAF6A1BD8D2F40903F79A3B595E9AD62/CAF6A1BD8D2F40903F79A3B595E9AD6216.png"})
                            
                    },
                    string.Empty,
                    body,
                }
            };

        var resultPath = Path.Combine(Path.GetDirectoryName(ThisFilePath()), "Result.html");

        File.WriteAllText(resultPath, result.GetHtml());
    }
}
