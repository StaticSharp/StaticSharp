
using StaticSharp.Tree;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root.Components {




    public abstract partial class ComponentPage : Page {


        public override string Title {
            get {
                var result = GetType().Namespace;
                result = result[(result.LastIndexOf('.') + 1)..];

                var suffix = "Component";
                if (result.EndsWith(suffix)) {
                    result = result.Remove(result.Length - suffix.Length);
                }

                result = StaticSharp.Gears.CaseUtils.CamelCaseRegex.Replace(result, match => {
                    if (match.Index == 0) return match.Value;
                    return " "+match.Value;                   
                    });                

                return result;
            }
        }


        

        public override Block LeftSideBar => new ScrollLayout {
            Content = new Column(){
                BackgroundColor = Color.FromArgb(96,96,96),
                Children = {
                    Node.Children.Select(x=>MenuItem(x))
                }
            }            
        };
    }



    [Representative]
    public partial class Ru : ComponentPage {

        protected override Task Setup(Context context) {
            BackgroundColor = ColorTranslator.FromHtml("#f8edeb");
            return base.Setup(context);
        }

        public override Inlines DescriptionContent => $"Компоненты для создания страниц.";

        public override Blocks Content => new(){

            new Row{
                Depth = 1,
                BackgroundColor = ColorTranslator.FromHtml("#b56576"),
                Children = { 
                    new Paragraph($"Paragraph"){ 
                        Children = {
                            new LinkBlock(Node.Parent){
                                BackgroundColor = ColorTranslator.FromHtml("#80000000"),
                                Margins = new (e=>Js.Window.Touch? -20 : 0)
                            }.FillWidth().FillHeight()
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
            .FillWidth().InheritHorizontalPaddings(),

            


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

