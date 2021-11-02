using CsmlEngine;
using CsmlWeb;
using CsmlWeb.Components;
using CsmlWeb.Html;
using CsmlWeb.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoWebsite.Content.Index {


    /*public static class MenuStatic {
        public static void Add<T>(this T collection, Menu item) where T : IVerifiedBlockReceiver, ITextAnchorsProvider {
            collection.AddBlock(item);
        }
    }

    public class Menu<TNode> : IBlock {
        TNode Node;

        public Menu(TNode node) {
        }

        public async Task<CsmlWeb.Html.INode> GenerateBlockHtmlAsync(Context context) {
            return new Tag("menu") {

            };
        }

    }*/

    [Representative]
    partial class Ru : Common {
        public override string Title => null;
        public override IImage TitleImage => new Video("qj6S37xIqK0").ConfigureAsBackgroundVideo();
        public override Paragraph Description => new() { "Ссылка на эту статью: ", Node, "ТЕСТ ТЕСТ ТЕСТ" };
        private Image image = new Image(new RelativePath("111.jpg"), "Ricardooo");

        //private Image refImage = new Image("https://bipbap.ru/wp-content/uploads/2017/04/0_7c779_5df17311_orig.jpg");

        //private Language wiki => new Language();
        //private Reference wiki_ru => new Reference("ТЕКСТ3: ", "https://ru.wikipedia.org/wiki/C_Sharp", "C#");    

        public override MaterialContent Content
        {
            get
            {
                return new(){

                new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", "Ссылки с текстом: "),
                new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", "Ссылки с текстом и подсказкой: ", "Шарп"),
                new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", image),
                //new Reference("ТЕКСТ3: ", "https://ru.wikipedia.org/wiki/C_Sharp", refImage),

                new Grid(ContentWidth / 4) {
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                    //new MaterialCard(Node.Representative),
                },

                new Paragraph() {
                "AAAAAAA"
                },

                new UnorderedList(new string[1] {"a"}),
                //new Panel("Default Panel"),
                new Info("Ситуации 7 и 9 - это не рабочий вариант. Работа сети в такой конфигурации будет крайне плоха из-за большого"),
                new Error("Error Panel"),
                new Bug("Bug Panel"),
                new Note("Note Panel"),
                new Success("Success Panel"),
                new Warning("Warning Panel"),

                // new Reference("ТЕКСТ1: ", "https://ru.wikipedia.org/wiki/C_Sharp"),
                // new Reference("ТЕКСТ2: ", "https://ru.wikipedia.org/wiki/C_Sharp", "Шарп"),
                // new Reference("ТЕКСТ3: ", "https://ru.wikipedia.org/wiki/C_Sharp", refImage),

                // new Grid(ContentWidth / 6) {
                //     new MaterialCard(Node.Representative),
                //     new MaterialCard(Node.Representative),
                //     new MaterialCard(Node.Representative),
                // },
                
                //new Video("S_fyy8X8C1g"),
                //new Video("S_fyy8X8C1g", image),
                
                // new Paragraph() {
                //     Node.Articles.Terms,

                //     Node.WithLanguage(Language.En).Representative,
                //     Node.WithLanguage(Language.Ru).Representative,
                // },
                // new Paragraph() {
                //     Node.Articles.Terms,

                //     Node.WithLanguage(Language.En).Representative,
                //     Node.WithLanguage(Language.Ru).Representative,
                // },

                //"Ссылка на эту статью: ", Node,
                //Code,

                //,Node.Children.Select(x=>x.Representative).OfType<Material>()
                };
            }
        }
    }
}