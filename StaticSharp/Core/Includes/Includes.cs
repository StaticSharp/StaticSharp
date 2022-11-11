using StaticSharpWeb.Html;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System;
using StaticSharp.Html;
using System.Collections;

namespace StaticSharp.Gears {

    public interface IIncludes {



        //void Require(IScript script);

        void Require(IStyle style);




        //void Require(IFont font);

        //CacheableFont GetOrCreateFont(Font font);

        //Task<Tag> GenerateScriptAsync();

        //Task<Tag> GenerateFontAsync();
        Task<Tag> GenerateStyleAsync();
    }


    public class Includes : IIncludes {
        //private readonly ConcurrentDictionary<string, IScript> scripts = new();
        private readonly ConcurrentDictionary<string, IStyle> styles = new();
        //private readonly ConcurrentDictionary<string, CacheableFont> fonts = new();


        


        /*public void Require(IScript script) {
            foreach(var i in script.Dependencies) {
                Require(i);
            }

            var id = script.Key;
            if(!scripts.ContainsKey(id)) {
                scripts.TryAdd(id, script);
            }
        }*/

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


        /*public CacheableFont GetOrCreateFont(Font font) {
            return fonts.GetOrAdd(font.Key, x => new CacheableFont(font)
            );
        }*/



        /*public async Task<Tag> GenerateScriptAsync() {
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
        }*/

        public async Task<Tag> GenerateStyleAsync() {
            return new Tag("style") {
                new PureHtmlNode(GenerateSuperStyle()) 
            };
        }

        public string GenerateSuperStyle() {
            string functionPath = MakeAbsolutePath("once.scss");
            StringBuilder styleList = new StringBuilder();
            styleList.AppendLine("@import " + "\"" + functionPath.Replace("\\", "/") + "\"" + ";");
            foreach(var style in styles.Values) {
                styleList.AppendLine("@import " + "\"" + style.Path.Replace("\\", "/") + "\"" + ";");
            }

            Style superStyle = new Style("");
            return superStyle.GenerateSuperStyle(styleList.ToString());
        }

        



        /*public void Require(IFont font) {
            var id = font.Key;
            if(!fonts.ContainsKey(id)) {
                fonts[id] = font;
            }
        }*/


    }
}