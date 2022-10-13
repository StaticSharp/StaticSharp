using StaticSharp.Gears;

namespace StaticSharp {
    namespace Js {

        [ConvertToJs("window")]
        public static class Window {
            public static bool Touch => NotEvaluatableValue<bool>();
        }

        public static class Math {
            public static double First(params double[] value) => NotEvaluatableValue<double>();
            public static double Sum(params double[] value) => NotEvaluatableValue<double>();
            public static double Min(params double[] value) => NotEvaluatableValue<double>();
            public static double Max(params double[] value) => NotEvaluatableValue<double>();
        }

    }


}

