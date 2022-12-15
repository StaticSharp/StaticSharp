using StaticSharp.Gears;
using System;
using System.Linq;
using System.Xml.Linq;

namespace StaticSharp {

    namespace Js {
        public class MaterialDesignIcon {
            public Color StrokeColor => NotEvaluatableValue<Color>();
            public double StrokeWidth => NotEvaluatableValue<double>();

        }
    }
    namespace Gears {
        public class MaterialDesignIconBindings<FinalJs> : Bindings<FinalJs> where FinalJs : new() {
            public Binding<Color> StrokeColor { set { Apply(value); } }
            public Binding<double> StrokeWidth { set { Apply(value); } }
        }
    }


    namespace Gears {
        public static class MaterialDesignIcon {
            public static string GetSvgTag(MaterialDesignIcons.IconName iconName, out float width, out float height) {
                var code = MaterialDesignIcons.Icon.GetSvg(iconName);//(await new HttpRequestGenome(MaterialDesignIconsStatic.GetSvgUri(Icon)).CreateOrGetCached()).ReadAllText();

                var document = XDocument.Parse(code);
                var svgElement = document.Elements().FirstOrDefault(x => x.Name.LocalName == "svg");

                if (svgElement == null)
                    throw new Exception("Invalid svg file");

                svgElement.Attribute("id")?.Remove();

                try {
                    var viewBox = svgElement.Attribute("viewBox").Value.Split(' ').Select(x => float.Parse(x)).ToArray();
                    width = viewBox[2];
                    height = viewBox[3];
                }
                catch {
                    throw new Exception("Invalid svg file");
                }
                return svgElement.ToString();
            }
        }
    }


    





}