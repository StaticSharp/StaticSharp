
using StaticSharpWeb.Html;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    /*public class Column : ElementContainer, IColumn, IElement {
        public virtual async Task<Tag> GenerateHtmlAsync(Context context) {
            context.Parents = context.Parents.Append(this);
            return new Tag("div") {
                new JSCall(AbsolutePath("Column.js"),null,"Before").Generate(context),
                await Task.WhenAll(Items.Select(x=>x.GenerateHtmlAsync(context))),
                new JSCall(AbsolutePath("Column.js"),null,"After").Generate(context),
            };
        }

    }*/


    public class Row : ElementContainer {
        public override IEnumerable<Task<Tag>> Before(Context context) {
            foreach (var i in base.Before(context)) yield return i;
            yield return Task.FromResult(
                new JSCall(AbsolutePath("Row.js"), null, "Before").Generate(context)
                );
        }

        public override IEnumerable<Task<Tag>> After(Context context) {
            foreach (var i in base.After(context)) yield return i;
            yield return Task.FromResult(
                new JSCall(AbsolutePath("Row.js"), null, "After").Generate(context)
                );
        }
    }



}
