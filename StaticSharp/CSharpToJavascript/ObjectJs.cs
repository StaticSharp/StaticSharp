using StaticSharp.Gears;
using System;

namespace StaticSharp {
    namespace Js {


        public class Object {

            
        }
    

        public static partial class Static {
            [ConvertToJs("{0}")]
            public static T As<T>(this Js.Object _) where T : Js.Object, new() => new();

            [ThreadStatic] public static bool NotEvaluatableFound = false;
            public static T NotEvaluatableObject<T>() where T : new() {
                NotEvaluatableFound = true;
                return new();
            }
            public static T NotEvaluatableValue<T>() {
                NotEvaluatableFound = true;
                return default;
            }

        }
    }

}

