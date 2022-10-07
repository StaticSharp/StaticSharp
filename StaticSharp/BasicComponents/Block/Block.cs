using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {
    
    [System.Diagnostics.DebuggerNonUserCode]
    public class BlockJs : BaseModifierJs {
        public float X => NotEvaluatableValue<float>();
        public float Y => NotEvaluatableValue<float>();
        public float Width => NotEvaluatableValue<float>();
        public float Height => NotEvaluatableValue<float>();
        public float MarginLeft => NotEvaluatableValue<float>();
        public float MarginRight => NotEvaluatableValue<float>();
        public float MarginTop => NotEvaluatableValue<float>();
        public float MarginBottom => NotEvaluatableValue<float>();
        public float PaddingLeft => NotEvaluatableValue<float>();
        public float PaddingRight => NotEvaluatableValue<float>();
        public float PaddingTop => NotEvaluatableValue<float>();
        public float PaddingBottom => NotEvaluatableValue<float>();
        public float FontSize => NotEvaluatableValue<float>();
    }



    namespace Gears {
        public class BlockBindings<FinalJs> : BaseModifierBindings<FinalJs> where FinalJs : new() {
            public Binding<float> X { set { Apply(value); } }
            public Binding<float> Y { set { Apply(value); } }
            public Binding<float> Width { set { Apply(value); } }
            public Binding<float> Height { set { Apply(value); } }



            public Binding<float> MarginLeft { set { Apply(value); } }
            public Binding<float> MarginRight { set { Apply(value); } }
            public Binding<float> MarginTop { set { Apply(value); } }
            public Binding<float> MarginBottom { set { Apply(value); } }

            public Binding<float> MarginsHorizontal { set { Apply(value, "MarginLeft", "MarginRight"); } }
            public Binding<float> MarginsVertical { set { Apply(value, "MarginTop", "MarginBottom"); } }
            public Binding<float> Margins { set { Apply(value, "MarginLeft", "MarginRight", "MarginTop", "MarginBottom"); } }




            public Binding<float> PaddingLeft { set { Apply(value); } }
            public Binding<float> PaddingRight { set { Apply(value); } }
            public Binding<float> PaddingTop { set { Apply(value); } }
            public Binding<float> PaddingBottom { set { Apply(value); } }

            public Binding<float> PaddingsHorizontal { set { Apply(value, "PaddingLeft", "PaddingRight"); } }
            public Binding<float> PaddingsVertical { set { Apply(value, "PaddingTop", "PaddingBottom"); } }
            public Binding<float> Paddings { set { Apply(value, "PaddingLeft", "PaddingRight", "PaddingTop", "PaddingBottom"); } }




            public Binding<float> FontSize { set { Apply(value); } }

        }

    }


    [Mix(typeof(BlockBindings<BlockJs>))]
    [ConstructorJs]
    public partial class Block : BaseModifier, IBlock {
        //public virtual List<Modifier> Modifiers { get; } = new();

        public Block? Overlay;

        protected Block(Block other,
            string callerFilePath = "",
            int callerLineNumber = 0) : base(other, callerFilePath, callerLineNumber) {

            //Modifiers = new(other.Modifiers);
        }
        public Block([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }
        
        protected virtual Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return Task.FromResult<Tag?>(null);
        }
        public virtual async Task<Tag> GenerateHtmlAsync(Context context, string? id = null) {

            await AddRequiredInclues(context);

            /*foreach (var m in Modifiers) {
                await m.AddRequiredInclues(context);
                context = m.ModifyContext(context);
            }*/

            context = ModifyContext(context);

            var tag = new Tag(TagName, id) { };

            /*foreach (var m in Modifiers)
                m.ModifyTag(tag);*/

            ModifyTag(tag);

            AddSourceCodeNavigationData(tag, context);

            tag.Add(await CreateConstructorScriptAsync(context));

            /*foreach (var m in Modifiers)
                tag.Add(await m.CreateConstructorScriptAsync(context));*/


            tag.Add(await GenerateHtmlInternalAsync(context, tag));


            if (Overlay != null) {
                string? overlayId = null;
                if (id != null) {
                    overlayId = id + "Overlay";
                }
                tag.Add(new Tag("overlay") {
                    await Overlay.GenerateHtmlAsync(context,overlayId)
                });
            }


            //tag.Add(After());

            return tag;
        }

    }

    public static partial class Static {

        public static T FillWidth<T>(this T _this) where T : Block {
            _this.X = 0;
            _this.Width = new(e => e.ParentBlock.Width);
            return _this;
        }

        public static T InheritPaddings<T>(this T _this) where T : Block {
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