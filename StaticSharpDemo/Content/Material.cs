using StaticSharpWeb;
using StaticSharpWeb.Components;
using StaticSharpWeb.Html;
using System.Collections.Generic;
using System.Drawing;

namespace StaticSharpDemo.Content {
    partial class Material : StaticSharpWeb.Material {
        
        public override Footer Footer => new() {
            new Grid(1200, 2) {
                new Paragraph{
                    "AAAAAA",
                    Node.Root.Index, 
                },
                "BBBB",
                Node.Root.Index.Name,
                Node.Root.Index,
                Node.Root.Index,
                new Paragraph{
                    "СССС",
                    Node.Root.Index,
                },
            }
        };

        public override RightSideBar RightSideBar => new() {
            new LanguageMenu<Language>(VirtualNode),
            new LanguageMenu<Language>(VirtualNode)
        };

        public override LeftSideBar LeftSideBar => new() {
            new NavigationMenu(new Logo(Color.FromArgb(0xacc435), Color.White, Node.Root.Index)) {
                Node.Root.Index,
                Node.Root.Index.Katya
            }
        };
    }

}
