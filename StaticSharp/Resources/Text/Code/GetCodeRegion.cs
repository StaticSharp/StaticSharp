using ImageMagick;
using StaticSharp.Gears;

namespace StaticSharp;


public partial class Static {

    public static Genome<IAsset> GetCodeRegion(this Genome<IAsset> genome, string RegionName, bool trim = true) {
        return new CodeRegionGenome(genome, RegionName, trim);

        /*var programmingLanguageName = asset.FileExtension[1..];
        var language = ProgrammingLanguageProcessor.FindByName(programmingLanguageName);
        var content = asset.Text;
        content = language.GetRegion(content, RegionName);
        if (content == null) {
            throw new Exception($"Region {RegionName} not found.");
        }

        if (trim) {
            content= content.TrimEmptyLines().Untab();
        }

        return new TextAsset(
            content,
            asset.FileExtension
            );*/
    }
}
