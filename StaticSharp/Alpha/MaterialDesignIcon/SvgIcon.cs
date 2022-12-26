using StaticSharp.Gears;
using System;
using System.Linq;
using System.Xml.Linq;

namespace StaticSharp {

    namespace Js {
        public class SvgIcon {
            public Color StrokeColor => NotEvaluatableValue<Color>();
            public double StrokeWidth => NotEvaluatableValue<double>();

        }
    }
    namespace Gears {
        public class SvgIconBindings<FinalJs> : Bindings<FinalJs> where FinalJs : new() {
            public Binding<Color> StrokeColor { set { Apply(value); } }
            public Binding<double> StrokeWidth { set { Apply(value); } }
        }
    }


    /*namespace Gears {
        public static class MaterialDesignIcon {
            
        }
    }*/


    





}