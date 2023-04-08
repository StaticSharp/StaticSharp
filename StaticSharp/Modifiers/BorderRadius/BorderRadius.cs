
using Scopes;
using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    namespace Js {
        public interface BorderRadius {
            public double Radius { get; }
            public double RadiusTopLeft { get; }
            public double RadiusTopRight { get; }
            public double RadiusBottomLeft { get; }
            public double RadiusBottomRight { get; }
        }
    }
    namespace Gears {
        public class BorderRadiusBindings<FinalJs> : ModifierBindings<FinalJs> {
            public Binding<double> Radius { set { Apply(value); } }
            public Binding<double> RadiusTopLeft { set { Apply(value); } }
            public Binding<double> RadiusTopRight { set { Apply(value); } }
            public Binding<double> RadiusBottomLeft { set { Apply(value); } }
            public Binding<double> RadiusBottomRight { set { Apply(value); } }
        }
    }



    [ConstructorJs]
    [Mix(typeof(BorderRadiusBindings<Js.BorderRadius>))]
    public partial class BorderRadius : Modifier {
        public BorderRadius([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
        }

        public BorderRadius(Gears.Object other, int callerLineNumber = 0, string callerFilePath = "") : base(other, callerLineNumber, callerFilePath) {
        }

        /*public override IdAndScript Generate(Context context) {

            return new IdAndScript(
                context.CreateId(),
                new Group {
                    "//Modifier"
                }
                );

        }*/

    }


}


