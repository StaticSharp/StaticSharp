using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public class PageSideMenusJs : PageJs {
        public float ContentWidth => NotEvaluatableValue<float>();
        public bool BarsCollapsed => NotEvaluatableValue<bool>();        
        public float SideBarsIconsSize => NotEvaluatableValue<float>();

    }

    namespace Gears {
        public class PageSideMenusBindings<FinalJs> : PageBindings<FinalJs> where FinalJs : new() {
            public Binding<float> ContentWidth { set { Apply(value); } }
            public Binding<float> SideBarsIconsSize { set { Apply(value); } }
        }
    }

    [ConstructorJs]
    [Mix(typeof(PageSideMenusBindings<PageJs>))]
    public partial class PageSideMenus : Page {

        protected override Task Setup(Context context) {
            SideBarsIconsSize = 48;
            return base.Setup(context);
        }
        public override string Title {
            get {
                var n = GetType().Namespace;
                return n[(n.LastIndexOf('.') + 1)..];
            }
        }

        public virtual Block? TopBar => new Row{
            Children = {
                new Space(before: 1),
                new Paragraph(Title) {
                    FontSize = new (e=>e.Root.As<PageSideMenusJs>().SideBarsIconsSize),
                    LineHeight = 1,
                },
                new Space(after: 1),
            }
        };

        public virtual Inlines? Description => null;
        public virtual Blocks? Content => null;


        public virtual Block? Footer => null;

        
        public virtual Block? RightSideBar => null;
        public virtual Block? LeftSideBar => null;


        protected override async Task<Tag> GenerateChildrenHtmlAsync(Context context, Tag elementTag) {

            return await new Blocks {
                {"LeftSideBar" ,LeftSideBar},
                {"RightSideBar" ,RightSideBar},
                {"Content", new Column {
                    Children = {
                        { "TopBar", TopBar },
                        Content,
                        new Space(),
                        Footer
                    }
                } }
            }.GenerateHtmlAsync(context);



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