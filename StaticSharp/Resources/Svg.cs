﻿using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {


    public static class Svg {
        public static Tag BlurFilter(float StandardDeviationX, float StandardDeviationY) {
            var result = new Tag("filter"){
                new Tag("feGaussianBlur"){
                    ["stdDeviation"] = $"{StandardDeviationX.ToInvariant()} {StandardDeviationY.ToInvariant()}"
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

    /*public record SvgBlurFilterGenerator(float StandardDeviationX, float StandardDeviationY) {

        public Tag Tag {
            get { 
                var key = Hash.CreateFromString(KeyUtils.GetKeyForObject(this)).ToString(16);
                var tag = 

                return ;
            }
        }

    }*/

}
