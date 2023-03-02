using StaticSharp.Gears;
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
    namespace Js {
        public interface Layout : Block {
            public bool Vertical { get; }
            public double PrimaryGap { get; }
            public double PrimaryGapGrow { get; }
            public double PrimaryGravity { get; }
            public double IntralinearGravity { get; }
            public double SecondaryGap { get; }
            public double SecondaryGapGrow { get; }
            public bool FillSecondary { get; }
            public bool Multiline { get; }
        }
    }

    namespace Gears {
        public class LayoutBindings<FinalJs> : BlockBindings<FinalJs> {
            public Binding<bool> Vertical { set { Apply(value); } }
            public Binding<double> PrimaryGap { set { Apply(value); } }
            public Binding<double> PrimaryGapGrow { set { Apply(value); } }
            public Binding<double> PrimaryGravity { set { Apply(value); } }
            public Binding<double> IntralinearGravity { set { Apply(value); } }
            public Binding<double> SecondaryGap { set { Apply(value); } }
            public Binding<double> SecondaryGapGrow { set { Apply(value); } }
            public Binding<bool> FillSecondary { set { Apply(value); } }
        }
    }

    [RelatedScript("../FrontendUtils/LayoutUtils")]
    [Mix(typeof(LayoutBindings<Js.Layout>))]
    [ConstructorJs]
    public partial class Layout : Block {
        public Layout(Layout other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
        }
        public Layout([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }
    }
}