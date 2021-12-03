using StaticSharpEngine;
using StaticSharpWeb;
using System;

namespace StaticSharpDemo.Content.Index {



    [Representative]
    partial class En : Common {
        public override string Title => base.Title + "_EN";

        public override MaterialContent Content => new() {
            new Paragraph() {
                "Read this article in russian ",
                Node.WithLanguage(Language.Ru),
                //this,
            }


            //Node.Articles.Terms
        };
    }


}