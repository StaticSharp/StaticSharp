
using CsmlEngine;
using CsmlWeb;
using System;

namespace DemoWebsite.Content.Index.Articles.Terms {

    [Representative]
    partial class Fr : Common {
        public override string Title => "Термины";
        //public override string Title => base.Title+"_RU";
        public override MaterialContent Content => new () {

            new Paragraph() { 
                "Тут приведен список всех внутренних терминов Antilatency"
            }

        };
    }
}