using StaticSharp.Gears;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root.Components {

    [Representative]
    public partial class Ru : Material {

        protected override Task Setup(Context context) {
            BackgroundColor = ColorTranslator.FromHtml("#f8edeb");
            return base.Setup(context);
        }

        public override Inlines Description => $"Компоненты для создания страниц.";

        public override Blocks Content => new(){

            new Row{
                BackgroundColor = ColorTranslator.FromHtml("#b56576"),
                Children = { 
                    new Paragraph($"Paragraph"){ 
                        Overlay = new LinkBlock(Node.Parent){
                            BackgroundColor = ColorTranslator.FromHtml("#80000000"),
                            Margins = new (e=>e.Root.Touch? -20 : 0)
                        }                       
                    },
                    $"{new LinkInline{HRef = "https://developers.antilatency.com/" , Children = { "First" }}}",
                    "Second",
                    new Space(),
                    "Last"
                }
            }
            .Modify(x=>{
                var items = x.Children.Values.OfType<Block>().ToArray();
                foreach (var i in items) {
                    i.Selectable = false;
                    i.Margins = 0;
                    i.Paddings = 20;
                    i.BackgroundColor = new(e=>e.Hover? ColorTranslator.FromHtml("#e56b6f") : ColorTranslator.FromHtml("#00000000"));
                }
                items[items.Length-1].BackgroundColor = new(e=>e.Hover? ColorTranslator.FromHtml("#ffb703") : ColorTranslator.FromHtml("#00000000"));
            })
            .FillWidth().InheritPaddings(),

            


            new Flipper{
                First = new Video("-LF5M9nlFQs"),
                Second = new Column{
                    Children = {
                        new Space(),
                        H3("Video"),
                        $"""
                        Компонент Video может отображаться как iframe или как video tag, в зависимости от настроек
                        Подробнее {Node.VideoPlayer}
                        """,
                        new Space()
                    }
                }
            },
        };

    }
}

