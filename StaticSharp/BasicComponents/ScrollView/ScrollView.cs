using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    public interface JScrollView : JBlock {
        Block Child { get; }
        double ScrollX { get; set; }
        double ScrollY { get; set; }
        double InternalWidth { get; }
        double InternalHeight { get; }
    }

    [ConstructorJs]
    public partial class ScrollView : Block {

        [Socket]
        public required Block Child { get; set; }
        public ScrollView(ScrollView other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            Child = other.Child;
        }

    }

}