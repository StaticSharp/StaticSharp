using StaticSharpGears;
using StaticSharpWeb.Html;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb;


public class BillboardSolidColor: /*IEnumerable,*/ IElement, IContainerConstraints<IColumn> {

    public object? Style { get; set; } = null;
    public Color Color { get; set; }
    public string MinHeight { get; set; } = "30vh";
    public string FontSize { get; set; } = "1.5rem";

    public float MaxContentWidth { get; set; } = 640;
    public Column Content { get; init; } = new();

    public async Task<INode> GenerateHtmlAsync(Context context) {

        return new Tag("div", new {
            style = SoftObject.MergeObjects(
                new {
                    TextAlign = "center",
                    FontSize = FontSize,
                    BackgroundColor = ColorTranslator.ToHtml(Color),
                    MinHeight = MinHeight,
                    Padding = $"{context.Theme.BaseSize*4} 0"
                },
                Style)
            })
        {
            new JSCall(AbsolutePath("BillboardSolidColor.js"),MaxContentWidth ).Generate(context),
            await Task.WhenAll(Content.Items.Select(x => x.GenerateHtmlAsync(context)))
        };
    }

    //public 
}