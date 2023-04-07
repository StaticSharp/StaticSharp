using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;


namespace StaticSharp {

    namespace Js {
        public interface Block : BaseModifier {
            
            public double X { get; }
            public double Y { get; }

            public double AbsoluteX { get; }
            public double AbsoluteY { get; }


            public double Width { get; }
            public double Height { get; }

            public double MarginLeft  { get; }
            public double MarginRight  { get; }
            public double MarginTop  { get; }
            public double MarginBottom  { get; }

            public double PaddingLeft  { get; set; }
            public double PaddingRight  { get; set; }
            public double PaddingTop  { get; set; }
            public double PaddingBottom  { get; set; }

            public double PaddingsHorizontal { set {
                    PaddingLeft = value;
                    PaddingRight = value;
                }
            }

            public double FontSize  { get; }
            public int Depth { get; }

            public bool ClipByParent { get; }

            public new Block Parent { get; }
            public Block NextSibling { get; }
            public Js.Enumerable<Block> UnmanagedChildren { get; }
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

    //[Mix(typeof(AssignMixin<Block, Js.Block>))]
    [Mix(typeof(BlockBindings<Js.Block>))]
    [ConstructorJs]
    
    public partial class Block : BaseModifier {
        //public virtual List<Modifier> Modifiers { get; } = new();
        [Socket]
        public virtual Blocks UnmanagedChildren { get; } = new();

        protected Block(Block other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerLineNumber, callerFilePath) {
            UnmanagedChildren = new(other.UnmanagedChildren);
        }
        public Block([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        


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

        public static T FillWidth<T>(this T _this) where T : Block {
            _this.X = new(e => Js.Math.First(e.MarginLeft, 0));
            _this.Width = new(e => Js.Math.Sum(e.Parent.Width, -e.MarginLeft, -e.MarginRight));
            return _this;
        }

        public static T FillHeight<T>(this T _this) where T : Block {
            _this.Y = new(e => Js.Math.First(e.MarginTop, 0));
            _this.Height = new(e => Js.Math.Sum(e.Parent.Height, -e.MarginTop, -e.MarginBottom));
            return _this;
        }

        public static T InheritHorizontalPaddings<T>(this T _this) where T : Block {
            _this.PaddingLeft = new(e => e.Parent.PaddingLeft);
            _this.PaddingRight = new(e => e.Parent.PaddingRight);
            return _this;
        }

        // TODO: convert to "Wrap()"
        //public static Block Override(this Block _this, Action<LayoutOverride> overrideAction)
        //{
        //    var overrider = _this as LayoutOverride ?? new LayoutOverride(_this);
        //    overrideAction(overrider);
        //    return overrider;
        //}
    }


}