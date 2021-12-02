using CsmlWeb.Html;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace CsmlWeb {

    public interface ISideBarComponent {
        Task<Tag> GenerateSideBarAsync(Context context);
    }



    public class LanguageMenu<TLanguage> : ISideBarComponent where TLanguage : struct, Enum {
        dynamic Node;
        public LanguageMenu(dynamic node) => Node = node;

        public async Task<Tag> GenerateSideBarAsync(Context context) {
            var relativePath = new RelativePath("LanguageMenu.js");
            context.Includes.Require(new Script(relativePath));
            var result = new Tag("menu");
            foreach (var i in Enum.GetValues<TLanguage>()) {
                var uri = context.Urls.ObjectToUri(Node.WithLanguage(i).Representative);
                result.Add(new Tag("a", new { href = uri }) {
                    i.ToString()
                });
            }
            result.Add(new JSCall(relativePath).Generate(context));
            return result;
        }
    }
}