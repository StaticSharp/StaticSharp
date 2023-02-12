using Javascriptifier;
using StaticSharp.Gears;

namespace StaticSharp {
    namespace Js {



        [JavascriptClass("window")]
        public static class Window {

            [JavascriptOnlyMember]
            public static bool Touch => throw new JavascriptOnlyException();

            [JavascriptOnlyMember]
            public static bool UserInteracted => throw new JavascriptOnlyException();

            [JavascriptOnlyMember]
            public static double DevicePixelRatio => throw new JavascriptOnlyException();

        }

    }


}

