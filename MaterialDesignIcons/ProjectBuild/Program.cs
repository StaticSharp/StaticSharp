using Scopes.C;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

internal static class ProjectDirectory {
    private static string GetPath([CallerFilePath] string path = "") {
        return System.IO.Path.GetDirectoryName(path);
    }
    public static string Path => GetPath();
}


public struct MetaElement {
    //public string id;
    public string name;
    public string codepoint;
    private static Regex KebabToPascalCaseRegex = new Regex("(?:-|^)[a-zA-Z0-9]", RegexOptions.Compiled);
    public string PascalName {
        get {
            return KebabToPascalCaseRegex.Replace(name, x => char.ToUpper(x.Value.Last()).ToString());
        }
    }
    public string EnumField => $"{PascalName} = 0x{codepoint},";
}


public class Program {

    private static Regex KebabToPascalCaseRegex = new Regex("(?:-|^)[a-zA-Z0-9]", RegexOptions.Compiled);

    public static string KebabToPascalName(string name) {
        return KebabToPascalCaseRegex.Replace(name, x => char.ToUpper(x.Value.Last()).ToString());        
    }

    public static HttpClient HttpClient { get; private set; } = new HttpClient();

    public static async Task Main(string[] args) {

        var sourceDirectory = Path.GetFullPath(Path.Combine(ProjectDirectory.Path, "../Source"));

        var csFilePath = Path.Combine(sourceDirectory, "IconName.cs");


        var zipUrl = "https://github.com/Templarian/MaterialDesign/archive/refs/heads/master.zip";

        var zipResponse = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, zipUrl));

        var zipStream = zipResponse.Content.ReadAsStream();

        var iconEnum = new Scope("public enum IconName") {

        };

        using (ZipArchive archive = new ZipArchive(zipStream)) {
            foreach (ZipArchiveEntry entry in archive.Entries.Where(x=>x.FullName.StartsWith("MaterialDesign-master/svg/") && x.Name.EndsWith(".svg"))) {
                var fileName = KebabToPascalName(entry.Name);
                var filePath = Path.Combine(sourceDirectory, "svg", fileName);

                if (!File.Exists(filePath)) {
                    Console.WriteLine(fileName);
                    entry.ExtractToFile(filePath);
                }
                iconEnum.Add(Path.GetFileNameWithoutExtension(fileName)+",");                
            }
        }

        var csFileContent = new Scope("namespace MaterialDesignIcons") {
            iconEnum
        };
        File.WriteAllText(csFilePath, csFileContent.ToString());

    }
}