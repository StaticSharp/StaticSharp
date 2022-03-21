using StaticSharpWeb.Html;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class Footer : Column {
        public override async Task<Tag> GenerateHtmlAsync(Context context) {
            return (await base.GenerateHtmlAsync(context)).Modify(x => {
                x.Name = "footer";
                x.AttributesNotNull["style"] = new {
                    minHeight = "100px",
                    backgroundColor = Color.LightGray,
                };
            });
        }
    }
}