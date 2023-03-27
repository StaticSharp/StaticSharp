using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Js {
        public interface MaterialShadow : Block {
            public double Elevation  { get; }
        }
    }


    namespace Gears {
        public class MaterialShadowBindings<FinalJs> : HierarchicalBindings<FinalJs>  {
            public Binding<double> Elevation { set { Apply(value); } }
        }
    }


    [Mix(typeof(MaterialShadowBindings<Js.MaterialShadow>))]
    [ConstructorJs]
    public partial class MaterialShadow : Hierarchical, IBlock, IInline {
        public MaterialShadow([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) { }
        public MaterialShadow(Hierarchical other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {}

        public string GetPlainText(Context context) {
            return "";
        }
    }
}
