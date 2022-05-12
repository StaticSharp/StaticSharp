using System.Text.RegularExpressions;

namespace StaticSharp.Gears;

public static class CaseConverter {

    private static Regex PascalToKebabCaseRegex = new Regex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled);
    public static string PascalToKebabCase(string value) {
        if (string.IsNullOrEmpty(value))
            return value;

        return PascalToKebabCaseRegex.Replace(value, "-$1").ToLower();
    }
}




