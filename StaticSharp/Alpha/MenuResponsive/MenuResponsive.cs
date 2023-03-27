using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public interface MenuResponsive : Block {
            public double? SecondaryGravity { get; }
        }
    }


    namespace Gears {
        public class MenuResponsiveBindings<FinalJs> : BlockBindings<FinalJs>  {
            public Binding<double?> SecondaryGravity { set { Apply(value); } }
        }
    }

    [RelatedScript("../../BasicComponents/LinearLayout/LinearLayout")]
    [Mix(typeof(MenuResponsiveBindings<Js.MenuResponsive>))]
    [ConstructorJs]
    public partial class MenuResponsive : Block, IBlock {
        public Block? Logo { get; set; } = null;

        public Block Button { get; set; } = new SvgIconBlock(SvgIcons.MaterialDesignIcons.Menu);

        public Block Dropdown { get; set; } = new LinearLayout() {
            Vertical = true,
            BackgroundColor = Color.FromGrayscale(0.9),
            Paddings = 5,
            Radius = 5
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
