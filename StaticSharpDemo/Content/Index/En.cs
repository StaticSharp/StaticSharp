using StaticSharpEngine;
using StaticSharpWeb;
using System;

namespace StaticSharpDemo.Content.Index {



    [Representative]
    partial class En : Common {
        public override string Title => base.Title + "_EN";
        private Image image = new Image(new RelativePath("111.jpg"), "Ricardooo");

        public override MaterialContent Content => new() {
            new Paragraph() {
                "Read this article in russian ",
                Node.WithLanguage(Language.Ru),
                //this,
            },

            new Reference("https://ru.wikipedia.org/wiki/C_Sharp"),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", "Ссылки с текстом: "),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", "Ссылки с текстом и подсказкой: ", "Шарп"),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", image)
            //Node.Articles.Terms
        };
    }


}