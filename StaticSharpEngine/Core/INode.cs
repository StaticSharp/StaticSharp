using System;
using System.Collections.Generic;

namespace StaticSharpEngine {

    public interface INode {
        INode Parent { get; }
        INode Root { get; }
        IRepresentative Representative { get; }
        IEnumerable<INode> Children { get; }
        string[] Path { get; }
        string Name { get; }
    }

}
