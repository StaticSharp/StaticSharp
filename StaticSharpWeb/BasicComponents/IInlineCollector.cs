using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Gears {
        public interface IInlineCollector : IVoidEnumerable {
            void Add(IInline? value);
        }
    }

    public static class InlineCollectorStatic {

        public static void Add<T>(this T collector, Paragraph paragraph) where T : IInlineCollector {
            if (paragraph != null) {
                collector.Add(paragraph);
            }
        }

        public static void Add<T>(
            this T collector,
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IInlineCollector {
            collector.Add(
                new Text(text, true, callerFilePath, callerLineNumber)
                );
        }


    }

}
