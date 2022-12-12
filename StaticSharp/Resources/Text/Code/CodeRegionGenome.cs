using StaticSharp.Gears;
using System;

namespace StaticSharp.Gears;

public record CodeRegionGenome(Genome<IAsset> Source, string RegionName, bool Trim = true) : TextProcessorGenome(Source) {
    protected override string Process(string text, ref string extension) {
        var programmingLanguageName = extension[1..];
        var language = ProgrammingLanguageProcessor.FindByName(programmingLanguageName);
        var result = text;
        result = language.GetRegion(result, RegionName);
        if (result == null) {
            throw new Exception($"Region {RegionName} not found.");
        }
        if (Trim) {
            result = result.TrimEmptyLines().Untab();
        }
        return result;
    }
}
