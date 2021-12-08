using StaticSharpWeb.Html;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System;

namespace StaticSharpWeb {

    public interface IIncludes {

        void Require(IScript script);

        void Require(IStyle style);

        void Require(IFont font);

        Task<Tag> GenerateScriptAsync(IStorage storage);

        Task<Tag> GenerateFontAsync(IStorage storage);
        Task<Tag> GenerateStyleAsync(IStorage storage);
    }

    public class Includes : IIncludes {
        private readonly ConcurrentDictionary<string, IScript> scripts = new();
        private readonly ConcurrentDictionary<string, IStyle> styles = new();
        private readonly ConcurrentDictionary<string, IFont> fonts = new();

        public void Require(IScript script) {
            foreach(var i in script.Dependencies) {
                Require(i);
            }

            var id = script.Key;
            if(!scripts.ContainsKey(id)) {
                scripts.TryAdd(id, script);
            }
        }

        // public async Task<Tag> GenerateScriptAsync(IStorage storage) {
        //     var scriptCode = new StringBuilder();
        //     foreach(var i in scripts.Values) {
        //         scriptCode.AppendLine(await i.GenerateAsync(storage));
        //     }
        //     return new Tag("script") {
        //         new PureHtmlNode(scriptCode.ToString())
        //     };
        // }

        public void Require(IStyle style) {
            var id = style.Key;
            if(!styles.ContainsKey(id)) {
                styles[id] = style;
            }
        }

        public async Task<Tag> GenerateScriptAsync(IStorage storage) {
            return new Tag("script") {
                new PureHtmlNode(GenerateSuperScript())
            };
        }

        public string GenerateSuperScript() {
            //StringBuilder scriptList = new StringBuilder();
            string[] scriptList = new string[scripts.Values.Count];
            int i = 0;
            foreach(var script in scripts.Values) {
                //scriptList.Append(script.Path);
                scriptList[i] = script.Path.Replace("\\", "/");
                i++;
            }
            Script superScript = new Script("");
            return superScript.GenerateSuperScript(scriptList);
        }

        public async Task<Tag> GenerateStyleAsync(IStorage storage) {
            return new Tag("style") {
                new PureHtmlNode(GenerateSuperStyle()) 
            };
        }

        public string GenerateSuperStyle() {
            string functionPath = new RelativePath("once.scss");
            StringBuilder styleList = new StringBuilder();
            styleList.AppendLine("@import " + "\"" + functionPath.Replace("\\", "/") + "\"" + ";");
            foreach(var style in styles.Values) {
                // FileStream stream = File.OpenRead(style.Path);
                // var file = File.ReadAllText(style.Path);
                // var newfile = file.Replace("☹filePathHash", style.Path.Replace("\\", "/").ToHashString())
                // .Replace("☹fileHash", BitConverter.ToString(MD5.Create().ComputeHash(stream)))
                // .Replace("☹filePath", style.Path.Replace("\\", "/").ToString())
                // .Replace("☹fileName", Path.GetFileName(style.Path.Replace("\\", "/")))
                // .Replace("☹directory", Path.GetDirectoryName(style.Path.Replace("\\", "/")));
                
                //Переписать сам SassProcessor. Добавить туда IFileManager FileManager (-)
                //$"$current-directory: \"{relativeDirectory.Replace('\\','/')}\";\n";
                //ReadFiles тк this (SassCompiler.FileManager = this;)
                //Из-за того, что наш SassProcessor личный, то мы переписали в нем как раз метод, который лезет внутрь и возвращает данные.
                //string a = "a";
                //Style style1 = new Style(style.Path);
                //style1.Context = a;
                styleList.AppendLine("@import " + "\"" + style.Path.Replace("\\", "/") + "\"" + ";");
            }
            //File.WriteAllText(new RelativePath("style.scss"), styleList.ToString());

            Style superStyle = new Style("");
            return superStyle.GenerateSuperStyle(styleList.ToString());
        }

        public void Require(IFont font) {
            var id = font.Key;
            if(!fonts.ContainsKey(id)) {
                fonts[id] = font;
            }
        }

        public async Task<Tag> GenerateFontAsync(IStorage storage) {
            var fontStyle = new StringBuilder();

            foreach(var i in fonts.Values) {
                fontStyle.AppendLine(await i.GenerateAsync(storage));
            }
            return new Tag("style") {
                new PureHtmlNode(fontStyle.ToString())
            };
        }
    }
}