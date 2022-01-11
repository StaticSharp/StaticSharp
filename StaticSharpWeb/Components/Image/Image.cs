using StaticSharpEngine;
using StaticSharpWeb.Html;
using StaticSharpWeb.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public interface IImage : IBlock {

    }
    public class Image : IImage {
        public string FilePath { get; }
        public string CallerFilePath { get; }
        public string Alt { get; set; }

        public string FullPath {
            get {
                if (File.Exists(FilePath)) { return FilePath; }
                var absolutePath = Path.Combine(Path.GetDirectoryName(CallerFilePath), FilePath);
                if (File.Exists(absolutePath)) {
                    return absolutePath;
                }
                Log.Error.OnObject(this, $"File {FilePath} not found");
                return null;
            }
        }

        public ImageResource Resource { get; private set; }

        public Image(string filePath, string alt = "", [CallerFilePath] string callerFilePath = "")
            => (FilePath, CallerFilePath, Alt) = (filePath, alt, callerFilePath);

        public async Task<Html.INode> GenerateBlockHtmlAsync(Context context) {
            Resource ??= await context.Storage.AddOrGetAsync(FilePath, () => new ImageResource(FilePath, context.Storage));
            var textContainter = new Tag("div", new { Class = "TextContainer" , id = "TextContainer" });
            var titleText = new Tag("h2") { "HEADER" };
            var text = new Tag("div") { "Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean. A small river named Duden flows by their place and supplies it with the necessary regelialia." };
            textContainter.Add(titleText);
            textContainter.Add(text);
            var tag = new Tag("div", new { Class = "ImageContainer", id = "ImageContainer" }) {
                new Tag("img", new { Class = "InnerImage", id = "InnerImage", src = Resource.Source, alt = Alt}),
                //new Tag("img", new { alt = Alt, style = "width: 100%; height: auto;"}),
                new JSCall(new RelativePath("Image.js")).Generate(context),        
                //new JSCall(new RelativePath("TestImage.js")).Generate(context),       
                //new JSCall(new RelativePath("RoiImage.js"), Resource.Aspect, Resource.Roi).Generate(context),  
                new JSCall(new RelativePath("MipsSelector.js"), Resource.Mips).Generate(context),
            };
            tag.Add(textContainter);
            context.Includes.Require(new Style(new RelativePath("Image.scss")));
            if (Resource.Roi.Length > 0){
                tag.Add(new JSCall(new RelativePath("RoiImage.js"), Resource.Aspect, Resource.Roi).Generate(context));
            }
            // if (Resource.Roi.Length > 0){
            //     tag.Add(new JSCall(new RelativePath("TestRoiImage.js"), Resource.Aspect, Resource.Roi).Generate(context));
            // }
            return tag;
        }
    }
    public static class ImageStatic {
        public static void Add<T>(this T collection, IImage item) where T : IVerifiedBlockReceiver, IFillAnchorsProvider {
            collection.AddBlock(item);
        }
    }
}
