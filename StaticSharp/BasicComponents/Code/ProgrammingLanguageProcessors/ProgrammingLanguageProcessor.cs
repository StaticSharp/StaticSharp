using ColorCode;
using ColorCode.Styling;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StaticSharp {


    


    public record CodeRegionGenome(Genome<IAsset> Source, string RegionName) : Genome<IAsset> {

        public override Genome[]? Sources => new Genome[] { Source };
        async Task<IAsset> CreateAssetAsync(IAsset sourceAsset) {
            var extension = await sourceAsset.GetFileExtensionAsync();
            var language = ProgrammingLanguageProcessor.FindByName(extension);
            string content = await sourceAsset.GetTextAsync();
            content = language.GetRegion(content, RegionName);            

            


            return new TextAsset(
                content,
                extension,
                await sourceAsset.GetMediaTypeAsync()
                );

        }

        public override IAsset Create() {
            var source = Source.CreateOrGetCached();

            return new AsyncAsset(CreateAssetAsync(source));



            

            /*return new Asset(
                ()=> Encoding.UTF8.GetBytes(content),
                source.FileExtension,
                source.MediaType,
                Hash.CreateFromString(content).ToString(),
                source.CharSet
                );*/

        }
    }

    /*namespace Gears {
        public class CodeRegionAsset : Cacheable<CodeRegionGenome>, Asset {

            public class Data {
                public DateTime LastWriteTime;
                public string ContentHash = null!;
            };

            public string MediaType { get; private set; } = null!;
            public string FileExtension { get; private set; } = null!;
            public string ContentHash { get; private set; } = null!;
            public string? CharSet { get; private set; } = null;
            public string Content { get; private set; } = null!;

            protected override async Task CreateAsync() {
                var source = await Genome.Source.CreateOrGetCached();
                MediaType = source.MediaType;
                FileExtension = source.FileExtension;
                CharSet = source.CharSet;


                var language = ProgrammingLanguageProcessor.FindByName(FileExtension);
                Content = source.ReadAllText();
                Content = language.GetRegion(Content, Genome.RegionName);

            }

            public byte[] ReadAllBites() {
                return Encoding.UTF8.GetBytes(Content);
            }

            public string ReadAllText() {
                return Content;
            }
        }
    }*/
}


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