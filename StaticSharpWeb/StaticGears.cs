using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears {
    public static partial class Static {

        /*public static async Task<IEnumerable<T>> SequentialOrParallel<T>(this IAsyncEnumerable<T> x) {
            return await x.ToListAsync();
        }*/

        public static async Task<IEnumerable<T>> SequentialOrParallel<T>(this IEnumerable<Task<T>?> x) {
#if DEBUG
            

            var result = new List<T>();
            foreach (var i in x) {
                if (i != null) {
                    var item = await i;
                    result.Add(item);
                }
            }
            return result;
#else
            IEnumerable<Task<T?>> tasks =
                from i in x
                where i != null
                select i;

            return await Task.WhenAll(tasks);
#endif
        }


    }
}
