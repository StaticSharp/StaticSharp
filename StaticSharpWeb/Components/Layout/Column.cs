
using StaticSharpWeb.Html;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IColumn { }
    public class Column : ElementContainer, IColumn, IElement {

        public float? MaxInnerWidth { get; } = null;
        public async Task<Tag?> GenerateHtmlAsync(Context context) {
            context.Parents = context.Parents.Append(this);
            return new Tag(null) {
                new JSCall(AbsolutePath("Column.js"), new {MaxInnerWidth = MaxInnerWidth }).Generate(context),
                await Task.WhenAll(Items.Select(x=>x.GenerateHtmlAsync(context))),
                
            };
        }

        async Task<INode?> IElement.GenerateHtmlAsync(Context context) {
            return await GenerateHtmlAsync(context);
        }
    }
}
