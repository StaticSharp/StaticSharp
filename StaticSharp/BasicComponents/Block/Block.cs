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
        public class Block : BaseModifier {
            public double X => NotEvaluatableValue<double>();
            public double Y => NotEvaluatableValue<double>();
            public double Width => NotEvaluatableValue<double>();
            public double Height => NotEvaluatableValue<double>();
            public double InternalWidth => NotEvaluatableValue<double>();
            public double InternalHeight => NotEvaluatableValue<double>();


            public double LayoutX => NotEvaluatableValue<double>();
            public double LayoutY => NotEvaluatableValue<double>();
            public double LayoutWidth => NotEvaluatableValue<double>();
            public double LayoutHeight => NotEvaluatableValue<double>();

            public double MarginLeft => NotEvaluatableValue<double>();
            public double MarginRight => NotEvaluatableValue<double>();
            public double MarginTop => NotEvaluatableValue<double>();
            public double MarginBottom => NotEvaluatableValue<double>();

            public double PaddingLeft => NotEvaluatableValue<double>();
            public double PaddingRight => NotEvaluatableValue<double>();
            public double PaddingTop => NotEvaluatableValue<double>();
            public double PaddingBottom => NotEvaluatableValue<double>();

            public double FontSize => NotEvaluatableValue<double>();
            public int Depth => NotEvaluatableValue<int>();
        }
    }



    namespace Gears {
        public class BlockBindings<FinalJs> : BaseModifierBindings<FinalJs> where FinalJs : new() {
            public Binding<double> X { set { Apply(value); } }
            public Binding<double> Y { set { Apply(value); } }
            public Binding<double> Width { set { Apply(value); } }
            public Binding<double> Height { set { Apply(value); } }



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

        }

    }


    [Mix(typeof(BlockBindings<Js.Block>))]
    [ConstructorJs]
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
        
        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {
            foreach (var c in Children) {
                var childTag = await c.Value.GenerateHtmlAsync(context, new Role(true, c.Key));
                elementTag.Add(childTag);
            }
            await base.ModifyHtmlAsync(context, elementTag);
        }
    }

    public static partial class Static {

        public static T CenterHorizontally<T>(this T _this) where T : Block {
            _this.X = new(e => 0.5 * (e.ParentBlock.Width - e.Width));
            return _this;
        }
        public static T CenterVertically<T>(this T _this) where T : Block {
            _this.Y = new(e => 0.5 * (e.ParentBlock.Height - e.Height));
            return _this;
        }
        public static T Center<T>(this T _this) where T : Block {
            return _this.CenterHorizontally().CenterVertically();
        }

        public static T FillWidth<T>(this T _this) where T : Block {
            _this.X = new(e => Js.Math.First(e.MarginLeft,0));
            _this.Width = new(e => Js.Math.Sum(e.ParentBlock.Width, -e.MarginLeft, -e.MarginRight) );
            return _this;
        }

        public static T FillHeight<T>(this T _this) where T : Block {
            _this.Y = new(e => Js.Math.First(e.MarginTop, 0));
            _this.Height = new(e => Js.Math.Sum(e.ParentBlock.Height, -e.MarginTop, -e.MarginLeft));
            return _this;
        }

        public static T InheritHorizontalPaddings<T>(this T _this) where T : Block {
            _this.PaddingLeft = new(e => e.ParentBlock.PaddingLeft);
            _this.PaddingRight = new(e => e.ParentBlock.PaddingRight);
            return _this;
        }

        /*public static T ParentHorizontalMarginsToPaddings<T>(this T _this) where T : Block {
            _this.X = new(e => -e.ParentBlock.MarginLeft);
            _this.Width = new(e => e.ParentBlock.Width + e.ParentBlock.MarginLeft + e.ParentBlock.MarginRight);
            _this.PaddingLeft = new(e => e.ParentBlock.MarginLeft);
            _this.PaddingRight = new(e => e.ParentBlock.MarginRight);
            return _this;
        }

        public static T ParentHorizontalMarginsToWidth<T>(this T _this) where T : Block {
            _this.X = new(e => -e.ParentBlock.MarginLeft);
            _this.Width = new(e => e.ParentBlock.Width + e.ParentBlock.MarginLeft + e.ParentBlock.MarginRight);
            return _this;
        }*/


    }


}