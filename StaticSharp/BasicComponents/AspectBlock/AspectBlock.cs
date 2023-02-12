using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public enum Fit { 
        Inside,
        Outside,
        Stretch
    }

    namespace Js {
        public interface AspectBlock : Block {
            public double Aspect  { get; }
            public Fit Fit { get; }
            public double GravityVertical  { get; }
            public double GravityHorizontal  { get; }
        }
    }


    namespace Gears {
        public class AspectBlockBindings<FinalJs> : BlockBindings<FinalJs> {
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