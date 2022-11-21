using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Threading.Tasks;

namespace StaticSharp {
    public record SvgInlineImageGenerator(Genome<Asset> Source) : TagGenerator {
        public override async Task<Tag> Generate(string id) {

            var image = await Source.CreateOrGetCached();
            var base64 = Convert.ToBase64String(image.ReadAllBites());

            return new Tag("image", id) {
                ["href"] = $"data:{image.MediaType};base64,{base64}"
            };
        }
    }

}

