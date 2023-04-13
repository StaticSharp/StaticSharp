using AngleSharp.Dom;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public interface JMenuResponsive : JBlockWithChildren {
        public double PrimaryGravity { get; }
        public double SecondaryGravity { get; }
        public bool DropdownExpanded { get; }

        public JBlock? Logo { get; }
        public JBlock Button { get; }
        public JBlockWithChildren Dropdown { get; }
    }

    namespace Gears {
        public class MenuResponsiveBindings<FinalJs> : BlockWithChildrenBindings<FinalJs>  {
            public Binding<double> PrimaryGravity { set { Apply(value); } }

            public Binding<double> SecondaryGravity { set { Apply(value); } }

            public Binding<bool> DropdownExpanded { set { Apply(value); } }
        }
    }

    [Scripts.LayoutUtils]
    [Mix(typeof(MenuResponsiveBindings<JMenuResponsive>))]
    [ConstructorJs]
    public partial class MenuResponsive : BlockWithChildren {

        protected static Color DefaultBackgroundColor => Color.FromGrayscale(0.9);

        [Socket]
        public Block? Logo { get; set; } = null;

        [Socket]
        public Block Button { get; set; } = new SvgIconBlock(SvgIcons.MaterialDesignIcons.Menu)
        {
            Visibility = new(e => ((JMenuResponsive)e.Parent).Dropdown.Children.Any() ? 1 : 0),
            BackgroundColor = new(e => ((JMenuResponsive)e.Parent).DropdownExpanded ? DefaultBackgroundColor : Color.White),
        };

        [Socket]
        public BlockWithChildren Dropdown { get; set; } = new LinearLayout() {
            Depth = 1, // TODO: ???
            Vertical = true,
            BackgroundColor = DefaultBackgroundColor,
            Paddings = 5,
            RadiusTopLeft= 5,
            RadiusBottomLeft= 5,
            RadiusBottomRight = 5,
            Visibility = new(e => ((JMenuResponsive)e.Parent).DropdownExpanded ? 1 : 0)
        };

        public MenuResponsive([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) { }

        /*protected override void ModifyHtml(Context context, Tag elementTag) {

            var logo = Logo;
            if (logo != null) {
                elementTag.Add(CreateScript_SetCurrentSocket("Logo"));
                elementTag.Add(logo.GenerateHtml(context));
            }

            elementTag.Add(CreateScript_SetCurrentSocket("Button"));
            elementTag.Add(Button.GenerateHtml(context));

            elementTag.Add(CreateScript_SetCurrentSocket("Dropdown"));
            elementTag.Add(Dropdown.GenerateHtml(context));

            base.ModifyHtml(context, elementTag);
        }*/
    }
}
