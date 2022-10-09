using StaticSharp.Gears;
using System;

namespace StaticSharp {
    namespace Js {
        public class Object {

            [ThreadStatic] public static bool NotEvaluatableFound = false;

            protected static T NotEvaluatableObject<T>() where T : new() {
                NotEvaluatableFound = true;
                return new();
            }
            protected static T NotEvaluatableValue<T>() {
                NotEvaluatableFound = true;
                return default;
            }
        }
    }

    public static partial class Static {
        [ConvertToJs("{0}")]
        public static T As<T>(this Js.Object _) where T : Js.Object, new() => new();
    }


}

