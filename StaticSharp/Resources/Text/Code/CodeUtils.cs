

using EnvDTE;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace StaticSharp;

public partial class Static {
    public static string TrimEmptyLines(this string code) {
        bool skipLeadingEmptyLine(ref int i) {
            for (int j = i; j < code.Length; j++) {
                if (code[j] == '\n') {
                    i = j + 1;
                    return true;
                }

                if (!char.IsWhiteSpace(code[j]))
                    return false;
            }

            return false;
        }

        bool skipTrailingEmptyLine(ref int i) {
            for (int j = i - 1; j >= 0; j--) {
                if (code[j] == '\n') {
                    i = j;
                    return true;
                }

                if (!char.IsWhiteSpace(code[j]))
                    return false;
            }

            return false;
        }

        int a = 0;
        while (skipLeadingEmptyLine(ref a))
            ;

        int b = code.Length;
        while (skipTrailingEmptyLine(ref b))
            ;

        if (a >= b)
            return "";

        return code[a..b];
    }




    public static string Untab(this string code) {

        code = code.Replace("\t", "    ");

        string[] lines = code.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
        int minSpaces = int.MaxValue;

        for (int i = 0; i < lines.Length; i++) {
            lines[i] = lines[i].TrimEnd();
        }

        foreach (var i in lines) {
            var numSpaces = i.TakeWhile(char.IsWhiteSpace).Count();
            minSpaces = Math.Min(minSpaces, numSpaces);
        }

        if (minSpaces > 0) {
            return string.Join('\n', lines.Select(x => x[minSpaces..]));
        } else {
            return string.Join('\n', lines);
        }      
    }



}