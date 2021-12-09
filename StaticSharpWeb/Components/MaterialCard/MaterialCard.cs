using StaticSharpWeb;
using StaticSharpWeb.Html;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace StaticSharpWeb.Components {
    public class MaterialCard : IBlock {
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

        public async Task<INode> GenerateBlockHtmlAsync(Context context) {
            var material = MaterialGetter(context);
            var result = new Tag("a", new { Class = "MaterialCard", href = context.Urls.ProtoNodeToUri((material as StaticSharpEngine.IRepresentative)?.Node) });
            if (material.TitleImage != null) {
                Image image;
                if (material.TitleImage is Video video) {
                    image = await video.GetImageAsync(context);
                } else {
                    image = material.TitleImage as Image;
                }
                result.Add(await image.GenerateBlockHtmlAsync(context));
            } else {
                Log.Warning.OnObject(this, "TitleImage of material required");
                //result.Add(CsmlPredefined.MissingImage.Generate(context));
            }
            result.Add(await GetSlideContentAsync(material, context));
            var className = nameof(MaterialCard);
            context.Includes.Require(new Style(new RelativePath(className + ".scss")));
            result.Add(new JSCall(new RelativePath(className + ".js")).Generate(context));
            return result;
        }
    }

    public static class MaterialCardStatic {
        public static void Add<T>(this T collection, MaterialCard item) where T : IVerifiedBlockReceiver {
            collection.AddBlock(item);
        }
    }
}