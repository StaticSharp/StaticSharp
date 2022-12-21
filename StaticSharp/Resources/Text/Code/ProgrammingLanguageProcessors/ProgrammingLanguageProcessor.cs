using AngleSharp.Html;
using ColorCode;
using ColorCode.Styling;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;



namespace StaticSharp.Gears {
    public class ProgrammingLanguageProcessor {
        private static ProgrammingLanguageProcessor First = new ProgrammingLanguageProcessor();
        private ProgrammingLanguageProcessor? Next;
        public static void AddProgrammingLanguageProcessor(ProgrammingLanguageProcessor value) {
            value.Next = First;
            First = value;
        }

        public static ProgrammingLanguageProcessor FindByName(string name) {
            var current = First;
            var favorite = First;
            var suitability = 0;

            while (current != null) {
                var s = current.Suitability(name);
                if (s > suitability) {
                    suitability = s;
                    favorite = current;
                }
                current = current.Next;
            }
            return favorite;
        }

        protected virtual int Suitability(string name) {
            return 0;
        }

        private static Regex NamedRegionStartRegex = new Regex(@"#.*?region\s+([\w\s]*[\w])", RegexOptions.IgnoreCase);
        private static Regex RegionEndRegex = new Regex(@"#.*?endregion", RegexOptions.IgnoreCase);
        public static string[] Lines(string code) {
            return code.Split(
                new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
        }

        public virtual string? GetRegion(string code, string regionName) {
            var resultLines = new List<string>();

            var lines = Lines(code);
            for (int start = 0; start < lines.Length; start++) {
                var matchStart = NamedRegionStartRegex.Match(lines[start]);
                if (!matchStart.Success) continue;
                if (matchStart.Groups[1].Value == regionName) {
                    for (int i = start+1; i < lines.Length; i++) {
                        var matchEnd = RegionEndRegex.Match(lines[i]);
                        if (!matchEnd.Success) {
                            resultLines.Add(lines[i]);
                        } else { 
                            return string.Join('\n', resultLines);
                        }
                    }                    
                }
            }
            return null;
        }


        public virtual Inlines Highlight(string code, string programmingLanguageName, bool dark = false) {
            var formatter = new CodeFormatter(dark ? StyleDictionary.DefaultDark : StyleDictionary.DefaultLight);
            var language = Languages.FindById(programmingLanguageName);
            var html = formatter.GetInlines(code, language);
            return html;
        }

    }
}