using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharp.Js;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public interface JBlockWithChildren : JBlock {
        public Enumerable<Block> Children { get; }
    }

    namespace Gears {
        public class BlockWithChildrenBindings<FinalJs> : BlockBindings<FinalJs> {
        }
    }

    [Mix(typeof(BlockWithChildrenBindings<JBlockWithChildren>))]
    [ConstructorJs]
    public partial class BlockWithChildren : Block {

        [Socket]
        public virtual Blocks Children { get; } = new();

        public BlockWithChildren(BlockWithChildren other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            Children = new(other.Children);
        }
        public BlockWithChildren([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

    }
}