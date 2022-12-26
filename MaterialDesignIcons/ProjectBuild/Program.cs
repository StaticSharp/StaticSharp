using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scopes.C;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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



        await SimpleIcons(sourceDirectory);
        await MakeMaterialDesignIcons(sourceDirectory);

    }
    static async Task SimpleIcons(string sourceDirectory) {
        var csFilePath = Path.Combine(sourceDirectory, "SimpleIcons.cs");

        var zipUrl = "https://github.com/simple-icons/simple-icons/archive/refs/heads/master.zip";

        var zipResponse = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, zipUrl));

        var zipStream = zipResponse.Content.ReadAsStream();

        var iconClass = new Scope("public static class SimpleIcons") {

        };


        KeyValuePair<string,string> TitleToIdentifierSlig(string title) {
            if (title == "del.icio.us") return new("DelIcioUs", "delicious");
            if (title == "Ferrari N.V.") return new("FerrariNV", "ferrarinv");
            if (title == "Sat.1") return new("Sat1", "sat1");
            if (title == "Warner Bros.") return new("WarnerBros", "warnerbros");

            
            title = title.Replace("+", "Plus");
            title = title.Replace("&", "And");

            title = title.Replace("/", "");
            
            //title = title.Replace("-", "");
            
            title = title.Replace("'", "");
            title = title.Replace("!", "");
            
            title = title.Replace("°", "");

            title = title.Replace(" ", "-");
            title = title.Replace("_", "-");
            title = title.Replace(":", "-");


            var identifier = title;
            var sig = title;

            identifier = identifier.Replace(".", "-");

            identifier = KebabToPascalName(identifier);
            if (char.IsDigit(identifier[0])) {
                identifier = "_" + identifier;
            }

            if (identifier == "SimpleIcons") {
                identifier = "_SimpleIcons";
            }


            sig = sig.Replace(".", "dot");
            sig = sig.Replace("-", "");
            sig = sig.ToLower();

            sig = sig.Replace("é", "e");
            sig = sig.Replace("ë", "e");
            sig = sig.Replace("è", "e");
            sig = sig.Replace("ü", "u");
            sig = sig.Replace("ã", "a");
            sig = sig.Replace("š", "s");
            sig = sig.Replace("ż", "z");


            return new (identifier,sig);
        }


        using (ZipArchive archive = new ZipArchive(zipStream)) {

            var jsonEntry = archive.Entries.First(x => x.FullName == "simple-icons-master/_data/simple-icons.json");
            var jsonStream = jsonEntry.Open();

            
            Dictionary<string, string> identifierSlig = new();



            List<string> titles = new();
            
            
            var files = archive.Entries.Where(x => x.FullName.StartsWith("simple-icons-master/icons/") && x.Name.EndsWith(".svg")).ToDictionary(x => Path.GetFileNameWithoutExtension(x.FullName));
            var fileNames = files.Keys.ToList();


            using (StreamReader reader = new StreamReader(jsonStream, Encoding.UTF8)) {
                var jsonText = reader.ReadToEnd();
                dynamic jsonIconDescriptions = JObject.Parse(jsonText);
                var array = jsonIconDescriptions.icons;

                foreach ( var icon in array ) {
                    var title = icon.title;
                    var item = TitleToIdentifierSlig(title);

                    if (identifierSlig.ContainsKey(item.Key)) {
                        
                    } else {
                        identifierSlig.Add(item.Key, item.Value);
                        
                        if (!fileNames.Contains(item.Value)) {
                            titles.Add(title);
                        } else {
                            fileNames.Remove(item.Value);
                        }

                    }
                }

                foreach (var f in fileNames) {
                    identifierSlig.Add(KebabToPascalName(f.Replace('_','-')), f);
                }

            }


            foreach (var i in identifierSlig) {
                var entry = files[i.Value];
                var filePath = Path.Combine(sourceDirectory, "svg", "SimpleIcons", entry.Name);
                
                if (!File.Exists(filePath)) {
                    entry.ExtractToFile(filePath);
                }

                float width = 24;
                float height = 24;

                var svg = File.ReadAllText(filePath);
                GetSize(svg, out width, out height);

                if (width != 24 || height != 24)
                    throw new Exception();


                iconClass.Add($"public static Icon {i.Key} => new Icon(\"SimpleIcons.{i.Value}\",{width}, {height});");

            }



                /*foreach (ZipArchiveEntry entry in archive.Entries.Where(x => x.FullName.StartsWith("simple-icons-master/icons/") && x.Name.EndsWith(".svg"))) {
                var fileName = KebabToPascalName(entry.Name);
                var filePath = Path.Combine(sourceDirectory, "svg", "MaterialDesignIcons", fileName);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);*/
                

                /*var svg = File.ReadAllText(filePath);
                GetSize(svg, out width, out height);

                if (width != 24 || height != 24)
                    throw new Exception();

                
            }*/
        }

        var csFileContent = new Scope("namespace Icons") {
            iconClass
        };
        File.WriteAllText(csFilePath, csFileContent.ToString());
    }

    static async Task MakeMaterialDesignIcons(string sourceDirectory) {
        var csFilePath = Path.Combine(sourceDirectory, "MaterialDesignIcons.cs");

        var zipUrl = "https://github.com/Templarian/MaterialDesign/archive/refs/heads/master.zip";

        var zipResponse = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, zipUrl));

        var zipStream = zipResponse.Content.ReadAsStream();

        var iconClass = new Scope("public static class MaterialDesignIcons") {

        };

        using (ZipArchive archive = new ZipArchive(zipStream)) {
            foreach (ZipArchiveEntry entry in archive.Entries.Where(x => x.FullName.StartsWith("MaterialDesign-master/svg/") && x.Name.EndsWith(".svg"))) {
                var fileName = KebabToPascalName(entry.Name);
                var filePath = Path.Combine(sourceDirectory, "svg", "MaterialDesignIcons", fileName);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                if (!File.Exists(filePath)) {
                    Console.WriteLine(fileName);
                    entry.ExtractToFile(filePath);
                }

                float width = 24;
                float height = 24;

                /*var svg = File.ReadAllText(filePath);
                GetSize(svg, out width, out height);

                if (width != 24 || height != 24)
                    throw new Exception();*/

                iconClass.Add($"public static Icon {fileNameWithoutExtension} => new Icon(\"MaterialDesignIcons.{fileNameWithoutExtension}\",{width}, {height});");
            }
        }

        var csFileContent = new Scope("namespace Icons") {
            iconClass
        };
        File.WriteAllText(csFilePath, csFileContent.ToString());
    }




    public static void GetSize(string svg, out float width, out float height) {
        var document = XDocument.Parse(svg);
        var svgElement = document.Elements().FirstOrDefault(x => x.Name.LocalName == "svg");

        if (svgElement == null)
            throw new Exception("Invalid svg file");

        svgElement.Attribute("id")?.Remove();
        try {
            var viewBox = svgElement.Attribute("viewBox").Value.Split(' ').Select(float.Parse).ToArray();
            width = viewBox[2];
            height = viewBox[3];
        }
        catch {
            throw new Exception("Invalid svg file");
        }
    }




}