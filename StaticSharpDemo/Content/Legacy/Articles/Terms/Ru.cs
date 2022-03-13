
using StaticSharpEngine;
using StaticSharpWeb;
using System;

namespace StaticSharpDemo.Root.Legacy.Articles.Terms {

    [Representative]
    partial class Ru : Common {
        public override string Title => "Термины";
        //public override string Title => base.Title+"_RU";
        public override Column Content => new () {

            new Paragraph() { 
                "Тут приведен список всех внутренних терминов Antilatency"
            }

        };
    }
}