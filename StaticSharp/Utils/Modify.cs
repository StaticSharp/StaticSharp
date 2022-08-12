using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {
    public  static partial class Static {

        public static T Modify<T>(this T x, Action<T> action) {
            action(x);
            return x;
        }

    }
}
