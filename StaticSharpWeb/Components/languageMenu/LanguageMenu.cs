using StaticSharpWeb.Html;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface ISideBarComponent {
        Task<Tag> GenerateSideBarAsync(Context context);
    }



    public class LanguageMenu<TLanguage> : ISideBarComponent where TLanguage : struct, Enum {
        dynamic Node;
        public LanguageMenu(dynamic node) => Node = node;

        public async Task<Tag> GenerateSideBarAsync(Context context) {
            var relativePath = new RelativePath("LanguageMenu.js");
            context.Includes.Require(new Script(relativePath));

            var slider = new Tag("div", new { Class = "RightSlider", id = "RightSlider"});
            var marker = new Tag("div", new { Class = "RightMarker", id = "RightMarker"});
            var icon = new Tag("span", new { Class = "RightIcon", id = "RightIcon"});
            marker.Add(icon);
            var menuList = new Tag("ul", new { Class = "menu-list" });
            var result = new Tag("aside", new { Class = "leftMenu", id = "leftMenu" }) {
                menuList
            };
            //var result = new Tag("aside", new { Class = "rightMenu", id = "rightMenu" });
            slider.Add(result);
            slider.Add(marker);
            foreach (var i in Enum.GetValues<TLanguage>()) {
                var uri = context.Urls.ObjectToUri(Node.WithLanguage(i).Representative);
                menuList.Add(new Tag("li") { 
                    new Tag("a", new { href = uri }) {
                    i.ToString()
                    }
                });
            }

            slider.Add(new JSCall(relativePath).Generate(context));
            context.Includes.Require(new Style(new RelativePath("LanguageMenu.scss")));
            return slider;
        }
    }
}