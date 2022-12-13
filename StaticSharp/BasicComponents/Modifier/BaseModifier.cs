using ImageMagick;
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
            public Color HierarchyBackgroundColor => NotEvaluatableValue<Color>();
            public Color ForegroundColor => NotEvaluatableValue<Color>();
            public Color HierarchyForegroundColor => NotEvaluatableValue<Color>();



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
            public Binding<Color> TextDecorationColor { set { Apply(value); } }

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

        [RelatedScript("../../CrossplatformLibrary/Color/Color")]
        [ConstructorJs]
        public abstract partial class BaseModifier: Hierarchical {

            public string? ExternalLink { get; set; }
            public Tree.Node? InternalLink { get; set; }
            public bool OpenLinksInANewTab { get; set; }
            
            public FontFamilyGenome[]? CodeFontFamilies { get; set; } = null;
            public FontFamilyGenome[]? FontFamilies { get; set; } = null;



            public FontWeight? Weight { get; set; } = null;
            public bool? Italic { get; set; } = null;





            //public string? Url = null;
            public string? Tooltip = null;

            public float? LineHeight = null;//line-height
            public float? LetterSpacing = null;//letter-spacing


            protected BaseModifier(Hierarchical other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {}
            public BaseModifier(int callerLineNumber, string callerFilePath) : base(callerLineNumber, callerFilePath) { }


            public string? GetUrl(Context context) {
                if (InternalLink != null) {
                    var url = context.NodeToUrlRelativeToCurrentNode(InternalLink);
                    return url.ToString();
                } else {
                    return ExternalLink;
                }
            }

            public override Tag GenerateHtml(Context context, Role? role) {
                var url = GetUrl(context);
                if (url == null) {
                    return base.GenerateHtml(context, role);
                } else {
                    return new Tag("a") {
                        ["href"] = url,
                        ["target"] = OpenLinksInANewTab ? "_blank" : "_self",
                        Style = {
                            ["display"] = "contents",
                        },
                        Children = {
                            base.GenerateHtml(context, role)
                        }
                    };
                }
            }



            protected override Context ModifyContext(Context context) {
                if (FontFamilies != null) {
                    context.FontFamilies = FontFamilies;
                }
                if (CodeFontFamilies != null) {
                    context.CodeFontFamilies = CodeFontFamilies;
                }

                if (Weight != null) {
                    context.FontWeight = Weight.Value;
                }

                if (Italic != null) {
                    context.ItalicFont = Italic.Value;
                }


                return context;
            }

            protected override void ModifyHtml(Context context, Tag elementTag) {
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

                if (Weight != null) {
                    elementTag.Style["font-weight"] = (int)Weight.Value;
                }

                if (Italic != null) {
                    elementTag.Style["font-style"] = Italic.Value ? "italic" : "normal";
                }


                base.ModifyHtml(context, elementTag);
            }

        }
    }
}