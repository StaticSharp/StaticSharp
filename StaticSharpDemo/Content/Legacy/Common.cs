using StaticSharpWeb;
using System;
using System.Linq;

namespace StaticSharpDemo.Root.Legacy {

    public partial class Common : Material {
        public override int ContentWidth => 1200;

        public Image Image => new Image("a.png");
        public string Titles {
            get {
                var options = Enum.GetValues<Language>().Select(x => VirtualNode.WithLanguage(x).Representative as Common);

                return string.Join(" - ", options.Select(x => x.Title));
            }
        }
    }
}