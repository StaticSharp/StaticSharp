using CsmlEngine;
using CsmlWeb;
using System;

namespace DemoWebsite.Content.Index {



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

    /*partial record En(DemoWebsite.Content.CsmlRoot.αIndex Node): INodeProvider {
        Node INodeProvider.Node => Node;
    }*/

}