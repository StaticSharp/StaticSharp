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
    public interface IImage : IElement {

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

        public async Task<Html.Tag> GenerateHtmlAsync(Context context) {
            Resource ??= await context.Storage.AddOrGetAsync(FilePath, () => new ImageResource(FilePath, context.Storage));
            var tag = new Tag("div", new { Class = nameof(Image) }) {
                new Tag("img", new { src = Resource.Source, alt = Alt, style = "width: 100%; height: auto;"}),
                new JSCall(AbsolutePath("Image.js")).Generate(context),
                //new JSCall(AbsolutePath("RoiImage.js"), Resource.Aspect, Resource.Roi).Generate(context),
                new JSCall(AbsolutePath("MipsSelector.js"), Resource.Mips).Generate(context)
            };
            return tag;
        }
    }
    public static class ImageStatic {
        public static void Add<T>(this T collection, IImage item) where T : IElementContainer {
            collection.AddElement(item);
        }
    }
}