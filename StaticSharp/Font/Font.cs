using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {



    


    /*public abstract class AbstractFontFamily {
        public string Name { get; init; }


    }

    public class FontFamily : AbstractFontFamily {

        public string Directory { get; init; }
        public FontFamily(string directory) {
            Directory = directory;

            var name = Path.GetFileName(directory);
            Name = char.ToUpper(name[0]) + name[1..];

        }
    }
    public class SafeFontFamily : AbstractFontFamily {

        

        public SafeFontFamily(FontFamily fontFamily) {
            Name = fontFamily.ToString();
        }
    }

    public struct FontSettings {
        public FontWeight Weight;
        public bool Italic;
        public float Size;
    }*/

    public enum DefaultFont {
        Arial,
        TimesNewRoman,
        CourierNew,
    }

    public record Font(
            FontFamily FontFamily,
            FontStyle FontStyle
            ) : Gears.Genome<Font, CacheableFont> {



        /*        public Font(string directory, FontWeight weight = FontWeight.Regular, bool italic = false)
                    : this(directory, FamilyFromDirectory(directory), weight, italic) { }
                public Font(DefaultFont defaultFont, FontWeight weight = FontWeight.Regular, bool italic = false)
                    : this("", defaultFont.ToString(), weight, italic) { }*/


        

    }


    namespace Gears {
        public static partial class FontUtils {
            
            static UnicodeCategory[] NonRenderingCategories = new UnicodeCategory[] {
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate };
            static bool IsPrintable(char c) {
                if (char.IsWhiteSpace(c))
                    return false;
                if (NonRenderingCategories.Contains(char.GetUnicodeCategory(c)))
                    return false;
                return true;
            }

            public static HashSet<char> ToPrintableChars(this string text) {
                var result = new HashSet<char>();
                foreach (var c in text) {
                    if (IsPrintable(c))
                        result.Add(c);
                }
                return result;
            }
        }
    }

    


    public class CacheableFont : Cacheable<Font>, ICacheable<Font> {

        public HashSet<char> usedChars = new();
        private CacheableFontFamily cacheableFontFamily = null!;

        private FontFamilyMember fontFamilyMember = null!;

        protected override async Task CreateAsync() {
            cacheableFontFamily = await Genome.FontFamily.CreateOrGetCached();
            fontFamilyMember = cacheableFontFamily.FindMember(Genome.FontStyle);
        }



        public HashSet<char> AddChars(HashSet<char> chars) {
            
            var existingChars = fontFamilyMember.GetExistingChars(chars);

            usedChars.UnionWith(existingChars);

            chars = new HashSet<char>(chars);
            chars.ExceptWith(existingChars);

            return chars;
        }



        //public string Base64 { get; private set; } = null!;
        //public string StyleInclude { get; private set; } = null!;

        //public SecondaryTask<SKTypeface> Typeface { get; init; } = new();

        


        /*protected override async Task CreateAsync() {

            var family = await Arguments.FontFamily.CreateOrGetCached();
            var member = family.FindMember(Arguments.FontStyle);



            //todo: case with SafeFont

            
            var path = member.FilePath;
            var format = member.CssFormat;
            //var italicSuffix = member.Italic ? " Italic" : "";
            var fontStyle = member.Italic ? "italic" : "normal";



            Base64 = Convert.ToBase64String(await File.ReadAllBytesAsync(path));


            *//*using (FileStream fileStream = File.OpenRead(path)) {
                var typeface = SKTypeface.FromStream(fileStream);


                //var typeface = SKFontManager.Default.CreateTypeface(fileStream);
                Typeface.SetResult(typeface);
            }*//*


            SixLabors.Fonts.FontCollection fontCollection = new SixLabors.Fonts.FontCollection();
            MeasurerFontFamily = fontCollection.Add(File.OpenRead(path));


            //var font = family.CreateFont(16);

            //SixLabors.Fonts.TextMeasurer.Measure("Text to measure", new(font));

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("@font-face {");
            stringBuilder.Append("font-family: ").Append("abc"*//*family.Name*//*).Append(";");
            //stringBuilder.AppendLine($"src:local('{Arguments.Family} {Arguments.Weight}{italicSuffix}'),");
            stringBuilder.Append($"src: url(data:application/font-{format};charset=utf-8;base64,{Base64}) format('{format}');");
            stringBuilder.Append("font-weight: ").Append((int)Arguments.FontStyle.FontWeight).Append(";");
            stringBuilder.Append("font-style: ").Append(fontStyle).Append(";}");

            StyleInclude = stringBuilder.ToString();
        }*/


        /*public object GenerateUsageCss(Context context) {
            context.Includes.Require(this);
            return new {
                *//*FontFamily = Arguments.Family,
                FontWeight = WeightToNames(Arguments.Weight).Last(),
                FontStyle = Arguments.Italic ? "Italic" : null*//*
            };
        }*/

        public async Task<string> GenerateIncludeAsync() {


            var sortedUsedChars = usedChars.OrderBy(c => c);
            var text = string.Concat(sortedUsedChars);


            /*var fontFamily = await Genome.FontFamily.CreateOrGetCached();
            var fontFamilyMember = fontFamily.FindMember(Genome.FontStyle);
            fontFamilyMember.Weight*/
            

            var subfontCssUrl = GoogleFonts.MakeCssUrl(Genome.FontFamily.Name, fontFamilyMember.FontStyle, text);
            var subFontCssRequest = await new HttpRequestGenome(GoogleFonts.MakeWoff2Request(subfontCssUrl)).CreateOrGetCached();

            var fontInfos = GoogleFonts.ParseCss(subFontCssRequest.ReadAllText());
            //TODO validation
            var fontInfo = fontInfos.First();
            var subFontRequest = await new HttpRequestGenome(fontInfo.Url).CreateOrGetCached();

            var content = subFontRequest.ReadAllBites();

            var base64 = Convert.ToBase64String(content);
            var format = "woff2";
            var fontStyle = fontFamilyMember.FontStyle.CssFontStyle;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("@font-face {");
            stringBuilder.Append("font-family: ").Append(Genome.FontFamily.Name).Append(";");
            //stringBuilder.AppendLine($"src:local('{Arguments.Family} {Arguments.Weight}{italicSuffix}'),");
            stringBuilder.Append($"src: url(data:application/font-{format};charset=utf-8;base64,{base64}) format('{format}');");
            stringBuilder.Append("font-weight: ").Append((int)fontFamilyMember.FontStyle.Weight).Append(";");
            stringBuilder.Append("font-style: ").Append(fontStyle).Append(";}");

            return stringBuilder.ToString();            
        }

        
    }
}




    /*public partial record Font : IFont, IKey, ICallerInfo {

        private readonly string _callerFilepath;
        private readonly int _callerLineNumber;

        public string CallerFilePath => _callerFilepath;
        public int CallerLineNumber => _callerLineNumber;
        
        
        
        
        
        //private string Directory { get; }


        


        
        
        public string Family { get; init; }
        public FontWeight Weight { get; init; }
        public bool Italic { get; init; }

        public string Style => Italic ? "italic" : "normal";


        public string Key => StaticSharpWeb.Key.Calculate(GetType(), Family, Weight, Italic);

        public Font(
            string directory,
            FontWeight weight,
            bool italic = false,
            [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0) {

            Directory = directory;

            var family = Path.GetFileName(directory);
            Family = char.ToUpper(family[0]) + family[1..];

        }
            : this(char.ToUpper(directory.Split('\\').LastOrDefault()[0]) + directory.Split('\\').LastOrDefault()[1..], weight, italic)
            => (Directory, _callerFilepath, _callerLineNumber) = (directory, CallerFilePath, CallerLineNumber);

        public Font(SafeFontFamily.FontFamily safeFonts, FontWeight weight, bool italic, [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0)
            : this(safeFonts.ToString(), weight, italic)
            => (_callerFilepath, _callerLineNumber) = (CallerFilePath, CallerLineNumber);


        private Font(string family, FontWeight weight, bool italic) => (Family, Weight, Italic) = (family, weight, italic);

        //GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(x => x.GetValue(this)).ToArray());
        //GetType().FullName+"/0"+ Family.Key+"/0"+ Weight + "/0";

        public object GenerateUsageCss(Context context) {
            context.Includes.Require(this);


            return new {
                FontFamily = Family,
                FontWeight = WeightToNames(Weight).Last(),
                FontStyle = Italic ? "Italic" : null
            };
            //return $"font-family:{Family}; font-weight:{WeightToNames(Weight).Last()};" + (Italic ? @" font-style: Italic;" : "");
        }

        public async Task<string> GenerateAsync(IStorage storage) {
            var result = await storage.AddOrGetAsync(Key, () => new Storable(this));
            return result.Source;
        }
    }

    public partial record Font {

        private class Storable : IResource {
            private Font Font { get; init; }

            private string _base64;
            private string _resultString;

            private static IReadOnlyDictionary<string, string> Formant => new Dictionary<string, string>() {
                [".woff2"] = "woff2",
                [".woff"] = "woff",
                [".ttf"] = "truetype",
                [".eot"] = "embedded-opentype",
                [".svg"] = "svg",
            };

            public string Key => Font.Key;

            public string Source => _resultString;

            public Storable(Font font) => Font = font;

            public async Task GenerateAsync() {
                var stringBuilder = new StringBuilder();
                var path = FindFilePath();
                var format = Formant[Path.GetExtension(path)];
                var isItalic = Font.Italic ? " Italic" : "";

                _base64 = Convert.ToBase64String(await File.ReadAllBytesAsync(path));
                stringBuilder.AppendLine("@font-face {");
                stringBuilder.Append("font-family: '").Append(Font.Family).AppendLine("';");
                stringBuilder.AppendLine($"src:local('{Font.Family} {Font.Weight}{isItalic}'),");
                stringBuilder.AppendLine($"url(data:application/font-{format};charset=utf-8;base64,{_base64}) format('{format}');");
                stringBuilder.Append("font-weight: ").Append(Font.Weight).AppendLine(";");
                stringBuilder.Append("font-style: ").Append(Font.Style).AppendLine(";\n}");
                _resultString = stringBuilder.ToString();
            }

            

            


        }
    }*/
