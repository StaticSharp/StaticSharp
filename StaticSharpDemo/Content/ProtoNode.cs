using StaticSharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpDemo.Content {

    public partial class ProtoNode {
        public Language Language { get; init; }

        protected T SelectRepresentative<T>(IEnumerable<T> representatives) {
            foreach (var r in representatives) {
                if (r.GetType().Name == Enum.GetName(Language))
                    return r;
            }
            var fallback = representatives.FirstOrDefault(x => x.GetType().Name == "En");
            if (fallback != null) return fallback;
            return representatives.FirstOrDefault();
        }
    }

    public abstract partial class ProtoNode : StaticSharpEngine.INode {

        public ProtoNode(Language language) {
            Language = language;
        }

        public abstract ProtoNode Parent { get; }
        INode INode.Parent => Parent;
        public abstract ProtoNode Root { get; }
        INode INode.Root => Root;

        IRepresentative INode.Representative => Representative as IRepresentative;
        public abstract object Representative { get; }

        public abstract IEnumerable<ProtoNode> Children { get; }
        IEnumerable<INode> INode.Children => Children;

        public abstract string Name { get; }

        public abstract string[] Path { get; }

        public abstract ProtoNode WithLanguage(global::StaticSharpDemo.Language language);
    }
}