using StaticSharpWeb.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class SideBar : List<ISideBarComponent> {
        public virtual async Task<Tag> GenerateSideBarAsync(Context context) {
            var result = new Tag("div");
            var relativePath = new AbsolutePath("SideBar.js");
            context.Includes.Require(new Script(relativePath));
            result.Add(new JSCall(relativePath).Generate(context));
            return result;
        }
    }

    public class RightSideBar : SideBar {
        public override async Task<Tag> GenerateSideBarAsync(Context context) {
            var rightSideBarElement = new Tag("div", new { Class = "rightBar", id = "rightBar" });
            var glass = new Tag("div", new { Class = "Glass", id = "Glass" });
            rightSideBarElement.Add(glass);
            context.Includes.Require(new Style(new AbsolutePath("SideBar.scss")));
            ForEach(async x => rightSideBarElement.Add(await x.GenerateSideBarAsync(context)));
            return rightSideBarElement;
        }
    }

    public class LeftSideBar : SideBar {
        public override async Task<Tag> GenerateSideBarAsync(Context context) {
            var leftSideBarElement = new Tag("div", new { Class = "leftBar", id = "leftBar" });
            var glass = new Tag("div", new { Class = "Glass", id = "Glass" });
            leftSideBarElement.Add(glass);
            context.Includes.Require(new Style(new AbsolutePath("SideBar.scss")));
            ForEach(async x => leftSideBarElement.Add(await x.GenerateSideBarAsync(context)));
            
            return leftSideBarElement;
        }
    }

}