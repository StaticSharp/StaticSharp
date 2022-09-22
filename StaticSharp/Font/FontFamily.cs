using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StaticSharp {
    public record FontFamily(
            string Name

            ) : Gears.Genome<FontFamily,Gears.CacheableFontFamily> {

        private static string FamilyFromDirectory(string directory) {
            var family = Path.GetFileName(directory);
            return char.ToUpper(family[0]) + family[1..];
        }

    }


    namespace Gears {

        public enum FontExtension {
            woff2,
            woff,
            ttf,
            //eot,
        }

        public class CacheableFontFamily : Cacheable<FontFamily> {

            private List<FontFamilyMember>[/*italic*/] members = new[]{
                new List<FontFamilyMember>(),
                new List<FontFamilyMember>()
            };





            public FontFamilyMember FindMember(FontStyle fontStyle) {

                var weights = members[fontStyle.Italic ? 1 : 0];
                var difWithPrevious = Math.Abs((int)fontStyle.Weight - (int)weights[0].FontStyle.Weight);

                var selectedIndex = weights.Count - 1;

                for (int i = 1; i < weights.Count; i++) {
                    var difWithCurent = Math.Abs((int)fontStyle.Weight - (int)weights[i].FontStyle.Weight);
                    if ((int)weights[i].FontStyle.Weight >= (int)fontStyle.Weight) {
                        selectedIndex = (difWithPrevious < difWithCurent) ? i - 1 : i;
                        break;
                    }
                    difWithPrevious = difWithCurent;

                }
                return weights[selectedIndex];
            }

            protected override void SetGenome(FontFamily arguments) {
                base.SetGenome(arguments);
                //Name = FamilyFromDirectory(Arguments.Directory);
            }


            protected override async Task CreateAsync() {

                var fullCssUrl = GoogleFonts.MakeCssUrl(Genome.Name);
                var fullCssRequest = await new HttpRequestGenome(
                    GoogleFonts.MakeWoff2Request(fullCssUrl)
                    ) {

                }.CreateOrGetCached();

                var fontInfos = GoogleFonts.ParseCss(fullCssRequest.ReadAllText());
                foreach (var i in fontInfos) {
                    var italicSubset = members[i.Italic ? 1 : 0];
                    var existing = italicSubset.Find(x => x.FontStyle.Weight == (FontWeight)i.Weight);
                    if (existing != null) {
                        existing.Segments.AddRange(i.Segments);
                    } else {
                        italicSubset.Add(new FontFamilyMember(new FontStyle((FontWeight)i.Weight, i.Italic), i.Segments));
                    }                    
                }


                /*if (!Directory.Exists(Arguments.Directory)) {
                    throw new DirectoryNotFoundException(Arguments.Directory);
                }
                var lName = Name.ToLower();

                foreach (var i in Directory.EnumerateFiles(Arguments.Directory)) {

                    var extensionName = Path.GetExtension(i)[1..].ToLower();
                    FontExtension extension;
                    if (!Enum.TryParse(extensionName, out extension))
                        continue;


                    var fileName = Path.GetFileNameWithoutExtension(i);
                    var lFileName = fileName.ToLower();
                    if (lFileName.StartsWith(lName))
                        lFileName = lFileName[lName.Length..].Trim('-', '_', ' ');


                    bool italic = lFileName.Contains("italic");
                    if (italic)
                        lFileName = lFileName.Replace("italic", "").Trim('-', '_', ' ');

                    FontWeight? weight = WeightFromName(lFileName);
                    if (weight == null)
                        continue;

                    var replacementCandidateIndex = members[italic ? 1 : 0].FindIndex(x => x.Weight == weight);

                    //var existing = members[italic ? 1 : 0].FirstOrDefault(x=>x.Weight == weight);
                    if (replacementCandidateIndex >= 0) {
                        if (members[italic ? 1 : 0][replacementCandidateIndex].Extension < extension) {
                            continue;
                        }                    
                    }

                    var newMember = new FontFamilyMember(
                        FilePath: i,
                        Extension: extension,
                        Italic: italic,
                        Weight: weight.Value
                    );
                    if (replacementCandidateIndex >= 0) {
                        members[italic ? 1 : 0][replacementCandidateIndex] = newMember;
                    } else {
                        members[italic ? 1 : 0].Add(newMember);
                    }
                }

                members[0].Sort((a, b) => a.Weight.CompareTo(b.Weight));
                members[1].Sort((a, b) => a.Weight.CompareTo(b.Weight));*/
            }

            

            private static FontWeight? WeightFromName(string name) {
                if (string.IsNullOrEmpty(name))
                    return FontWeight.Regular;

                int number;
                if (int.TryParse(name, out number)) {
                    //if (Enum.GetValues<FontWeight>().Contains((FontWeight)number))
                    return (FontWeight)number;
                    //throw new Ex
                }

                FontWeight byName;
                if (Enum.TryParse(name,true, out byName)) {
                    return byName;
                }

                return null;

                /*public static string[] WeightToNames(FontWeight weight) => weight switch {
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
                };*/

            }

            

        }

        //internal class FontFamilyConstants

    }

}