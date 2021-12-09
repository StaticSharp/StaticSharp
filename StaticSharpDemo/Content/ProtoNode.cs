using StaticSharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpDemo.Content {

    public partial class ProtoNode {
        public Language Language { get; init; }

        public Dictionary<string, ProtoNode>? GetAllParallelNodes() 
            => Enum.GetValues(typeof(Language)).Cast<Language>()
                .ToDictionary(l => l.ToString().ToLower(), l => WithLanguage(l));


        protected T? SelectRepresentative<T>(IEnumerable<T> representatives) =>
            representatives.FirstOrDefault(r => r?.GetType().Name == Enum.GetName(Language) || r?.GetType().Name == "En");
    }

    public abstract partial class ProtoNode : StaticSharpEngine.INode {

        public ProtoNode(Language language) => Language = language;
        public abstract ProtoNode Parent { get; }
        INode INode.Parent => Parent;
        public abstract ProtoNode Root { get; }
        INode INode.Root => Root;

        IRepresentative? INode.Representative => Representative as IRepresentative;
        public abstract object Representative { get; }

        public abstract IEnumerable<ProtoNode> Children { get; }
        IEnumerable<INode> INode.Children => Children;

        public abstract string Name { get; }

        public abstract string[] Path { get; }

        public abstract ProtoNode WithLanguage(global::StaticSharpDemo.Language language);
    }
}