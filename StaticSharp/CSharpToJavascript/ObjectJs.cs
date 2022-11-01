using StaticSharp.Gears;
using System;

namespace StaticSharp {
    namespace Js {

        public class Object {
            //public dynamic this[string propertyName] => NotEvaluatableObject<dynamic>();

            /*[ConvertToJs("{0}[{1}]")]
            public Object Property(string name) => NotEvaluatableObject<Object>();            
            
            [ConvertToJs("{0}[{1}]")]
            public T Property<T>(string name) where T : new() => NotEvaluatableObject<T>();*/

            [ConvertToJs("{0}")]
            public T As<T>() where T : new() => NotEvaluatableObject<T>();
            
            public Object this[string name] {
                [ConvertToJs("{0}[{1}]")]
                get {
                    return NotEvaluatableObject<Object>();
                }
            }
        }
    

        public static partial class Static {
            
            //public static T As<T>(this Js.Object _) where T : Js.Object, new() => new();

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

