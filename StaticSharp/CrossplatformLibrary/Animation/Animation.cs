using Javascriptifier;
using System.Linq.Expressions;

namespace StaticSharp {

    namespace Scripts {
        public class AnimationAttribute : ScriptReferenceAttribute {
            public AnimationAttribute() : base(GetScriptFilePath()) { }
        }
    }


    namespace Js {
        //[JavascriptClass("")]
        public static class Animation {

            [JavascriptOnlyMember]
            [Stateful]
            /// <param name="duration">The duration in seconds.</param>
            public static T Duration<T>(double diration, T target) => throw new Javascriptifier.JavascriptOnlyException();

            [JavascriptOnlyMember]
            [Stateful]
            /// <param name="speedLimit">The speedLimit target units per seconds.</param>
            public static T SpeedLimit<T>(double speedLimit, T target) => throw new Javascriptifier.JavascriptOnlyException();
        
        }
    }


}

