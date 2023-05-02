using StaticSharp.Gears;
using StaticSharp.Html;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public interface JLinearLayout : JBlock {
        bool Vertical { get; set; }
        double ItemGrow { get; set; }
        bool Reverse { get; set; }
        double Gap { get; set; }
        double GapGrow { get; set; }
        double? PrimaryGravity { get; set; }
        double? SecondaryGravity { get; set; }
        double InternalWidth { get;  }
        double InternalHeight { get;  }
    }



    [Scripts.LayoutUtils]
    [ConstructorJs]
    public partial class LinearLayout : BlockWithChildren {

    }
}