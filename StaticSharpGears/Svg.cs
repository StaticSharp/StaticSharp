using System.Drawing;
using System.Xml.Linq;

namespace StaticSharpGears;

public class Svg {

    private XDocument document;
    private XElement? svgElement;

    public Svg(string code) {
        document = XDocument.Parse(code);
        svgElement = document.Elements().FirstOrDefault(x => x.Name.LocalName == "svg");
    }

    public Uri DataUri => new Uri("data:image/svg+xml,"+Uri.EscapeDataString(svgElement.ToString()));

    public Color FillColor {
        set {
            svgElement?.SetAttributeValue("fill", ColorTranslator.ToHtml(value));
        }
    }



}



