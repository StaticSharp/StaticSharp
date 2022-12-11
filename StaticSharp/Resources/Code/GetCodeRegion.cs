using StaticSharp.Gears;
using System;

namespace StaticSharp;

public partial class Static {

    public static IAsset GetCodeRegion(this IAsset asset, string RegionName, bool trim = true) {
        var programmingLanguageName = asset.FileExtension[1..];
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
            );
    }
}
