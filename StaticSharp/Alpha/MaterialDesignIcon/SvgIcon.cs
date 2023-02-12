using StaticSharp.Gears;
using System;
using System.Linq;
using System.Xml.Linq;

namespace StaticSharp {

    namespace Js {
        public interface SvgIcon {
            public Color StrokeColor { get; }
            public double StrokeWidth  { get; }

        }
    }
    namespace Gears {
        public class SvgIconBindings<FinalJs> : Bindings<FinalJs> {
            public Binding<Color> StrokeColor { set { Apply(value); } }
            public Binding<double> StrokeWidth { set { Apply(value); } }
        }
    }


    /*namespace Gears {
        public static class MaterialDesignIcon {
            
        }
    }*/


    





}