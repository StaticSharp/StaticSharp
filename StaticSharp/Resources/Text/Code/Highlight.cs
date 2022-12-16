using StaticSharp.Gears;
using System;

namespace StaticSharp;

public partial class Static {



    public static Inlines Highlight(this Genome<IAsset> genome, bool dark = false) {
        return genome.Result.Highlight(dark);
    }

    public static Inlines Highlight(this IAsset asset, bool dark = false) {
        var programmingLanguageName = asset.Extension[1..];
        var programmingLanguageProcessor = ProgrammingLanguageProcessor.FindByName(programmingLanguageName);
        var content = asset.Text;
        return programmingLanguageProcessor.Highlight(content, programmingLanguageName, dark);
    }

    public static Inlines Highlight(this string code, string programmingLanguage, bool dark = false) {
        var programmingLanguageProcessor = ProgrammingLanguageProcessor.FindByName(programmingLanguage);
        return programmingLanguageProcessor.Highlight(code, programmingLanguage, dark);
    }


}
