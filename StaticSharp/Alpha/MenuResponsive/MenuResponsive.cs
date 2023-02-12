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
            public Binding<bool> Flipped { set { Apply(value); } }
            public Binding<bool> RightToLeft { set { Apply(value); } }
            public Binding<bool> BottomToTop { set { Apply(value); } }
        }
    }


    [Mix(typeof(MenuResponsiveBindings<Js.MenuResponsive>))]
    [ConstructorJs]
    public partial class MenuResponsive : Block, IBlock {

        /*public required Block First { get; init; }
        public required Block Second { get; init; }*/

        public Block Dropdown { get; set; } = new Block() { 
            BackgroundColor = Color.Gray,
            Radius = 5
        };


        public MenuResponsive([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag) {
            /*elementTag.Add(                
                First.GenerateHtml(context,new Role(false,"First"))
            );*/
            elementTag.Add(
                Dropdown.GenerateHtml(context, new Role(false, "Dropdown"))
            );


            base.ModifyHtml(context, elementTag);
        }
    }
}
