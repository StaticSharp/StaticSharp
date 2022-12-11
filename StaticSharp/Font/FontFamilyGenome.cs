using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {
    public record FontFamilyGenome(string Name) : Genome<FontFamily> {
        public override FontFamily Create() {
            var fullCssUrl = GoogleFonts.MakeCssUrl(Name);

            var fullCssRequest = new HttpRequestGenome(
                GoogleFonts.MakeWoff2Request(fullCssUrl)
                ) {

            }.CreateOrGetCached();

            var result = new FontFamily(Name);

            var fontInfos = GoogleFonts.ParseCss(fullCssRequest.Text);
            foreach (var i in fontInfos) {
                var italicSubset = result.Members[i.Italic ? 1 : 0];
                var existing = italicSubset.Find(x => x.FontStyle.Weight == (FontWeight)i.Weight);
                if (existing != null) {
                    existing.Segments.AddRange(i.Segments);
                } else {
                    italicSubset.Add(new Font(result, new FontStyle((FontWeight)i.Weight, i.Italic), i.Segments));
                }
            }
            return result;
        }


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

        public class FontFamily {


            public string Name { get; }

            public FontFamily(string name) {
                Name = name;
            }

            public List<Font>[/*italic*/] Members { get;} = new[]{
                new List<Font>(),
                new List<Font>()
            };





            public Font FindFont(FontStyle fontStyle) {

                var weights = Members[fontStyle.Italic ? 1 : 0];
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