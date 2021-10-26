using System;

namespace Csml {
    public static class Extensions {
        public static T If<T>(this T _this, bool condition, Func<T, T> func) => condition ? func(_this) : _this;
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture) {
            return suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType)
                ? input.Substring(0, input.Length - suffixToRemove.Length)
                : input;
        }

    }
}
