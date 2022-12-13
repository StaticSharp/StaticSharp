using ColorCode;
using ColorCode.Common;
using ColorCode.HTML.Common;
using ColorCode.Parsing;
using ColorCode.Styling;
using StaticSharp.Html;
using System.Collections.Generic;


namespace StaticSharp.Gears {
    public class CodeFormatter : CodeColorizerBase {
        public CodeFormatter(StyleDictionary Style = null, ILanguageParser languageParser = null) : base(Style, languageParser) {
        }

        public Inlines GetInlines(string sourceCode, ILanguage language) {

            var result = new Inlines();

            Inline CreateInlineForScope(Scope scope) {
                string foreground = string.Empty;
                string background = string.Empty;
                bool italic = false;
                bool bold = false;
                if (Styles.TryGetValue(scope.Name, out var style)) {
                    foreground = style.Foreground;
                    background = style.Background;
                    italic = style.Italic;
                    bold = style.Bold;
                }
                var result = new Inline();

                if (!string.IsNullOrEmpty(foreground)) {
                    result.ForegroundColor = new Color(foreground);
                    //result.Style["color"] = foreground.ToHtmlColor();
                }
                if (!string.IsNullOrEmpty(background)) {
                    result.BackgroundColor = new Color(background);
                    //result.Style["background-color"] = background.ToHtmlColor();
                }

                result.Weight = bold ? FontWeight.Bold : FontWeight.Regular;
                result.Italic = italic;

                return result;
            }

            void WriteFragment(Inlines inlines, string parsedSourceCode, IList<Scope> scopes) {
                if (scopes.Count == 0) {
                    inlines.Add(parsedSourceCode);
                } else {
                    scopes.SortStable((x, y) => x.Index.CompareTo(y.Index));
                    int offset = 0;
                    foreach (Scope scope in scopes) {
                        var length = scope.Index - offset;
                        if (length > 0) {
                            var text = parsedSourceCode.Substring(offset, scope.Index - offset);
                            inlines.Add(text);
                        }
                        var scopeInline = CreateInlineForScope(scope);
                        WriteFragment(scopeInline.Children, parsedSourceCode.Substring(scope.Index, scope.Length), scope.Children);
                        inlines.Add(scopeInline);
                        offset = scope.Index + scope.Length;
                    }

                    var rest = parsedSourceCode.Length - offset;
                    if (rest > 0) {
                        var text = parsedSourceCode.Substring(offset);
                        inlines.Add(text);
                    }
                }
            }

            languageParser.Parse(sourceCode, language, (parsedSourceCode, captures) => WriteFragment(result, parsedSourceCode, captures));

            return result;
        }

        protected override void Write(string parsedSourceCode, IList<Scope> scopes) {
        }
    }
}