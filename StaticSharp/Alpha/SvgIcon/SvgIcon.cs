using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharp.Js;
using SvgIcons;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace StaticSharp {

    public interface JSvgIcon {
        public Color StrokeColor { get; set; }
        public double StrokeWidth { get; set; }

    }

}