using System.Text.RegularExpressions;

namespace StaticSharp.Gears {
    public static class CaseUtils {
        public static Regex CamelCaseRegex = new("[A-Z]+(?![a-z])|[A-Z]");
        public static string CamelToKebab(string value) {
            return CamelCaseRegex.Replace(value, match => (match.Index > 0 ? "-" : "") + match.ToString().ToLower());
        }

    }
}
