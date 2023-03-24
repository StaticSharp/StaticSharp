using Javascriptifier;

namespace StaticSharp {
    namespace Js {

        public static class Constants {
            public static readonly string Undefined = "undefined";
        }


        [JavascriptClass("")]
        public static class Math {
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
        }

    }


}

