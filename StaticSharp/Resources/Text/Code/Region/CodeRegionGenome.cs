using StaticSharp.Gears;
using StaticSharp.Resources.Text;
using System;
using System.Text.RegularExpressions;

namespace StaticSharp {


    public static class CodeRegionGenomeExtension {
        public static Genome<IAsset> GetCodeRegion(this Genome<IAsset> genome, string RegionName, bool trim = true) {
            return new CodeRegionGenome(genome, RegionName, trim);
        }
    }


    namespace Gears {

        public record CodeRegionGenome(Genome<IAsset> Source, string RegionName, bool Trim = true) : TextProcessorGenome(Source) {

            private static Regex NamedRegionStartRegex = new Regex(@"#.*?region\s+([\w\s]*[\w])", RegexOptions.IgnoreCase);
            private static Regex RegionEndRegex = new Regex(@"#.*?endregion", RegexOptions.IgnoreCase);


            protected override string Process(string text, ref string extension) {
                var result = text;
                result = GetRegion(result, RegionName);
                if (result == null) {
                    throw new Exception($"Region {RegionName} not found.");
                }
                if (Trim) {
                    result = result.TrimEmptyLines().Untab();
                }
                return result;
            }

            private static string? GetRegion(string code, string regionName) {
                var resultLines = new List<string>();

                var lines = code.SplitLines();
                for (int start = 0; start < lines.Length; start++) {
                    var matchStart = NamedRegionStartRegex.Match(lines[start]);
                    if (!matchStart.Success) continue;
                    if (matchStart.Groups[1].Value == regionName) {
                        for (int i = start + 1; i < lines.Length; i++) {
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


        }
    }
}