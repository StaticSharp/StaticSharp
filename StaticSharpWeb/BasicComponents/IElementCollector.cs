using StaticSharp.Gears;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Gears {
        public interface IVoidEnumerable : IEnumerable {
            IEnumerator IEnumerable.GetEnumerator() {
                return null!;
            }
        }


        public interface IElementCollector : IVoidEnumerable {
            void Add(IElement? value);


        }
    }


    public static class ElementCollectorStatic {

        public static void Add<T>(this T collector, Group? group) where T : IElementCollector {
            if (group != null) {
                foreach (var i in group.Children) {
                    collector.Add(i);
                }
            }
        }

        public static void Add<T>(this T collector, string text) where T : IElementCollector {
            
        }

    }
}
