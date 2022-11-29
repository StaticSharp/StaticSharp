using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public record SvgBlurFilterGenerator(float StandardDeviationX, float StandardDeviationY) : Genome<Task<Tag>> {
        public override Task<Tag> Create() {
            return Task.FromResult(new Tag("filter", Key){
                new Tag("feGaussianBlur"){
                    ["stdDeviation"] = $"{StandardDeviationX.ToInvariant()} {StandardDeviationY.ToInvariant()}"
                }
            });
        }
    }

}

