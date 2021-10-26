using CsmlWeb.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CsmlWeb {

    public interface IFont : IInclude {

        string GenerateUsageCss(Context context);
    }

    public enum FontWeight {
        Thin,
        ExtraLight,
        Light,
        Regular,
        Medium,
        SemiBold,
        Bold,
        ExtraBold,
        Black
    }

    public enum SafeFonts {
        Arial,
        TimesNewRoman,
        CourierNew,
    }

    public partial record Font : IFont, IKey, ICallerInfo {

        private readonly string _callerFilepath;
        private readonly int _callerLineNumber;

        public string CallerFilePath => _callerFilepath;
        public int CallerLineNumber => _callerLineNumber;
        private string Directory { get; }


        public static string[] WeightToNames(FontWeight weight) => weight switch {
            FontWeight.Thin => new[] { "thin", "100" },
            FontWeight.ExtraLight => new[] { "extralight", "200" },
            FontWeight.Light => new[] { "light", "300" },
            FontWeight.Regular => new[] { "", "regular", "400" },
            FontWeight.Medium => new[] { "medium", "500" },
            FontWeight.SemiBold => new[] { "semibold", "600" },
            FontWeight.Bold => new[] { "bold", "700" },
            FontWeight.ExtraBold => new[] { "extrabold", "800" },
            FontWeight.Black => new[] { "black", "900" },
            _ => throw new ArgumentOutOfRangeException(nameof(weight))
        };


        public const string ItalicName = "italic";

        public static readonly string[] Extensions = new[] { ".woff2", ".woff", ".ttf", ".eot" };

        public string Family { get; init; }
        public FontWeight Weight { get; init; }
        public bool Italic { get; init; }

        public string Style => Italic ? "italic" : "normal";


        public string Key => CsmlWeb.Key.Calculate(GetType(), Family, Weight, Italic);

        public Font(string directory, FontWeight weight, bool italic = false, [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0)
            : this(char.ToUpper(directory.Split('\\').LastOrDefault()[0]) + directory.Split('\\').LastOrDefault()[1..], weight, italic)
            => (Directory, _callerFilepath, _callerLineNumber) = (directory, CallerFilePath, CallerLineNumber);

        public Font(SafeFonts safeFonts, FontWeight weight, bool italic, [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0)
            : this(safeFonts.ToString(), weight, italic)
            => (_callerFilepath, _callerLineNumber) = (CallerFilePath, CallerLineNumber);


        private Font(string family, FontWeight weight, bool italic) => (Family, Weight, Italic) = (family, weight, italic);

        //GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(x => x.GetValue(this)).ToArray());
        //GetType().FullName+"/0"+ Family.Key+"/0"+ Weight + "/0";

        public string GenerateUsageCss(Context context) {
            context.Includes.RequireFont(this);
            return $"font-family:{Family}; font-weight:{WeightToNames(Weight).Last()};" + (Italic ? @" font-style: Italic;" : "");
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

            private static int ExtensionToPriority(string extension) => extension switch {
                ".woff2" => 0,
                ".woff" => 1,
                ".ttf" => 2,
                ".eot" => 3,
                _ => int.MaxValue
            };

            private string FindFilePath() {
                var directoryName = Font.Family.ToLower();

                bool MatchFileName(string filePath) {
                    var fileName = Path.GetFileNameWithoutExtension(filePath).ToLower();
                    if (!fileName.StartsWith(directoryName)) { return false; }
                    var parameters = fileName[directoryName.Length..].Trim(new[] { ' ', '-', '_' });
                    if (!Font.Italic) { return WeightToNames(Font.Weight).Any(x => x == parameters); }
                    if (!parameters.EndsWith(ItalicName)) { return false; }
                    parameters = parameters.Replace(ItalicName, "");
                    return WeightToNames(Font.Weight).Any(x => x == parameters);
                }

                if (!System.IO.Directory.Exists(Font.Directory)) {
                    throw new InvalidUsageException(Font);
                }
                var files = System.IO.Directory.EnumerateFiles(Font.Directory);
                files = files.OrderBy(x => ExtensionToPriority(Path.GetExtension(x).ToLower()));
                return files.FirstOrDefault(x => MatchFileName(x) && Extensions.Contains(Path.GetExtension(x).ToLower()));
            }


        }
    }
}