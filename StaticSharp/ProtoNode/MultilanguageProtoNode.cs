using StaticSharp.Gears;
using StaticSharp.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharp;

public abstract class MultilanguageProtoNode<LanguageEnum> : ProtoNode<MultilanguageProtoNode<LanguageEnum>> where LanguageEnum : struct, Enum {
    public LanguageEnum Language { get; init; }

    public MultilanguageProtoNode(LanguageEnum language) => Language = language;

    public IEnumerable<MultilanguageProtoNode<LanguageEnum>> GetAllParallelNodes()
        => Enum.GetValues<LanguageEnum>().Select(l => WithLanguage(l));

    protected T? SelectRepresentative<T>(IEnumerable<T> representatives) {
        var findedLanguage = representatives.FirstOrDefault(r => r?.GetType().Name == Enum.GetName(Language));
        if (findedLanguage is null) {
            findedLanguage = representatives.FirstOrDefault(r => r?.GetType().Name == "En");
        }
        if (findedLanguage is null) {
            findedLanguage = representatives.FirstOrDefault();
        }
        return findedLanguage;
    }
    public abstract MultilanguageProtoNode<LanguageEnum> WithLanguage(LanguageEnum language);


}
