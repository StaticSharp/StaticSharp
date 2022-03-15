using StaticSharpWeb.Html;
using System;
using System.Threading.Tasks;

namespace StaticSharpWeb;

public abstract class AbstractLink { 

}



public class OverlayLink : IContainerConstraintsNone {

    private Func<Context, Uri> UriCalculator;
    public OverlayLink(StaticSharpEngine.IRepresentative representative) {
        UriCalculator = (context) => UriFromNode(representative?.Node, context);
    }

    public OverlayLink(StaticSharpEngine.ITypedRepresentativeProvider<IPage> node) {
        UriCalculator = (context) => UriFromNode(node, context);
    }

    private static Uri UriFromNode(StaticSharpEngine.INode? node, Context context) {
        var uri = context.NodeToUrl(node);
        if (uri == null) {
            throw new NullReferenceException();//todo: special exception
        }
        return uri;
    }



    public async Task<Tag> GenerateHtmlAsync(Context context) {
        return new Tag("a", new {
            style = new { 
                position = "absolute",
                left = 0,
                top = 0,
                width = "100%",
                height = "100%"
            },
            href = UriCalculator(context).ToString(),

        });
    }
}
