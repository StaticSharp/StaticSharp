using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public record SvgBlurFilterGenerator(float StandardDeviationX, float StandardDeviationY) : TagGenerator {
        public override Task<Tag> Generate(string id) {
            return Task.FromResult(new Tag("filter", id){
                new Tag("feGaussianBlur"){
                    ["stdDeviation"] = $"{StandardDeviationX} {StandardDeviationY}"
                }
            });
        }
    }

}

