

using StaticSharp.Tree;
using System.Collections.Generic;

namespace StaticSharp;


public abstract class ProtoNode : INode {    
    public abstract ProtoNode Parent { get; }
    INode INode.Parent => Parent;
    public abstract ProtoNode Root { get; }
    INode INode.Root => Root;

    //IRepresentative? INode.Representative => Representative as IRepresentative;
    public abstract Page Representative { get; }

    public abstract IEnumerable<ProtoNode> Children { get; }
    IEnumerable<INode> INode.Children => Children;

    public abstract string Name { get; }

    public abstract string[] Path { get; }    
}