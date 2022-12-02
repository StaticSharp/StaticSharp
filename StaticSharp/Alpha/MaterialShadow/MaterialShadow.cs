using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public class MaterialShadow : Block {
            public double Elevation => NotEvaluatableValue<double>();
        }
    }


    namespace Gears {
        public class MaterialShadowBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
            public Binding<double> Elevation { set { Apply(value); } }
        }
    }


    [Mix(typeof(MaterialShadowBindings<Js.MaterialShadow>))]
    [ConstructorJs]
    public partial class MaterialShadow : Hierarchical, IBlock, IInline {
        public MaterialShadow([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) { }
        public MaterialShadow(Hierarchical other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {}

        public Task<string> GetPlaneTextAsync(Context context) {
            return Task.FromResult(string.Empty);
        }
    }
}
