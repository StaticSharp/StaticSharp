using StaticSharpWeb.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class Footer : Column {
        public override async Task<Tag> GenerateHtmlAsync(Context context) {
            return (await base.GenerateHtmlAsync(context)).Modify(x => {
                x.Name = "footer";
            });
        }
    }
}