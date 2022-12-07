using ColorCode;
using ColorCode.Styling;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StaticSharp {

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

    /*public interface ICodeResource : IResource {
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
    }*/
    #region Adf dfgdfff

    #region B
    public static partial class CodeUtils {

        
    }

#endregion
#endregion



    [RelatedStyle]
    //[RelatedStyle("Code")]
    [ConstructorJs]
    public class CodeBlock : ParagraphBase {
        protected override string TagName => "code-block";

        public string? ProgrammingLanguage { get; init; } = null;
        public string? RegionName { get; init; }
        
        protected Genome<IAsset> assetGenome;

        public CodeBlock(CodeBlock other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            ProgrammingLanguage = other.ProgrammingLanguage;
            RegionName = other.RegionName;
            assetGenome = other.assetGenome;
        }
        public CodeBlock(Genome<IAsset> assetGenome, string? programmingLanguage = null, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            this.assetGenome = assetGenome;
            ProgrammingLanguage = programmingLanguage;
        }

        public CodeBlock(string pathOrUrl, string? programmingLanguage = null, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            assetGenome = AssetGenome.GenomeFromPathOrUrl(pathOrUrl, callerFilePath);
            ProgrammingLanguage = programmingLanguage;
        }

        protected override async Task<Inlines> GetInlinesAsync() {
            var asset = assetGenome.CreateOrGetCached();
            var code = await asset.GetTextAsync();

            //var styleDictionary = CreateStyleDictionary();

            var styleDictionary = StyleDictionary.DefaultLight;

            var programmingLanguageName = ProgrammingLanguage ?? (await asset.GetFileExtensionAsync()).TrimStart('.');

            var languageProcessor = ProgrammingLanguageProcessor.FindByName(programmingLanguageName);

            return languageProcessor.Highlight(code, programmingLanguageName, false);
        }


        /*public CodeBlock(string filePath, ProgrammingLanguage programmingLanguage = default,
            [CallerFilePath] string callerFilePath = "") {
            UserDefinedProgrammingLanguage = programmingLanguage;
            if (!File.Exists(filePath)) {
                var error = $"File {filePath} not found";
                Log.Error.OnCaller(error);
                throw new FileNotFoundException(error);
            }
        }*/

        /*protected virtual ProgrammingLanguage ProgrammingLanguage {
            get {
                if (UserDefinedProgrammingLanguage != ProgrammingLanguage.Undefined) return UserDefinedProgrammingLanguage;
                if (ProgrammingLanguageBasedOnExtension != ProgrammingLanguage.Undefined) return ProgrammingLanguageBasedOnExtension;
                return ProgrammingLanguage.Undefined;
            }
        }*/

        /*private ProgrammingLanguage GetProgrammingLanguageByExtension(string extension)
            => extension.TrimStart('.').ToLower() switch {
                "cs" => ProgrammingLanguage.CSharp,
                "xml" => ProgrammingLanguage.Xml,
                "json" => ProgrammingLanguage.JavaScript,
                _ => throw new ArgumentException("Please add extension here")
            };*/

        //protected virtual string FinalSourceCode => _codeResource.Source;

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

        public static string NormalizeCode(string code) {

            /*var lines = Regex.Split(code, "\r\n|\r|\n");


            code = code.Replace("\r\n", "\n").Replace("\r", "\n");
            code = code.Replace("\t", "    ");*/

            return code;
        }

        protected override Context ModifyContext(Context context) {
            FontFamilies = context.CodeFontFamilies;
            return base.ModifyContext(context);
        }





            /*protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {

                context.FontFamilies = context.CodeFontFamilies;

                if (context.FontFamilies != null) {
                    elementTag.Style["font-family"] = string.Join(',', context.FontFamilies.Select(x => x.Name));
                }

                var inlines = await GetInlinesAsync();


                var pre = new Tag("p");
                foreach (var i in inlines) {
                    var child = await i.Value.GenerateHtmlAsync(context, new Role(true, i.Key));
                    pre.Add(child);
                }
                elementTag.Add(pre);

                await base.ModifyHtmlAsync(context, elementTag);
            }*/



            /*public async Task<Tag> GenerateHtmlAsync(Context context) {
                _codeResource ??= await context.Storage.AddOrGetAsync(_source, () => new CodeResource(_source));
                //throw new NotImplementedException();
                //var result = new Tag(null);

                //if ((Source is GitHub.File) && !context.AForbidden) {
                //    var hrefTargetUrl = (Source as GitHub.File).HtmlUri;

                //    var lineSpan = LineSpan;
                //    if (lineSpan.HasValue)
                //        hrefTargetUrl += $"#L{lineSpan.Value.Start.Value + 1}-L{lineSpan.Value.End.Value + 1}";

                //    result.Add(new Tag("a", new { Class = "GitHubLink", target = "_blank", href = hrefTargetUrl }));
                //}

                var programmingLanguage = ProgrammingLanguage;
                var languageCssClass = GetLanguageName(programmingLanguage);
                var code = FinalSourceCode;

                //https://github.com/WilliamABradley/ColorCode-Universal

                if (programmingLanguage == ProgrammingLanguage.Undefined) {
                    var tag = new Tag("div", new { Class = $"Code CodeBlock {languageCssClass}" }) {
                        new Tag("pre") { code }
                    };
                    tag.Add(new JSCall(AbsolutePath("Code.js")).Generate(context));
                    return tag;
                    // return new Tag("div", new { Class = $"Code CodeBlock {languageCssClass}" }) {
                    //     new Tag("pre") {
                    //         code
                    //     }
                    // };
                }

                var styleDictionary = CreateStyleDictionary();
                var formatter = new HtmlClassFormatter(styleDictionary);

                var html = formatter.GetHtmlString(code, LanguageToColorCode(programmingLanguage));

                string prefix = "<div class=\"";
                if (!html.StartsWith(prefix))
                    Log.Error.Here("Unexpected html");

                html = $"{prefix}Code CodeBlock {languageCssClass} {html[prefix.Length..]}";
                var result = new Tag("div");

                result.Add(new PureHtmlNode(html));
                context.Includes.Require(new Style(AbsolutePath(nameof(CodeBlock) + ".scss")));
                result.Add(new JSCall(AbsolutePath("Code.js")).Generate(context));
                return result;
            }*/

        }
    /*public static class CodeStatic {
        public static void Add<T>(this T collection, CodeBlock item) where T : IElementContainer {
            collection.AddElement(item);
        }
    }*/
}