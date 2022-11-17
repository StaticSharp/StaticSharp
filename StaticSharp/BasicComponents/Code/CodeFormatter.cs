using ColorCode;
using ColorCode.Common;
using ColorCode.HTML.Common;
using ColorCode.Parsing;
using ColorCode.Styling;
using StaticSharp.Html;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace StaticSharp.Gears {
    public class CodeFormatter : CodeColorizerBase {
        /// <summary>
        /// Creates a <see cref="CodeFormatter"/>, for creating HTML to display Syntax Highlighted code.
        /// </summary>
        /// <param name="Style">The Custom styles to Apply to the formatted Code.</param>
        /// <param name="languageParser">The language parser that the <see cref="CodeFormatter"/> instance will use for its lifetime.</param>
        public CodeFormatter(StyleDictionary Style = null, ILanguageParser languageParser = null) : base(Style, languageParser) {
        }

        private TextWriter Writer { get; set; }

        /// <summary>
        /// Creates the HTML Markup, which can be saved to a .html file.
        /// </summary>
        /// <param name="sourceCode">The source code to colorize.</param>
        /// <param name="language">The language to use to colorize the source code.</param>
        /// <returns>Colorised HTML Markup.</returns>
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
            /*var buffer = new StringBuilder(sourceCode.Length * 2);

            using (TextWriter writer = new StringWriter(buffer)) {
                Writer = writer;
                WriteHeader(language);

                

                WriteFooter(language);

                writer.Flush();
            }

            return buffer.ToString();*/
        }

        protected override void Write(string parsedSourceCode, IList<Scope> scopes) {
            var styleInsertions = new List<TextInsertion>();

            foreach (Scope scope in scopes)
                GetStyleInsertionsForCapturedStyle(scope, styleInsertions);

            styleInsertions.SortStable((x, y) => x.Index.CompareTo(y.Index));

            int offset = 0;

            foreach (TextInsertion styleInsertion in styleInsertions) {
                var text = parsedSourceCode.Substring(offset, styleInsertion.Index - offset);
                Writer.Write(WebUtility.HtmlEncode(text));
                if (string.IsNullOrEmpty(styleInsertion.Text))
                    BuildSpanForCapturedStyle(styleInsertion.Scope);
                else
                    Writer.Write(styleInsertion.Text);
                offset = styleInsertion.Index;
            }

            Writer.Write(WebUtility.HtmlEncode(parsedSourceCode.Substring(offset)));
        }

        private void WriteFooter(ILanguage language) {
            Writer.WriteLine();
            WriteHeaderPreEnd();
            WriteHeaderDivEnd();
        }

        private void WriteHeader(ILanguage language) {
            WriteHeaderDivStart();
            WriteHeaderPreStart();
            Writer.WriteLine();
        }

        private void GetStyleInsertionsForCapturedStyle(Scope scope, ICollection<TextInsertion> styleInsertions) {
            styleInsertions.Add(new TextInsertion {
                Index = scope.Index,
                Scope = scope
            });

            foreach (Scope childScope in scope.Children)
                GetStyleInsertionsForCapturedStyle(childScope, styleInsertions);

            styleInsertions.Add(new TextInsertion {
                Index = scope.Index + scope.Length,
                Text = "</span>"
            });
        }

        private void BuildSpanForCapturedStyle(Scope scope) {
            string foreground = string.Empty;
            string background = string.Empty;
            bool italic = false;
            bool bold = false;

            if (Styles.Contains(scope.Name)) {
                Style style = Styles[scope.Name];

                foreground = style.Foreground;
                background = style.Background;
                italic = style.Italic;
                bold = style.Bold;
            }

            WriteElementStart("span", foreground, background, italic, bold);
        }

        private void WriteHeaderDivEnd() {
            WriteElementEnd("div");
        }

        private void WriteElementEnd(string elementName) {
            Writer.Write("</{0}>", elementName);
        }

        private void WriteHeaderPreEnd() {
            WriteElementEnd("pre");
        }

        private void WriteHeaderPreStart() {
            WriteElementStart("pre");
        }

        private void WriteHeaderDivStart() {
            string foreground = string.Empty;
            string background = string.Empty;

            if (Styles.Contains(ScopeName.PlainText)) {
                Style plainTextStyle = Styles[ScopeName.PlainText];

                foreground = plainTextStyle.Foreground;
                background = plainTextStyle.Background;
            }

            WriteElementStart("div", foreground, background);
        }

        private void WriteElementStart(string elementName, string foreground = null, string background = null, bool italic = false, bool bold = false) {
            Writer.Write("<{0}", elementName);

            if (!string.IsNullOrWhiteSpace(foreground) || !string.IsNullOrWhiteSpace(background) || italic || bold) {
                Writer.Write(" style=\"");

                if (!string.IsNullOrWhiteSpace(foreground))
                    Writer.Write("color:{0};", foreground.ToHtmlColor());

                if (!string.IsNullOrWhiteSpace(background))
                    Writer.Write("background-color:{0};", background.ToHtmlColor());

                if (italic)
                    Writer.Write("font-style: italic;");

                if (bold)
                    Writer.Write("font-weight: bold;");

                Writer.Write("\"");
            }

            Writer.Write(">");
        }
    }
    /*public static class CodeStatic {
        public static void Add<T>(this T collection, CodeBlock item) where T : IElementContainer {
            collection.AddElement(item);
        }
    }*/
}