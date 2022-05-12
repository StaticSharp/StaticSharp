using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Gears {
        public interface IBlockCollector : IVoidEnumerable {
            void Add(IBlock? value);
        }
    }
    public static class BlockCollectorStatic {

        public static void Add<T>(this T collector, Group? group) where T : IBlockCollector {
            if (group != null) {
                foreach (var i in group.Children) {
                    collector.Add(i);
                }
            }
        }

        public static void Add<T>(this T collector, Paragraph paragraph) where T : IBlockCollector {
            if (paragraph != null) {
                collector.Add(paragraph);                
            }
        }

        public static void Add<T>(
            this T collector,
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {
            collector.Add(
                new Paragraph(callerFilePath, callerLineNumber) {
                    new Text(text, true, callerFilePath, callerLineNumber)
                }
                );
        }

    }
}
