using Javascriptifier;
using StaticSharp.Gears;
using System;
using System.Linq.Expressions;

namespace StaticSharp {
    namespace Js {

        public static class Storage {

            [JavascriptOnlyMember]
            public static T Restore<T>(string name, Expression<Func<T>> getter) where T: struct => throw new Javascriptifier.JavascriptOnlyException();
        }

    }


}

