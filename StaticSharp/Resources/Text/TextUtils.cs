namespace StaticSharp.Resources.Text;

public static class TextUtils {
    public static string TrimEmptyLines(this string text) {
        bool skipLeadingEmptyLine(ref int i) {
            for (int j = i; j < text.Length; j++) {
                if (text[j] == '\n') {
                    i = j + 1;
                    return true;
                }

                if (!char.IsWhiteSpace(text[j]))
                    return false;
            }

            return false;
        }

        bool skipTrailingEmptyLine(ref int i) {
            for (int j = i - 1; j >= 0; j--) {
                if (text[j] == '\n') {
                    i = j;
                    return true;
                }

                if (!char.IsWhiteSpace(text[j]))
                    return false;
            }

            return false;
        }

        int a = 0;
        while (skipLeadingEmptyLine(ref a))
            ;

        int b = text.Length;
        while (skipTrailingEmptyLine(ref b))
            ;

        if (a >= b)
            return "";

        return text[a..b];
    }




    public static string Untab(this string text) {

        text = text.Replace("\t", "    ");

        string[] lines = SplitLines(text);

        int minSpaces = int.MaxValue;

        for (int i = 0; i < lines.Length; i++) {
            lines[i] = lines[i].TrimEnd();
        }

        foreach (var i in lines) {
            var numSpaces = i.TakeWhile(char.IsWhiteSpace).Count();
            if (numSpaces < i.Length) {
                minSpaces = Math.Min(minSpaces, numSpaces);
            }
        }

        if (minSpaces > 0) {
            return string.Join('\n', lines.Select(x => x.Length > minSpaces ? x[minSpaces..] : ""));
        } else {
            return string.Join('\n', lines);
        }
    }
    public static string[] SplitLines(this string text) {
        return text.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
    }


}