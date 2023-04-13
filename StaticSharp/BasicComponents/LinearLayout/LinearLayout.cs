using StaticSharp.Gears;
using StaticSharp.Html;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace StaticSharp {
    /*
     Vertical: false,
        PrimaryGap: 0,
        PrimaryGapGrow: 0,
        PrimaryGravity: -1,

        IntralinearGravity: 0,
        SecondaryGap: 0,
        SecondaryGapGrow: 1,
        FillSecondary: true,

        Multiline: false
     
     */
    public interface JLinearLayout : JBlock {
        public bool Vertical { get; }
        public double ItemGrow { get; }
        public double Gap { get; }
        public double GapGrow { get; }
        public double? PrimaryGravity { get; }
        public double? SecondaryGravity { get; }
        //public bool FillSecondary { get; }

        public double InternalWidth { get; }
        public double InternalHieght { get; }
    }

    namespace Gears {
        public class LinearLayoutBindings<FinalJs> : BlockBindings<FinalJs> {
            public Binding<bool> Vertical { set { Apply(value); } }
            public Binding<bool> Reverse { set { Apply(value); } }
            public Binding<double> ItemGrow { set { Apply(value); } }
            public Binding<double> Gap { set { Apply(value); } }
            public Binding<double> GapGrow { set { Apply(value); } }
            public Binding<double?> PrimaryGravity { set { Apply(value); } }
            public Binding<double?> SecondaryGravity { set { Apply(value); } }
            //public Binding<bool> FillSecondary { set { Apply(value); } }
        }
    }

    [Scripts.LayoutUtils]
    //[RelatedScript("../FrontendUtils/LayoutUtils")]
    [Mix(typeof(LinearLayoutBindings<JLinearLayout>))]
    [ConstructorJs]
    public partial class LinearLayout : BlockWithChildren {

        

        public LinearLayout(LinearLayout other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
        }
        public LinearLayout([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        


    }
}