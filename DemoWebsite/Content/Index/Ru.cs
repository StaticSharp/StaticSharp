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

        public override MaterialContent Content
        {
            get
            {
                return new(){
                    new Grid(ContentWidth / 4) {
                    new MaterialCard(Node.Representative),
                    new MaterialCard(Node.Representative),
                    new MaterialCard(Node.Representative),
                    //new MaterialCard(Node.Representative),
                },

                    new Paragraph() {
                    "AAAAAAA"
                },

                new Info(),

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