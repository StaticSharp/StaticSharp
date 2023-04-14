using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;


namespace StaticSharp {

    public interface JBlock : JBaseModifier {

        public double X { get; set; }
        public double Y { get; set; }

        public double AbsoluteX { get; }
        public double AbsoluteY { get; }


        public double Width { get; set; }
        public double Height { get; set; }

        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }
        public double MarginTop { get; set; }
        public double MarginBottom { get; set; }

        public double PaddingLeft { get; set; }
        public double PaddingRight { get; set; }
        public double PaddingTop { get; set; }
        public double PaddingBottom { get; set; }

        public double PaddingsHorizontal {
            set {
                PaddingLeft = value;
                PaddingRight = value;
            }
        }

        public double PaddingsVertical {
            set {
                PaddingTop = value;
                PaddingBottom = value;
            }
        }

        public double Paddings {
            set {
                PaddingsHorizontal = value;
                PaddingsVertical = value;
            }
        }

        public double MarginsHorizontal {
            set {
                MarginLeft = value;
                MarginRight = value;
            }
        }

        public double MarginsVertical {
            set {
                MarginTop = value;
                MarginBottom = value;
            }
        }

        public double Margins {
            set {
                MarginsHorizontal = value;
                MarginsVertical = value;
            }
        }

        public double FontSize { get; set; }
        public int Depth { get; set; }

        //public bool ClipByParent { get; }

        public new JBlock Parent { get; }
        public JBlock NextSibling { get; }
        public Js.Enumerable<JBlock> UnmanagedChildren { get; }
    }

    [ConstructorJs]    
    public partial class Block : BaseModifier {

        [Socket]
        public virtual Blocks UnmanagedChildren { get; } = new();

        public Block(Block other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerLineNumber, callerFilePath) {
            UnmanagedChildren = new(other.UnmanagedChildren);
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

        public static T FillWidth<T>(this T _this) where T : Block {
            _this.X = new(e => Js.Num.First(e.MarginLeft, 0));
            _this.Width = new(e => Js.Num.Sum(e.Parent.Width, -e.MarginLeft, -e.MarginRight));
            return _this;
        }

        public static T FillHeight<T>(this T _this) where T : Block {
            _this.Y = new(e => Js.Num.First(e.MarginTop, 0));
            _this.Height = new(e => Js.Num.Sum(e.Parent.Height, -e.MarginTop, -e.MarginBottom));
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