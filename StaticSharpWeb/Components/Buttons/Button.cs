
using StaticSharpWeb.Html;
using System.Threading.Tasks;

namespace StaticSharpWeb;

public class Button : /*IEnumerable,*/ IElement, IContainerConstraintsNone {
    public async Task<INode> GenerateHtmlAsync(Context context) {
        return new Tag("div", new { Class = "Button" }) {
            new TextNode("Hello")
            new JSCall(AbsolutePath("Button.js")).Generate(context),
            
        };
    }
}
