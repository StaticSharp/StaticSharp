using StaticSharpEngine;
using System;
using System.Linq;

namespace StaticSharpDemo.Content.Index {

    public partial class Common : Material {
        public override int ContentWidth => 1200;

        
        public string Titles {
            get {
                var options = Enum.GetValues<Language>().Select(x => VirtualNode.WithLanguage(x).Representative as Common);

                return string.Join(" - ", options.Select(x => x.Title));
            }
        }
    }
}