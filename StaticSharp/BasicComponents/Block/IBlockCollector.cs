using StaticSharp.Gears;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    namespace Gears {
        public interface IBlockCollector : IVoidEnumerable {
            void Add(string? id, IBlock? value);
        }
    }


    public static class BlockCollectorStatic {

        
        public static void Add<T>(this T collector, IEnumerable<KeyValuePair<string?, IBlock>>? values) where T : IBlockCollector {
            if (values == null)
                return;
            foreach (var i in values)
                collector.Add(i.Key, i.Value);
        }


        public static void Add<T>(this T collector, IBlock? value) where T : IBlockCollector {
            collector.Add(null, value);
        }

        public static void Add<T>(this T collector, IEnumerable<IBlock?>? values) where T : IBlockCollector {
            if (values == null)
                return;
            foreach (var i in values) {
                collector.Add(null, i);
            }
        }

        public static void Add<T>(
            this T collector,
            Inlines inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) where T : IBlockCollector =>

            collector.Add(null, new Paragraph(inlines, callerFilePath, callerLineNumber));

        public static void Add<T>(this T collector, string? id, Inlines inlines) where T : IBlockCollector {
            collector.Add(id, new Paragraph(inlines));
            /*if (paragraph != null) {

                ((IBlockCollector)collector).Add(id, paragraph);                
            }*/
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
