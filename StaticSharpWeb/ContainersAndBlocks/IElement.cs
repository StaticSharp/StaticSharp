using System.Threading.Tasks;

namespace StaticSharpWeb;


public interface IElement {
    //можжет быть возвращать Tag?
    Task<Html.Tag> GenerateHtmlAsync(Context context);
}
/*
public static class IElementStatic {
    public static Task<Html.INode?> GenerateHtmlAsync(this IElement? element, Context context) {
        if (element == null) { 
            return Task.FromResult<Html.INode?>(null);
        } else {
            return element.GenerateHtmlAsync(context);
        }
    }
}*/