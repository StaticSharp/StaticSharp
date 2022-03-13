
using StaticSharpWeb.Html;
using System.Threading.Tasks;

namespace StaticSharpWeb;

public class Button : /*IEnumerable,*/ IElement, IContainerConstraintsNone {

    public Paragraph Paragraph { get; }

    public Button(Paragraph paragraph) {
        Paragraph = paragraph;
    }

    

    public async Task<INode> GenerateHtmlAsync(Context context) {

        context.Includes.Require(new Script(AbsolutePath("Button.js")));

        return new Tag("div") {
            new JSCall(AbsolutePath("Button.js")).Generate(context),
            await Paragraph.GenerateHtmlAsync(context)
            //new TextNode("Hello"),
            
            
        };
    }
}
