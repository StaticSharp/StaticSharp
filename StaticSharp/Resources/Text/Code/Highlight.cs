using StaticSharp.Gears;
using System;

namespace StaticSharp;

public partial class Static {

    public static Inlines Highlight(this Genome<IAsset> genome, bool dark = false) {
        return genome.Result.Highlight(dark);
    }

    public static Inlines Highlight(this IAsset asset, bool dark = false) {
        var programmingLanguageName = asset.Extension[1..];
        var language = ProgrammingLanguageProcessor.FindByName(programmingLanguageName);
        var content = asset.Text;
        return language.Highlight(content, programmingLanguageName, dark);
    }
}
