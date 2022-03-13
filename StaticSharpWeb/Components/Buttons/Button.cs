
using StaticSharpWeb.Html;
using System.Threading.Tasks;

namespace StaticSharpWeb;

public class Button : /*IEnumerable,*/ IElement, IContainerConstraintsNone {
    public async Task<INode> GenerateHtmlAsync(Context context) {

        context.Includes.Require(new Script(AbsolutePath("Button.js")));

        return new Tag("div") {
            new JSCall(AbsolutePath("Button.js")).Generate(context),
            //new TextNode("Hello"),
            
            
        };
    }
}
