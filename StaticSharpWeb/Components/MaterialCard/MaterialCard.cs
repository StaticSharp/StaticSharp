using StaticSharpWeb;
using StaticSharpWeb.Html;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace StaticSharpWeb.Components {
    public class MaterialCard : IElement {
        Func<Context, IMaterial> MaterialGetter;
        public MaterialCard(IMaterial material) {
            MaterialGetter = (context) => material;
        }
        //public MaterialCard(ILanguageSelector<Material> languageSelector) {
        //    MaterialGetter = (context) => languageSelector[context.Language];
        //}


        private async ValueTask<Tag> GetSlideContentAsync(IMaterial material, Context context) {
            var description = new Tag("div", new { Class = "Text" });
            if (material.Description != null) {
                description.Add(await material.Description.GetPlaneTextAsync(context));
            }
            return new Tag("div", new { Class = "MaterialCardSlider" }) {
                new Tag("h2", new { Class = "Title" }){
                    material.Title ?? ""
                },
                    description
            };
        }

        public async Task<Tag> GenerateHtmlAsync(Context context) {
            var material = MaterialGetter(context);
            var node = (material as StaticSharpEngine.IRepresentative)?.Node;
            if (node == null) {
                throw new Exception("Material is outside of node tree");//todo: refactor exceptions
            }
            
            var result = new Tag("a", new { Class = "MaterialCard", href = context.NodeToUrl(node) });
            if (material.TitleImage != null) {
                Image image;
                if (material.TitleImage is Video video) {
                    image = await video.GetImageAsync(context);
                } else {
                    image = material.TitleImage as Image;
                }
                result.Add(await image.GenerateHtmlAsync(context));
            } else {
                Log.Warning.OnObject(this, "TitleImage of material required");
                //result.Add(CsmlPredefined.MissingImage.Generate(context));
            }
            result.Add(await GetSlideContentAsync(material, context));
            var className = nameof(MaterialCard);
            context.Includes.Require(new Style(AbsolutePath(className + ".scss")));
            result.Add(new JSCall(AbsolutePath(className + ".js")).Generate(context));
            return result;
        }
    }

    public static class MaterialCardStatic {
        public static void Add<T>(this T collection, MaterialCard item) where T : IElementContainer {
            collection.AddElement(item);
        }
    }
}