using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Js {
        public interface BlockWithChildren : Block {
           public Enumerable<Block> Children { get; }
        }
    }

    namespace Gears {
        public class BlockWithChildrenBindings<FinalJs> : BlockBindings<FinalJs> {
        }
    }

    [Mix(typeof(BlockWithChildrenBindings<Js.BlockWithChildren>))]
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