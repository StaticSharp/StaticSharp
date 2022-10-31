using StaticSharp.Gears;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    namespace Gears {
        public interface IBlockCollector : IVoidEnumerable {
            void Add(HtmlModifier? id, IBlock? value);
        }
    }


    public static class BlockCollectorStatic {


        public static void Add<T>(this T collector, IEnumerable<KeyValuePair<HtmlModifier?, IBlock>>? values) where T : IBlockCollector {
            if (values == null)
                return;
            foreach (var i in values)
                collector.Add(i.Key, i.Value);
        }        

        public static void Add<T>(this T collector, IEnumerable<IBlock?>? values) where T : IBlockCollector {
            if (values == null)
                return;
            foreach (var i in values) {
                collector.Add(null, i);
            }
        }

        #region Block
        public static void Add<T>(this T collector, IBlock? value) where T : IBlockCollector {
            collector.Add(null, value);
        }

        public static void Add<T>(this T collector, string name, IBlock? value) where T : IBlockCollector {
            collector.Add(new HtmlModifier().AssignParentProperty(name), value);         
        }

        #endregion



        #region Inlines

        public static void Add<T>(
            this T collector,
            Inlines inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector { 

            collector.Add(null, new Paragraph(inlines, callerFilePath, callerLineNumber));
        }

        public static void Add<T>(
            this T collector,
            HtmlModifier modifier,
            Inlines inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {

            collector.Add(modifier, new Paragraph(inlines, callerFilePath, callerLineNumber));
        }
        public static void Add<T>(
            this T collector,
            string name,
            Inlines inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {

            collector.Add(new HtmlModifier().AssignParentProperty(name), new Paragraph(inlines, callerFilePath, callerLineNumber));
        }

        #endregion


        public static void Add<T>(
            this T collector,
            HtmlModifier? modifier,
            Inline inline,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {
            collector.Add(modifier, new Paragraph(inline, callerFilePath, callerLineNumber));
        }

        public static void Add<T>(
            this T collector,
            Inline inline,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {
            collector.Add(null, new Paragraph(inline, callerFilePath, callerLineNumber));
        }


        public static void Add<T>(
            this T collector,
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector {
            collector.Add(
                new Paragraph(text, callerFilePath, callerLineNumber)
                //,null
                );
        }

    }
}
