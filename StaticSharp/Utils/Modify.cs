
namespace StaticSharp {
    public static partial class Static {

        public static T Modify<T>(this T x, Action<T> action) {
            action(x);
            return x;
        }

        public static T Function<T>(Func<T> func) {
            return func();
        }
    }
}
