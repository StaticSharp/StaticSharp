using StaticSharp.Gears;
using StaticSharp.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpDemo.Root {

    public enum Language {
        En,
        Ru
    }

    public abstract class ProtoNode : MultilanguageProtoNode<Language> {
        protected ProtoNode(Language language) : base(language) {
        }
    }

}