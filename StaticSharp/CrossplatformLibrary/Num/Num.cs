using Javascriptifier;
using StaticSharp.Animations;

namespace StaticSharp {


    namespace Scripts {
        public class NumAttribute : ScriptReferenceAttribute {
            public NumAttribute() : base(GetScriptFilePath()) { }
        }
    }




    namespace Js {

        public static class Constants {
            public static readonly string Undefined = "undefined";
        }


        //[JavascriptClass("")]
        public static class Num {
            [JavascriptOnlyMember]
            public static double First(params double[] value) => throw new JavascriptOnlyException();

            [JavascriptOnlyMember]
            public static double Sum(params double[] value) => throw new JavascriptOnlyException();

            [JavascriptOnlyMember]
            public static double Min(params double[] value) => throw new JavascriptOnlyException();

            [JavascriptOnlyMember]
            public static double Max(params double[] value) => throw new JavascriptOnlyException();

            [JavascriptOnlyMember]
            public static double Clamp(double value, double min, double max ) => throw new JavascriptOnlyException();

            public static double PiecewiseLinearInterpolation(double x, params (double x, double y)[] keyframes) {
                if (keyframes.Length == 0) {
                    throw new ArgumentException("Keyframes list must not be empty.");
                }

                if (keyframes.Length == 1) {
                    return keyframes[0].y;
                }

                // Sort keyframes by x.
                Array.Sort(keyframes, (a, b) => a.x.CompareTo(b.x));

                if (x < keyframes[0].x) {
                    return keyframes[0].y;
                }

                if (x >= keyframes[keyframes.Length - 1].x) {
                    return keyframes[keyframes.Length - 1].y;
                }

                for (int i = 1; i < keyframes.Length; i++) {
                    if (x < keyframes[i].x) {
                        double deltaX = keyframes[i].x - keyframes[i - 1].x;
                        double alpha = (x - keyframes[i-1].x) / deltaX;
                        double deltaY = keyframes[i].y - keyframes[i - 1].y;
                        return keyframes[i-1].y + deltaY * alpha;
                    }
                }

                // This should never happen if the keyframes are sorted correctly.
                throw new Exception("Keyframes are not sorted correctly.");
            }

        }

    }


}

