using StaticSharp.Gears;
using System;

namespace StaticSharp;

public partial class Static {

    public static Inlines Highlight(this IAsset asset, bool dark = false) {
        var programmingLanguageName = asset.FileExtension[1..];
        var language = ProgrammingLanguageProcessor.FindByName(programmingLanguageName);
        var content = asset.Text;
        return language.Highlight(content, programmingLanguageName, dark);
    }
}
