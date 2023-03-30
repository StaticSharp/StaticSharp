using Javascriptifier;
using System;
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
            static Enumerable<R> FromArguments<R>(params R[] args) => throw new JavascriptOnlyException();

            [JavascriptOnlyMember]
            static Enumerable<int> Range(int start, int count) => throw new JavascriptOnlyException();
        }

        public interface Enumerable<T> : Enumerable {
            T? First(Expression<Func<T, bool>>? func);
            
            T? First();

            bool Any(Expression<Func<T, bool>>? func);

            bool Any();

            Enumerable<T> Where(Expression<Func<T, bool>> func);

            Enumerable<TResult> Select<TResult>(Expression<Func<T, TResult>> func);

            T? Last(Expression<Func<T, bool>>? func);

            T? Last();

            int Count(Expression<Func<T, bool>>? func);
            
            int Count();

            TResult Aggregate<TResult>(Expression<Func<TResult, T, TResult>> func, TResult? initialValue);

            T Aggregate(Expression<Func<T, T, T>> func);

            T Max();

            T Min();

            T Sum();

            Enumerable<T> Order(Expression<Func<T, T, int>> compareFunc);

            Enumerable<T> Order();

            
            Enumerable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector, Expression<Func<TKey, TKey, int>> compareFunc);

            Enumerable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);

            Enumerable<T> Reverse();

            Enumerable<T> Skip(int count);

            Enumerable<T> Take(int count);
        }
    }

}