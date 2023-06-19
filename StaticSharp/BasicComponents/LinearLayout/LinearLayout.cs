using StaticSharp.Gears;
using StaticSharp.Html;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public enum LinearLayoutOverflow { 
        Shrink,
        Remove,
        None
    }

    public interface JLinearLayout : JBlockWithChildren {
        bool Vertical { get; set; }
        double ItemGrow { get; set; }
        bool Reverse { get; set; }
        double Gap { get; set; }
        double GapGrow { get; set; }
        double StartGapGrow { get; set; }
        double EndGapGrow { get; set; }
        double? PrimaryGravity { get; set; }
        double? SecondaryGravity { get; set; }
        double InternalWidth { get;  }
        double InternalHeight { get;  }
        LinearLayoutOverflow Overflow { get; set; }
        public JBlock Ellipsis { get; }
    }



    [Scripts.LayoutUtils]
    [ConstructorJs]
    public partial class LinearLayout : BlockWithChildren {
        [Socket]
        public Block? Ellipsis { get; set; } = null;
        protected LinearLayout(LinearLayout other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
            Ellipsis = other.Ellipsis;
        }
    }
}