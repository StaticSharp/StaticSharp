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
            var relativePath = AbsolutePath("LanguageMenu.js");
            context.Includes.Require(new Script(relativePath));

            var slider = new Tag("div", new { Class = "RightSlider", id = "RightSlider"});
            var marker = new Tag("div", new { Class = "RightMarker", id = "RightMarker"});
            var icon = new Tag("span", new { Class = "RightIcon", id = "RightIcon"});
            marker.Add(icon);
            var menuList = new Tag("ul", new { Class = "menu-list" });
            var result = new Tag("aside", new { Class = "rightMenu", id = "rightMenu" }) {
                menuList
            };
            //var result = new Tag("aside", new { Class = "rightMenu", id = "rightMenu" });
            slider.Add(result);
            slider.Add(marker);
            var translateIcon = new Tag("a", new { Class = "translateIcon", id = "translateIcon",
            style = "content: url(https://api.iconify.design/ic/baseline-translate.svg?color=%235883cc&width=24&height=24)"});
            menuList.Add( translateIcon );
            foreach (var i in Enum.GetValues<TLanguage>()) {
                var uri = context.Urls.ProtoNodeToUri(Node.WithLanguage(i));
                menuList.Add(new Tag("li") { 
                    new Tag("a", new { href = uri }) {
                    i.ToString()
                    }
                });
            }

            slider.Add(new JSCall(relativePath).Generate(context));
            context.Includes.Require(new Style(AbsolutePath("LanguageMenu.scss")));
            return slider;
        }
    }
}