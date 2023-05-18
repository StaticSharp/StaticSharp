namespace StaticSharp.Resources.Text;

public static class TextUtils {
    public static string LoremIpsum(int numWords) {
        string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisl sit amet erat. Duis semper. Duis arcu massa, scelerisque vitae, consequat in, pretium a, enim. Pellentesque congue. Ut in risus volutpat libero pharetra tempor. Cras vestibulum bibendum augue. Praesent egestas leo in pede. Praesent blandit odio eu enim. Pellentesque sed dui ut augue blandit sodales. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aliquam nibh. Mauris ac mauris sed pede pellentesque fermentum. Maecenas adipiscing ante non diam sodales hendrerit.";
        int[] spaces ={5,11,17,21,27,39,50,56,60,64,71,83,90,98,108,112,118,129,134,144,149,156,161,171,180,186,195,202,209,216,219,226,234,242,247,251,257,268,273,277,285,295,300,304,313,317,321,331,336,341,345,350,356,361,369,374,379,386,398,405,415,419,427,430,436,449,457,460,463,469,478,485,494,502,507,518,527,534,543,551,555,558,564,573,581,586,589,595,608,612,616,619,625,633,642,653,658,664,671,674,683,688,695,698,707,715,723,730,738,744,751,754,761,765,770,783,794,803,814,819,823,828,836 };

        if (numWords > spaces.Length)
            return lorem;

        var length = spaces[numWords - 1];
        var result = lorem.Substring(0, spaces[numWords - 1]);
        if (result[result.Length - 1] == ',') {
            result = result.Substring(0, result.Length - 1);
        }
        if (result[result.Length - 1] == '.') {
            return result;
        } else {
            return result+'.';
        }
    }

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