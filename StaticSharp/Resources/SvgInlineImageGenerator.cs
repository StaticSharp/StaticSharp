using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Threading.Tasks;

namespace StaticSharp {
    public record SvgInlineImageGenerator(Genome<IAsset> Source) : TagGenerator {
        public override async Task<Tag> Generate(string id) {

            var image = await Source.CreateOrGetCached();
            var base64 = Convert.ToBase64String(await image.GetBytesAsync());

            return new Tag("image", id) {
                ["href"] = $"data:{await image.GetMediaTypeAsync()};base64,{base64}"
            };
        }
    }

}

