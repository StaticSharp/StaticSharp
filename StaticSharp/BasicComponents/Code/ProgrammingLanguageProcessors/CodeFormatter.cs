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

        public Tag GetHtmlString(string sourceCode, ILanguage language) {

            var result = new Tag("pre");

            Tag CreateTagForScope(Scope scope) {
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
                var result = new Tag("span") {
                };
                if (!string.IsNullOrEmpty(foreground)) {
                    result.Style["color"] = foreground.ToHtmlColor();
                }
                if (!string.IsNullOrEmpty(background)) {
                    result.Style["background-color"] = background.ToHtmlColor();
                }
                if (italic) {
                    result.Style["font-style"] = "italic";
                }
                if (bold) {
                    result.Style["font-weight"] = "bold";
                }

                return result;
            }

            void WriteFragment(Tag tag, string parsedSourceCode, IList<Scope> scopes) {
                if (scopes.Count == 0) {
                    tag.Add(parsedSourceCode);
                } else {
                    scopes.SortStable((x, y) => x.Index.CompareTo(y.Index));
                    int offset = 0;
                    foreach (Scope scope in scopes) {
                        var length = scope.Index - offset;
                        if (length > 0) {
                            var text = parsedSourceCode.Substring(offset, scope.Index - offset);
                            tag.Add(text);
                        }
                        var scopeTag = CreateTagForScope(scope);
                        WriteFragment(scopeTag, parsedSourceCode.Substring(scope.Index, scope.Length), scope.Children);
                        tag.Add(scopeTag);
                        offset = scope.Index + scope.Length;
                    }

                    var rest = parsedSourceCode.Length - offset;
                    if (rest > 0) {
                        var text = parsedSourceCode.Substring(offset);
                        tag.Add(text);
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