using Javascriptifier;
using System.Linq.Expressions;

namespace StaticSharp {


    namespace Scripts {
        public class LinqAttribute : ScriptReferenceAttribute {
            public LinqAttribute() : base(GetScriptFilePath()) { }
        }
    }


    namespace Js {

        public interface Enumerable {
            [JavascriptOnlyMember]
            public static IEnumerable<R> FromArguments<R>(params R[] args) => throw new JavascriptOnlyException();
        }

        public interface Enumerable<T> : Enumerable {
            T? First(Expression<Func<T, bool>>? func);
            T? First();
        }
    }

}