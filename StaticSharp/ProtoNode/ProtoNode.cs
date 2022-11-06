

using StaticSharp.Tree;
using System.Collections.Generic;

namespace StaticSharp;


public abstract class ProtoNode<FinalNode> : INode where FinalNode: ProtoNode<FinalNode> {    
    public abstract FinalNode Parent { get; }
    INode INode.Parent => Parent;
    public abstract FinalNode Root { get; }
    INode INode.Root => Root;

    //IRepresentative? INode.Representative => Representative as IRepresentative;
    public abstract Page? Representative { get; }

    public abstract IEnumerable<FinalNode> Children { get; }
    IEnumerable<INode> INode.Children => Children;

    public abstract string Name { get; }

    public abstract string[] Path { get; }    
}