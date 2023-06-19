using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharp.Js;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public interface JBlockWithChildren : JBlock {
        public Enumerable<JBlock> Children { get; }
    }

    [ConstructorJs]
    public partial class BlockWithChildren : Block {

        [Socket]
        public virtual Blocks Children { get; } = new();

        public BlockWithChildren(BlockWithChildren other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            Children = new(other.Children);
        }

    }
}