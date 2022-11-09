using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public class BaseModifier : Hierarchical {

            public Color BackgroundColor => NotEvaluatableValue<Color>();
            public Color ForegroundColor => NotEvaluatableValue<Color>();
            public bool Hover => NotEvaluatableValue<bool>();
            public bool Selectable => NotEvaluatableValue<bool>();

            public double Visibility => NotEvaluatableValue<double>();

            public double Radius => NotEvaluatableValue<double>();
            public double RadiusTopLeft => NotEvaluatableValue<double>();
            public double RadiusTopRight => NotEvaluatableValue<double>();
            public double RadiusBottomLeft => NotEvaluatableValue<double>();
            public double RadiusBottomRight => NotEvaluatableValue<double>();

        }
    }

    namespace Gears {
        public class BaseModifierBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
            public Binding<Color> BackgroundColor { set { Apply(value); } }
            public Binding<Color> ForegroundColor { set { Apply(value); } }
            public Binding<bool> Selectable { set { Apply(value); } }
            public Binding<double> Visibility { set { Apply(value); } }            
            public Binding<double> Radius { set { Apply(value); } }
            public Binding<double> RadiusTopLeft { set { Apply(value); } }
            public Binding<double> RadiusTopRight { set { Apply(value); } }
            public Binding<double> RadiusBottomLeft { set { Apply(value); } }
            public Binding<double> RadiusBottomRight { set { Apply(value); } }

        }
    }

    namespace Gears {

        [Mix(typeof(BaseModifierBindings<Js.BaseModifier>))]
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


            protected override Context ModifyContext(Context context) {
                if (FontFamilies != null) {
                    context.FontFamilies = FontFamilies;
                }
                if (FontStyle != null) {
                    context.FontStyle = FontStyle;
                }
                return context;
            }

            protected override Task ModifyHtmlAsync(Context context, Tag elementTag) {
                //protected void ModifyTag(Tag tag) {
                /*if (Url != null) {
                    tag.Name = "a";
                    tag["href"] = Url;
                }*/

                if (Tooltip != null) {
                    elementTag["title"] = Tooltip;
                }

                if (LineHeight != null) {
                    elementTag.Style["line-height"] = LineHeight;
                }

                if (LetterSpacing != null){
                    elementTag.Style["letter-spacing"] = LetterSpacing+"em";
                }

                if (FontFamilies != null) {
                    elementTag.Style["font-family"] = string.Join(',', FontFamilies.Select(x => x.Name));
                }
                if (FontStyle != null) {
                    elementTag.Style["font-weight"] = (int)FontStyle.Weight;
                    elementTag.Style["font-style"] = FontStyle.CssFontStyle;
                }

                return base.ModifyHtmlAsync(context, elementTag);
            }

        }
    }
}