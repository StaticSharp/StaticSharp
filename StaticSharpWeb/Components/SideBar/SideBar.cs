using StaticSharpWeb.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class SideBar : List<ISideBarComponent> {
        public virtual async Task<Tag> GenerateSideBarAsync(Context context) {
            var result = new Tag("div");
            var relativePath = new RelativePath("SideBar.js");
            context.Includes.Require(new Script(relativePath));
            result.Add(new JSCall(relativePath).Generate(context));
            return result;
        }
    }

    public class RightSideBar : SideBar {
        public override async Task<Tag> GenerateSideBarAsync(Context context) {
            var result = new Tag("div", new {
                id = "rightBar",
                //style = ""
            });
            context.Includes.Require(new Style(new RelativePath("SideBar.scss")));
            ForEach(async x => result.Add(await x.GenerateSideBarAsync(context)));
            return result;
        }
    }

    public class LeftSideBar : SideBar {
        public override async Task<Tag> GenerateSideBarAsync(Context context) {
            var result = new Tag("div", new {
                id = "leftBar",
                //style = "visibility: visible;position: fixed;width: auto;display: flex;flex-direction: column;top: 0px;height: 100vh;background-color: rgb(227, 227, 227)"
            });
            context.Includes.Require(new Style(new RelativePath("SideBar.scss")));
            ForEach(async x => result.Add(await x.GenerateSideBarAsync(context)));
            return result;
        }
    }

}