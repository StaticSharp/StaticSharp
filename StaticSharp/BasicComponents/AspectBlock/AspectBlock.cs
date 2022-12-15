using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public enum Fit { 
        Inside,
        Outside,
        Stretch
    }

    namespace Js {
        public class AspectBlock : Block {
            public double Aspect => NotEvaluatableValue<double>();
            public Fit Fit => NotEvaluatableValue<Fit>();
            public double GravityVertical => NotEvaluatableValue<double>();
            public double GravityHorizontal => NotEvaluatableValue<double>();
        }
    }


    namespace Gears {
        public class AspectBlockBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
            public Binding<double> Aspect { set { Apply(value); } }
            public Binding<Fit> Fit { set { Apply(value); } }
            public Binding<double> GravityVertical { set { Apply(value); } }
            public Binding<double> GravityHorizontal { set { Apply(value); } }
        }
    }


    [Mix(typeof(AspectBlockBindings<Js.AspectBlock>))]
    [ConstructorJs]
    public partial class AspectBlock : Block {
        public AspectBlock(AspectBlock other, int callerLineNumber, string callerFilePath): base(other, callerLineNumber, callerFilePath) {}
        public AspectBlock([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {}

    }

}