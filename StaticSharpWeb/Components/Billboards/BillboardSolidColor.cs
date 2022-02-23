using StaticSharpGears;
using StaticSharpWeb.Html;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb;


public class BillboardSolidColor: /*IEnumerable,*/ IBlock, IContainerConstraints<IWideAnchorsProvider,ITextAnchorsProvider,IFillAnchorsProvider> {

    public object? Style { get; set; } = null;
    public Color Color { get; set; }
    public string MinHeight { get; set; } = "30vh";
    public string FontSize { get; set; } = "1.5rem";

    public float MaxContentWidth { get; set; } = 640;
    public TextContainer Content { get; init; } = new();

    public async Task<INode> GenerateBlockHtmlAsync(Context context) {
        /*
         new {
                style = SoftObject.MergeObjects(new {
                    Padding = "24px 0"
                }, Style)
            }
         
         */


        return new Tag("div", new {
            style = SoftObject.MergeObjects(
                new {
                    TextAlign = "center",
                    FontSize = FontSize,
                    BackgroundColor = ColorTranslator.ToHtml(Color),
                    MinHeight = MinHeight,
                    Padding = "64px 0"
                },
                Style)
            })
        {
            new JSCall(AbsolutePath("BillboardSolidColor.js"),MaxContentWidth ).Generate(context),
            await Task.WhenAll(Content.Select(x => x.GenerateBlockHtmlAsync(context)))
        };
    }

    //public 
}