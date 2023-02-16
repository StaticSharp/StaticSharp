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
            /*public Block First => NotEvaluatableObject<Block>();
            public Block Second => NotEvaluatableObject<Block>();
            public bool Flipped { get; }

            public bool RightToLeft { get; }
            public bool BottomToTop { get; }*/
        }
    }


    namespace Gears {
        public class MenuResponsiveBindings<FinalJs> : BlockBindings<FinalJs>  {
            /*public Binding<bool> Flipped { set { Apply(value); } }
            public Binding<bool> RightToLeft { set { Apply(value); } }
            public Binding<bool> BottomToTop { set { Apply(value); } }*/
        }
    }


    [Mix(typeof(MenuResponsiveBindings<Js.MenuResponsive>))]
    [ConstructorJs]
    public partial class MenuResponsive : Block, IBlock {

        /*public required Block First { get; init; }
        public required Block Second { get; init; }*/
        public Block? Logo { get; set; } = null;

        public Block Button { get; set; } = new SvgIconBlock(SvgIcons.MaterialDesignIcons.Menu);
        public Block Dropdown { get; set; } = new Column() {
            X = new(e=>e.Parent.Width - e.Width),
            BackgroundColor = Color.FromGrayscale(0.9),
            Paddings = 5,
            Radius = 5
        };

        

        public MenuResponsive([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag) {

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
        }
    }
}
