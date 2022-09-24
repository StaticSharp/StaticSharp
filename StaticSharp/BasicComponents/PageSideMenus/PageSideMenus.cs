using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public class PageSideMenusJs : PageJs {
        public float ContentWidth => NotEvaluatableValue<float>();
        public bool BarsCollapsed => NotEvaluatableValue<bool>();       

    }

    namespace Gears {
        public class PageSideMenusBindings<FinalJs> : PageBindings<FinalJs> where FinalJs : new() {
            public Binding<float> ContentWidth { set { Apply(value); } }

        }
    }

    [ConstructorJs]
    [Mix(typeof(PageSideMenusBindings<PageJs>))]
    public partial class PageSideMenus : Page {

        public override string Title {
            get {
                var n = GetType().Namespace;
                return n[(n.LastIndexOf('.') + 1)..];
            }
        }

        public virtual Block? TopBar => new Row{
            Children = {
                new Space(),
                new Paragraph(Title+"\nLower text") {
                    FontSize = 48,
                    LineHeight = 1,
                    ["Height"] = "()=>Max(element.InternalHeight,64)"
                },
                new Space(),
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
                        TopBar,
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