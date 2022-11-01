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
        public Blocks Children { get; } = new();
        //public Block? Overlay;

        protected Block(Block other,
            string callerFilePath = "",
            int callerLineNumber = 0) : base(other, callerFilePath, callerLineNumber) {
            Children = new(other.Children);
        }
        public Block([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }
        
        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {
            foreach (var c in Children) {
                var childTag = await c.Value.GenerateHtmlAsync(context);
                if (c.Key != null) {
                    await c.Key.Apply(childTag);
                }
                childTag.AddAsChild();
                elementTag.Add(childTag);
            }
            await base.ModifyHtmlAsync(context, elementTag);
        }

        

        /*public virtual async Task<Node> GenerateConstructor(Context context, string? id) {
            
            await AddRequiredInclues(context);
            context = ModifyContext(context);

            var jsConstructorsNames = FindJsConstructorsNames();
            var propertiesInitializers = await GetGeneratedBundingsAsync(context).ToListAsync();
            propertiesInitializers.AddRange(Properties);



            var result = new Group() {
                $"let result = Create({string.Join(',', jsConstructorsNames)})",
                "result.Parent = parent"
            };


            foreach (var child in Children) {
                result.Add(new Indent($"result.AddChild(function(parent){{", "}(result))") {
                    child.Value.GenerateConstructor(context, id)
                });
            }
            

            foreach (var initializer in propertiesInitializers) {
                result.Add(new Indent($"result.{initializer.Key} = function(parent){{", "}(result)") {
                    initializer.Value
                });
            }

            result.Add("return result");
            return result;
        }*/
    }

    public static partial class Static {

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