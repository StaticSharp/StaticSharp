using StaticSharp.Gears;
using StaticSharp.Html;
using System.Drawing;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public class PageSideMenus : Page {
            public double ContentWidth => NotEvaluatableValue<double>();
            public bool BarsCollapsed => NotEvaluatableValue<bool>();
            public double SideBarsIconsSize => NotEvaluatableValue<double>();

        }
    }

    namespace Gears {
        public class PageSideMenusBindings<FinalJs> : PageBindings<FinalJs> where FinalJs : new() {
            public Binding<double> ContentWidth { set { Apply(value); } }
            public Binding<double> SideBarsIconsSize { set { Apply(value); } }
        }
    }

    [ConstructorJs]
    [Mix(typeof(PageSideMenusBindings<Js.PageSideMenus>))]
    public abstract partial class PageSideMenus : Page {

        
        protected override void Setup(Context context) {
            SideBarsIconsSize = 48;
            base.Setup(context);
        }
        public override string Title {
            get {
                var n = GetType().Namespace;
                return n[(n.LastIndexOf('.') + 1)..].Replace('_',' ');
            }
        }
        public virtual Block? Description => (DescriptionContent != null) ? new Paragraph(DescriptionContent) : null;

        public virtual Block? TopBar => new Paragraph(Title) {
            Height = new(e=>Js.Math.Max(e.Root.As<Js.PageSideMenus>().SideBarsIconsSize, e.InternalHeight)),
            //["Height"] = "() => Min(element.Root.SideBarsIconsSize,element.InternalHeight)",
            TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
            MarginsVertical = 0,
            Weight = FontWeight.ExtraLight,
            FontSize = new(e => e.Root.As<Js.PageSideMenus>().SideBarsIconsSize),
        };

        public override Block? MainVisual => null;
        public virtual Blocks? Content => null;
        public virtual Block? Footer => null;
        public virtual Block? LeftSideBarIcon => null;
        public virtual Block? LeftSideBar => null;
        public virtual Block? RightSideBarIcon => null;
        public virtual Block? RightSideBar => null;


        protected override Blocks BodyContent => new Blocks {
            {"LeftSideBarIcon"  ,LeftSideBarIcon},
            {"LeftSideBar" ,LeftSideBar},

            {"RightSideBarIcon" ,RightSideBarIcon},
            {"RightSideBar" ,RightSideBar},
            {"Content", new ScrollLayout {
                //FontSize = new(e=>Js.Storage.Store("scroll",()=>e.FontSize)),
                Content = new Column {
                    Children = {
                        { "TopBar", TopBar },
                        { "MainVisual", MainVisual },
                        { "Description", Description },
                        new Block(){ 
                            Height = 1,
                            BackgroundColor = Color.Gray,
                            MarginBottom = 20
                        },
                        Content,
                        new Space(),
                        { "Footer", Footer }
                    }
                }
            }
            }
        };       


    }

}