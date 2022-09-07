using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Gears {
        public interface IInlineCollector : IVoidEnumerable {
            void Add(string? id, IInline? value);
        }
    }

    public static class InlineCollectorStatic {

        /*public static void Add<T>(this T collector, Paragraph paragraph) where T : IInlineCollector {
            if (paragraph != null) {
                foreach (var i in paragraph.Children) {
                    collector.Add(i);
                }                
            }
        }*/

        public static void Add<T>(this T collector, IInline? value) where T : IInlineCollector {
            collector.Add(null, value);
        }

        public static void Add<T>(
            this T collector,
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IInlineCollector {
            collector.Add(
                null,
                new Text(text, true, callerFilePath, callerLineNumber)
            );
        }


    }

}
