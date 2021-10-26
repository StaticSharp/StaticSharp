using ColorCode;
using ColorCode.Styling;
using CsmlWeb.Html;
using CsmlWeb.Resources;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CsmlWeb.Components {

    public enum ProgrammingLanguage {
        Undefined = 0,
        PowerShell,
        Haskell,
        Koka,
        FSharp,
        Typescript,
        Cpp,
        Css,
        Php,
        Xml,
        VbDotNet,
        Sql,
        Markdown,
        Fortran,
        Java,
        Html,
        CSharp,
        AspxVb,
        AspxCs,
        Aspx,
        Asax,
        Ashx,
        JavaScript
    }

    public interface ICodeResource : IResource {

    }


    public class CodeResource : ICodeResource {
        private readonly string _inputFilePath;
        private string _code;
        private string _extension;

        public CodeResource(string filePath) => _inputFilePath = filePath;

        public string Key => _inputFilePath;

        public string Source => _code;

        public string Extension => _extension;

        public async Task GenerateAsync() {

            if (File.Exists(_inputFilePath)) {
                _extension = Path.GetExtension(_inputFilePath);
                _code = await File.ReadAllTextAsync(_inputFilePath);
            } else {
                Log.Error.OnCaller($"File {_inputFilePath} not found");
            }
        }
    }






    public class Code : IBlock, IInline {
        private readonly ProgrammingLanguage UserDefinedProgrammingLanguage;
        private readonly ProgrammingLanguage ProgrammingLanguageBasedOnExtension;
        private ICodeResource CodeResource { get; }


        private readonly string _source;
        public string Source => _source ?? CodeResource?.Source;

        protected Code() { }
        protected Code(string source) => _source = source;
        protected Code(Code other) : this() => CodeResource = other.CodeResource;
        public Code(string source, ProgrammingLanguage programmingLanguage = ProgrammingLanguage.Undefined) {
            UserDefinedProgrammingLanguage = programmingLanguage;
            _source = source;
        }

        public Code(string filePath, ProgrammingLanguage programmingLanguage = default, 
            [CallerFilePath] string callerFilePath = "") {

            UserDefinedProgrammingLanguage = programmingLanguage;
            if (!File.Exists(filePath)) {
                var error = $"File {filePath} not found";
                Log.Error.OnCaller(error);
                throw new FileNotFoundException(error);
            }
        }


        protected virtual ProgrammingLanguage ProgrammingLanguage {
            get {
                if (UserDefinedProgrammingLanguage != ProgrammingLanguage.Undefined) return UserDefinedProgrammingLanguage;
                if (ProgrammingLanguageBasedOnExtension != ProgrammingLanguage.Undefined) return ProgrammingLanguageBasedOnExtension;
                return ProgrammingLanguage.Undefined;
            }
        }

        private ProgrammingLanguage GetProgrammingLanguageByExtension(string extension)
            => extension.TrimStart('.').ToLower() switch {
                "cs" => ProgrammingLanguage.CSharp,
                "xml" => ProgrammingLanguage.Xml,
                "json" => ProgrammingLanguage.JavaScript,
                _ => throw new ArgumentException("Please add extension here")
            };



        protected virtual string FinalSourceCode => CodeResource.Source;

        protected virtual Range? LineSpan => null;

        private ILanguage LanguageToColorCode(ProgrammingLanguage programmingLanguage) {
            var name = GetLanguageName(programmingLanguage);
            var allColorCodeLanguages = typeof(Languages).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.PropertyType == typeof(ILanguage));
            return (ILanguage)allColorCodeLanguages.FirstOrDefault(x => x.Name == name)?.GetValue(null);
        }

        private string GetLanguageName(ProgrammingLanguage lang)
            => Enum.GetName(typeof(ProgrammingLanguage), lang);

        private StyleDictionary CreateStyleDictionary() {
            static string CapitalizeFirstLetter(string name) 
                => string.IsNullOrEmpty(name) ? name : char.ToUpper(name[0]) + name[1..];

            var result = new StyleDictionary();
            foreach (var style in StyleDictionary.DefaultLight) {
                var newStyle = new ColorCode.Styling.Style(style.ScopeName) {
                    Background = style.Background,
                    Foreground = style.Foreground,
                    Italic = style.Italic,
                    Bold = style.Bold,
                    ReferenceName = CapitalizeFirstLetter(style.ReferenceName),
                };

                result.Add(newStyle);
            }

            return result;
        }

        public static string SpacesOrTabsOnly(string x) {
            string result = "";
            for (int i = 0; i < x.Length; i++) {
                if ((x[i] == ' ') | (x[i] == '\t')) {
                    result += x[i];
                }
            }
            return result;
        }

        public static string Untab(string code, string tabs) {
            if (string.IsNullOrEmpty(tabs)) { return code; }
            var stringBuilder = new StringBuilder();
            using var reader = new StringReader(code);
            var line = string.Empty;
            do {
                line = reader.ReadLine();
                if (line != null) {
                    if (line.StartsWith(tabs)) {
                        line = line[tabs.Length..];
                    }
                    stringBuilder.AppendLine(line);
                }
            } while (line != null);

            return stringBuilder.ToString();
        }

        public static string Untab(string code) {
            string tabs = string.Concat(code.TakeWhile(c => char.IsWhiteSpace(c) && c != '\r' && c != '\n'));
            return Untab(code, tabs);
        }

        public static string TrimEmptyLines(string code) {
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



        public Task<INode> GenerateBlockHtmlAsync(Context context) {
            throw new NotImplementedException();
            //var result = new Tag(null);

            //if ((Source is GitHub.File) && !context.AForbidden) {
            //    var hrefTargetUrl = (Source as GitHub.File).HtmlUri;

            //    var lineSpan = LineSpan;
            //    if (lineSpan.HasValue)
            //        hrefTargetUrl += $"#L{lineSpan.Value.Start.Value + 1}-L{lineSpan.Value.End.Value + 1}";

            //    result.Add(new Tag("a")
            //        .AddClasses("GitHubLink")
            //        .Attribute("target", "_blank")
            //        .Attribute("href", hrefTargetUrl));
            //}

            //var programmingLanguage = ProgrammingLanguage;
            //var languageCssClass = GetLanguageName(programmingLanguage);
            //var code = FinalSourceCode;

            ////https://github.com/WilliamABradley/ColorCode-Universal

            //if (programmingLanguage == ProgrammingLanguage.Undefined) {
            //    return new Tag("div")
            //        .AddClasses("Code", "CodeBlock", languageCssClass)
            //        .Add(new Tag("pre").AddText(code));
            //}

            //var styleDictionary = CreateStyleDictionary();
            //var formatter = new ColorCode.HtmlClassFormatter(styleDictionary);

            //var html = formatter.GetHtmlString(code, LanguageToColorCode(programmingLanguage));

            //string prefix = "<div class=\"";
            //if (!html.StartsWith(prefix))
            //    Log.Error.Here("Unexpected html");

            //html = $"{prefix}Code CodeBlock {languageCssClass} {html[prefix.Length..]}";

            //result.Add(new PureHtmlNode(html));
            //return result;
        }

        public Task<INode> GenerateInlineHtmlAsync(Context context) {
            throw new NotImplementedException();
        }
    }
}