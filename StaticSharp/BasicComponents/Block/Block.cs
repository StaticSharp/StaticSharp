using Scopes;
using Scopes.C;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public interface Block : BaseModifier {
            
            public double X { get; }
            public double Y { get; }
            public double Width { get; }
            public double Height { get; }

            //public double LayoutX { get; }
            //public double LayoutY  { get; }
            //public double LayoutWidth  { get; }
            //public double LayoutHeight  { get; }


            //public double PreferredWidth { get; }
            //public double PreferredHeight { get; }
            //public double Grow { get; }
            //public double Shrink { get; }

            //public double MinHeight { get; }
            //public double MaxWidth { get; }
            //public double MaxHeight { get; }

            //public double InternalWidth { get; }
            //public double InternalHeight { get; }


            public double MarginLeft  { get; }
            public double MarginRight  { get; }
            public double MarginTop  { get; }
            public double MarginBottom  { get; }

            public double PaddingLeft  { get; }
            public double PaddingRight  { get; }
            public double PaddingTop  { get; }
            public double PaddingBottom  { get; }

            public double FontSize  { get; }
            public int Depth { get; }

            public bool ClipByParent { get; }



            public new Block Parent { get; }
            public new Block FirstChild { get; }
            public new Block NextSibling { get; }
            public new Enumerable<Block> Children { get; }

            public Block Layer { get; } // TODO:
        }
    }



    namespace Gears {
        public class BlockBindings<FinalJs> : BaseModifierBindings<FinalJs> {
            public Binding<double> X { set { Apply(value); } }
            public Binding<double> Y { set { Apply(value); } }
            public Binding<double> Width { set { Apply(value); } }
            public Binding<double> Height { set { Apply(value); } }


            //public Binding<double> PreferredWidth { set { Apply(value); } }
            //public Binding<double> PreferredHeight { set { Apply(value); } }

            //public Binding<double> Grow { set { Apply(value); } }
            //public Binding<double> Shrink { set { Apply(value); } }

            //public Binding<double> MinWidth { set { Apply(value); } }
            //public Binding<double> MinHeight { set { Apply(value); } }
            //public Binding<double> MaxWidth { set { Apply(value); } }
            //public Binding<double> MaxHeight { set { Apply(value); } }


            public Binding<double> MarginLeft { set { Apply(value); } }
            public Binding<double> MarginRight { set { Apply(value); } }
            public Binding<double> MarginTop { set { Apply(value); } }
            public Binding<double> MarginBottom { set { Apply(value); } }

            public Binding<double> MarginsHorizontal { set { Apply(value, "MarginLeft", "MarginRight"); } }
            public Binding<double> MarginsVertical { set { Apply(value, "MarginTop", "MarginBottom"); } }
            public Binding<double> Margins { set { Apply(value, "MarginLeft", "MarginRight", "MarginTop", "MarginBottom"); } }

            public Binding<double> PaddingLeft { set { Apply(value); } }
            public Binding<double> PaddingRight { set { Apply(value); } }
            public Binding<double> PaddingTop { set { Apply(value); } }
            public Binding<double> PaddingBottom { set { Apply(value); } }

            public Binding<double> PaddingsHorizontal { set { Apply(value, "PaddingLeft", "PaddingRight"); } }
            public Binding<double> PaddingsVertical { set { Apply(value, "PaddingTop", "PaddingBottom"); } }
            public Binding<double> Paddings { set { Apply(value, "PaddingLeft", "PaddingRight", "PaddingTop", "PaddingBottom"); } }
            public Binding<double> FontSize { set { Apply(value); } }

            public Binding<int> Depth { set { Apply(value); } }
            public Binding<bool> ClipByParent { set { Apply(value); } }

        }

    }


    [Mix(typeof(BlockBindings<Js.Block>))]
    [ConstructorJs]
    [RelatedScript("../Reactive/Layer")]
    public partial class Block : BaseModifier, IBlock {
        //public virtual List<Modifier> Modifiers { get; } = new();
        public virtual Blocks Children { get; } = new();
        //public Block? Overlay;

        protected Block(Block other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerLineNumber, callerFilePath) {
            Children = new(other.Children);
        }
        public Block([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }


        protected override void ModifyHtml(Context context, Tag elementTag) {
            
            var children = Children.ToArray();
            if (children.Length > 0) {
                elementTag.Add(CreateScript_SetCurrentSocket("FirstChild"));
                elementTag.Add(Children.GenerateHtml(context));
            }

            base.ModifyHtml(context, elementTag);
        }
    }

    public static partial class Static {

        public static T CenterHorizontally<T>(this T _this) where T : Block {
            _this.X = new(e => 0.5 * (e.Parent.Width - e.Width));
            return _this;
        }
        public static T CenterVertically<T>(this T _this) where T : Block {
            _this.Y = new(e => 0.5 * (e.Parent.Height - e.Height));
            return _this;
        }
        public static T Center<T>(this T _this) where T : Block {
            return _this.CenterHorizontally().CenterVertically();
        }

        //public static Block FillWidth(this Block _this) {
        //    return new Overrider(_this)
        //    {
        //        OverrideX = new(e => Js.Math.First(e.MarginLeft, 0) - Js.Math.First(e.Parent.PaddingLeft, 0)),
        //        OverrideWidth = new(e => e.Parent.Width)
        //    };
        //}

        public static T FillWidth<T>(this T _this) where T : Block
        {
            _this.X = new(e => Js.Math.First(e.MarginLeft, 0));
            _this.Width = new(e => Js.Math.Sum(e.Parent.Width, -e.MarginLeft, -e.MarginRight));
            return _this;
        }

        public static T FillHeight<T>(this T _this) where T : Block {
            _this.Y = new(e => Js.Math.First(e.MarginTop, 0));
            _this.Height = new(e => Js.Math.Sum(e.Parent.Height, -e.MarginTop, -e.MarginLeft));
            return _this;
        }

        public static Block InheritHorizontalPaddings(this Block _this) {
            return new Overrider(_this)
            {
                OverridePaddingLeft = new(e => e.Parent.PaddingLeft),
                OverridePaddingRight = new(e => e.Parent.PaddingLeft),
            };
        }
        
        //public static T InheritHorizontalPaddings<T>(this T _this) where T : Block {
        //    _this.PaddingLeft = new(e => e.Parent.PaddingLeft);
        //    _this.PaddingRight = new(e => e.Parent.PaddingRight);
        //    return _this;
        //}



    }


}