using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StaticSharp {

    [System.Diagnostics.DebuggerNonUserCode]
    public class BaseModifierJs : HierarchicalJs {
        
        public Color BackgroundColor => NotEvaluatableValue<Color>();
        public Color ForegroundColor => NotEvaluatableValue<Color>();
        public bool Hover => NotEvaluatableValue<bool>();
        public bool Selectable => NotEvaluatableValue<bool>();

        public float Radius => NotEvaluatableValue<float>();
        public float RadiusTopLeft => NotEvaluatableValue<float>();
        public float RadiusTopRight => NotEvaluatableValue<float>();
        public float RadiusBottomLeft => NotEvaluatableValue<float>();
        public float RadiusBottomRight => NotEvaluatableValue<float>();

    }

    public class BaseModifierBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
        
        public Binding<Color> BackgroundColor { set { Apply(value); } }
        public Binding<Color> ForegroundColor { set { Apply(value); } }
        public Binding<bool> Selectable { set { Apply(value); } }

        public Binding<float> Radius            { set { Apply(value); } }
        public Binding<float> RadiusTopLeft     { set { Apply(value); } }
        public Binding<float> RadiusTopRight    { set { Apply(value); } }
        public Binding<float> RadiusBottomLeft  { set { Apply(value); } }
        public Binding<float> RadiusBottomRight { set { Apply(value); } }       


    }



    namespace Gears {

        [Mix(typeof(BaseModifierBindings<BaseModifierJs>))]
        [ConstructorJs]
        public abstract partial class BaseModifier: Hierarchical {

            public FontFamily[]? FontFamilies = null;
            public FontStyle? FontStyle = null;
            //public string? Url = null;
            public string? Tooltip = null;

            public float? LineHeight = null;//line-height
            public float? LetterSpacing = null;//letter-spacing


            protected BaseModifier(Hierarchical other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {}
            public BaseModifier(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }


            public Context ModifyContext(Context context) {
                if (FontFamilies != null) {
                    context.FontFamilies = FontFamilies;
                }
                if (FontStyle != null) {
                    context.FontStyle = FontStyle;
                }
                return context;
            }

            public void ModifyTag(Tag tag) {
                /*if (Url != null) {
                    tag.Name = "a";
                    tag["href"] = Url;
                }*/

                if (Tooltip != null) {
                    tag["title"] = Tooltip;
                }

                if (LineHeight != null) {
                    tag.Style["line-height"] = LineHeight;
                }

                if (LetterSpacing != null){
                    tag.Style["letter-spacing"] = LetterSpacing+"em";
                }

                if (FontFamilies != null) {
                    tag.Style["font-family"] = string.Join(',', FontFamilies.Select(x => x.Name));
                }
                if (FontStyle != null) {
                    tag.Style["font-weight"] = (int)FontStyle.Weight;
                    tag.Style["font-style"] = FontStyle.CssFontStyle;
                }
            }

        }
    }
}