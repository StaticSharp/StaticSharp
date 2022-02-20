using StaticSharpWeb.Html;
using System.Drawing;
using System.Threading.Tasks;

namespace StaticSharpWeb;

public class BillboardSolidColor: /*IEnumerable,*/ IBlock, IContainerConstraints<IWideAnchorsProvider,ITextAnchorsProvider,IFillAnchorsProvider> {
    
    public Color Color { get; set; }

    public string MinHeight { get; set; } = "30vh";
    
    public async Task<INode> GenerateBlockHtmlAsync(Context context) {
        return new Tag("div", new {
            billboard = true,
            style = $"background-color: {ColorTranslator.ToHtml(Color)}; min-height: {MinHeight}"
        }) { 
            new JSCall(AbsolutePath("Billboard.js")).Generate(context)
        };
    }

    //public 
}