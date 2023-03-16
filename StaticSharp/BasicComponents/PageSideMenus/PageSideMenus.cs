using StaticSharp.Gears;
using StaticSharp.Html;
using System.Drawing;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public interface PageSideMenus : Page {
            public double ContentWidth  { get; }
            public bool BarsCollapsed { get; }
            public double SideBarsIconsSize  { get; }

        }
    }

    namespace Gears {
        public class PageSideMenusBindings<FinalJs> : PageBindings<FinalJs> {
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



        //public override Inlines Description => (base.Description != null) ? new Paragraph(base.Description) : null;

        public virtual Block? TopBar => new Paragraph(Title) {
            Height = new(e=>Js.Math.Max(((Js.PageSideMenus)e.Root).SideBarsIconsSize, e.Layer.Height /*InternalHeight*/)),
            //["Height"] = "() => Min(element.Root.SideBarsIconsSize,element.InternalHeight)",
            TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
            MarginsVertical = 0,
            Weight = FontWeight.ExtraLight,
            FontSize = new(e => ((Js.PageSideMenus)e.Root).SideBarsIconsSize),
        };

        public override Block? MainVisual => null;
        public virtual Blocks? Content => null;
        public virtual Block? Footer => null;
        public virtual Block? LeftSideBarIcon => null;
        public virtual Block? LeftSideBar => null;
        public virtual Block? RightSideBarIcon => null;
        public virtual Block? RightSideBar => null;


        public override Blocks Children => new Blocks();

        /*protected override Blocks  => new Blocks {
            {"LeftSideBarIcon"  ,LeftSideBarIcon},
            {"LeftSideBar" ,LeftSideBar},

            {"RightSideBarIcon" ,RightSideBarIcon},
            {"RightSideBar" ,RightSideBar},
            {"Content", new ScrollLayout {
                //FontSize = new(e=>Js.Storage.Store("scroll",()=>e.FontSize)),
                Content = new Column {
                    Children = {
                        { "TopBar", TopBar },
                        MainVisual ,
                        (Description != null) ? new Paragraph(Description) : null ,
                        new Block(){ 
                            Height = 1,
                            BackgroundColor = Color.Gray,
                            MarginBottom = 20
                        },
                        Content,
                        new Space(),
                        Footer
                    }
                }
            }
            }
        }*/

    }

}