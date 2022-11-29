using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Threading.Tasks;

namespace StaticSharp {
    public record SvgInlineImageGenome(Genome<IAsset> Source) : Genome<Task<Tag>> {

        public override Genome[]? Sources => new Genome[] { Source };
        public override async Task<Tag> Create() {
            var image = Source.CreateOrGetCached();
            var base64 = Convert.ToBase64String(await image.GetBytesAsync());

            return new Tag("image", Key) {
                ["href"] = $"data:{await image.GetMediaTypeAsync()};base64,{base64}"
            };
        }

        /*public override async Task<Tag> Generate(string id) {

            
        }*/
    }

}

