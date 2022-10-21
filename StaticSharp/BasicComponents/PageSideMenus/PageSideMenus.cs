using StaticSharp.Gears;
using StaticSharp.Html;
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
    public partial class PageSideMenus : Page {

        protected override Task Setup(Context context) {
            SideBarsIconsSize = 48;
            return base.Setup(context);
        }
        public override string Title {
            get {
                var n = GetType().Namespace;
                return n[(n.LastIndexOf('.') + 1)..].Replace('_',' ');
            }
        }

        public virtual Block? TopBar => new Row{
            //Height = new(Js.Math.Min())
            ["Height"] = "() => Min(element.Root.SideBarsIconsSize,element.InternalHeight)",
            Children = {
                new Space(before: 1),
                new Paragraph(Title) {
                    MarginsVertical = 0,
                    FontSize = new (e=>e.Root.As<Js.PageSideMenus>().SideBarsIconsSize),
                },
                new Space(after: 1),
            }
        };

        public virtual Inlines? Description => null;
        public virtual Blocks? Content => null;


        public virtual Block? Footer => null;

        

        public virtual Block? LeftSideBarIcon => null;
        public virtual Block? LeftSideBar => null;


        public virtual Block? RightSideBarIcon => null;
        public virtual Block? RightSideBar => null;
        


        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {




            elementTag.Add(
                await new Blocks {
                    {"LeftSideBarIcon" ,LeftSideBarIcon},
                    {"LeftSideBar" ,LeftSideBar},

                    {"RightSideBarIcon" ,RightSideBarIcon},
                    {"RightSideBar" ,RightSideBar},
                    {"Content", new ScrollLayout {
                        Content = new Column {
                            Children = {
                                { "TopBar", TopBar },
                                Content,
                                new Space(),
                                Footer
                            }
                        }
                    }}
                }.GenerateHtmlAsync(context)
                );



            /*var result = new Tag(null);
            
            if (LeftSideBar != null) {
                result.Add(await LeftSideBar.GenerateHtmlAsync(context, "LeftSideBar"));
            }

            if (RightSideBar != null) {
                result.Add(await RightSideBar.GenerateHtmlAsync(context, "RightSideBar"));
            }

            result.Add(
                await new Column {
                    Children = {
                        Content,
                        new Space(),
                        Footer
                    }

                }.GenerateHtmlAsync(context, "Content")
            );

            return result;*/
        }


    }

}