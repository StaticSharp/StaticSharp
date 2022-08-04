using System.Collections;

namespace StaticSharp {

    namespace Gears {
        public interface IVoidEnumerable : IEnumerable {
            IEnumerator IEnumerable.GetEnumerator() {
                return null!;
            }
        }
    }
}
