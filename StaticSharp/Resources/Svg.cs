using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp.Gears {


    public static class Svg {
        public static Tag BlurFilter(double StandardDeviationX, double StandardDeviationY) {
            var result = new Tag("filter"){
                new Tag("feGaussianBlur"){
                    ["stdDeviation"] = $"{StandardDeviationX} {StandardDeviationY}"
                }
            };
            result.MakeIdFromContent();
            return result;
        }

        public static Tag InlineImage(string dataUrl) {
            var result = new Tag("image") {
                ["href"] = dataUrl
            };
            result.MakeIdFromContent();
            return result;
        }


    }
}

