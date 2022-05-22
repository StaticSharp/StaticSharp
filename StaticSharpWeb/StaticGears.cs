using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears {
    public static partial class Static {

        public static async Task<IEnumerable<T>> SequentialOrParallel<T>(this IEnumerable<Task<T>> x) {
#if DEBUG

            var result = new List<T>();
            foreach (var i in x) { 
                result.Add(await i);
            }
            return result;
#else
            return await Task.WhenAll(children.Select(x=>x.GenerateHtmlAsync(context))),
#endif
        }
    
    
    }
}
