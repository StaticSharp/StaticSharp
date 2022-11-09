

using StaticSharp.Tree;
using System.Collections.Generic;

namespace StaticSharp {

    namespace Tree {
        public abstract class Node {
            public abstract Node Root { get; }
            public abstract Node Parent { get; }
            public abstract Page? Representative { get; }

            public abstract IEnumerable<Node> Children { get; }

            public abstract string[] Path { get; }

            public abstract string Name { get; }
        }
    }

    public abstract class ProtoNode<FinalNode> : StaticSharp.Tree.Node where FinalNode : ProtoNode<FinalNode> {
        public override abstract FinalNode Parent { get; }
        //INode INode.Parent => Parent;
        public override abstract FinalNode Root { get; }
        //INode INode.Root => Root;

        //IRepresentative? INode.Representative => Representative as IRepresentative;


        public override abstract IEnumerable<FinalNode> Children { get; }
        //IEnumerable<INode> INode.Children => Children;




    }
}