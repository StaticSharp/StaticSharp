using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Js {
        public interface MenuResponsive : Block {
            public double PrimaryGravity { get; }
            public double SecondaryGravity { get; }
            public bool HideButton { get; }
            public bool DropdownExpanded { get; }
            

            public Enumerable<Block> MenuItems { get; }
            public Block? Logo { get; }
            public Block Button { get; }
            public Block Dropdown { get; }
        }
    }


    namespace Gears {
        public class MenuResponsiveBindings<FinalJs> : BlockBindings<FinalJs>  {
            public Binding<double> PrimaryGravity { set { Apply(value); } }

            public Binding<double> SecondaryGravity { set { Apply(value); } }

            public Binding<bool> DropdownExpanded { set { Apply(value); } }

            public Binding<bool> HideButton { set { Apply(value); } }
        }
    }

    [Scripts.LayoutUtils]
    [Mix(typeof(MenuResponsiveBindings<Js.MenuResponsive>))]
    [ConstructorJs]
    public partial class MenuResponsive : Block {

        protected static Color DefaultBackgroundColor => Color.FromGrayscale(0.9);


        public Block? Logo { get; set; } = null;

        public Block Button { get; set; } = new SvgIconBlock(SvgIcons.MaterialDesignIcons.Menu)
        {
            Visibility = new(e => ((Js.MenuResponsive)e.Parent).Dropdown.Children.Any(null) || !((Js.MenuResponsive)e.Parent).HideButton ? 1 : 0),
            BackgroundColor = new(e => ((Js.MenuResponsive)e.Parent).DropdownExpanded ? DefaultBackgroundColor : Color.White),
            // TODO: Cursor = new(e => ((Js.MenuResponsive)e.Parent).Dropdown.Children.Any(null) ? Cursor.Pointer : Cursor.Default),
        };

        public Block Dropdown { get; set; } = new LinearLayout() {
            Vertical = true,
            BackgroundColor = DefaultBackgroundColor,
            Paddings = 5,
            RadiusTopLeft= 5,
            RadiusBottomLeft= 5,
            RadiusBottomRight = 5,
            Visibility = new(e => ((Js.MenuResponsive)e.Parent).DropdownExpanded ? 1 : 0),
        };

        public virtual Blocks MenuItems { get; } = new();

        public MenuResponsive([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag) {

            var logo = Logo;
            if (logo != null) {
                elementTag.Add(CreateScript_SetCurrentSocket("Logo"));
                elementTag.Add(logo.GenerateHtml(context));
            }


            if (MenuItems != null)
            {
                var menuItems = MenuItems.ToArray();
                elementTag.Add(CreateScript_SetCurrentCollectionSocket("MenuItems"));
                foreach(var menuItem in menuItems)
                {
                    elementTag.Add(menuItem.GenerateHtml(context));
                }
            }

            elementTag.Add(CreateScript_SetCurrentSocket("Button"));
            elementTag.Add(Button.GenerateHtml(context));

            elementTag.Add(CreateScript_SetCurrentSocket("Dropdown"));
            elementTag.Add(Dropdown.GenerateHtml(context));

            



            base.ModifyHtml(context, elementTag);
        }
    }
}
