using StaticSharp.Gears;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    namespace Gears {
        public interface IBlockCollector {
            void Add(IBlock? value);
        }
    }


    public static class BlockCollectorStatic {

  

        public static void Add<T>(this T collector, IEnumerable<IBlock?>? values) where T : IBlockCollector {
            if (values == null)
                return;
            foreach (var i in values) {
                collector.Add(i);
            }
        }

        #region Block
        public static void Add<T>(this T collector, IBlock? value) where T : IBlockCollector {
            collector.Add(value);
        }

        #endregion



        #region Inlines

        public static void Add<T>(
            this T collector,
            Inlines? inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {
            if (inlines != null)
                collector.Add(new Paragraph(inlines, callerLineNumber, callerFilePath));
        }
        #endregion


        public static void Add<T>(
            this T collector,
            Inline inline,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {
            collector.Add(new Paragraph(inline, callerLineNumber, callerFilePath));
        }


        public static void Add<T>(
            this T collector,
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {
            collector.Add(
                new Paragraph(text, callerLineNumber, callerFilePath)
                );
        }

    }
}
