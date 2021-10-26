using CsmlEngine;
using System;
using System.Linq;

namespace DemoWebsite.Content.Index {

    partial class Common : Material {

        public override int ContentWidth => 1200;
        public string Titles {
            get {
                
                //dynamic node = (this as IRepresentative).Node;
                //Func<Language,dynamic> changeLanguage = (x) => node.WithLanguage(x).Representative;

                var options = Enum.GetValues<Language>().Select(x => VirtualNode.WithLanguage(x).Representative as Common);

                return string.Join(" - ", options.Select(x=>x.Title));
            }
        
        }
    }

}

