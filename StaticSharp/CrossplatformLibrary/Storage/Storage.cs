using StaticSharp.Gears;
using System;
using System.Linq.Expressions;

namespace StaticSharp {
    namespace Js {

        public static class Storage {
            public static T Restore<T>(string name, Expression<Func<T>> getter) where T: struct {
                return NotEvaluatableValue<T>();
            }
        }

    }


}

