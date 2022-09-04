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

        
        public static void Add<T>(this T collector, IEnumerable<KeyValuePair<string?, IBlock>> values) where T : IBlockCollector {
            foreach (var i in values)
                collector.Add(i.Key, i.Value);
        }


        public static void Add<T>(this T collector, IBlock? value) where T : IBlockCollector {
            collector.Add(null, value);
        }

        public static void Add<T>(this T collector, IEnumerable<IBlock?> value) where T : IBlockCollector {
            foreach (var i in value) {
                collector.Add(null, i);
            }
        }

        public static void Add<T>(this T collector, Group? group) where T : IBlockCollector {
            if (group != null) {
                foreach (var i in group.Children) {
                    collector.Add(i.Key,i.Value);
                }
            }
        }

        public static void Add<T>(this T collector, Paragraph paragraph) where T : IBlockCollector =>
            collector.Add(null, paragraph);

        public static void Add<T>(this T collector, string? id, Paragraph paragraph) where T : IBlockCollector {
            if (paragraph != null) {
                ((IBlockCollector)collector).Add(id, paragraph);                
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
                //,null
                );
        }

    }
}
