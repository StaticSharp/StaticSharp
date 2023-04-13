using ColorCode.Styling;
using ImageMagick;
using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StaticSharp {

    public interface JBaseModifier : JHierarchical {

        public new JBaseModifier Parent { get; }
        public Color BackgroundColor { get; }
        public Color HierarchyBackgroundColor { get; }
        public Color ForegroundColor { get; }
        public Color HierarchyForegroundColor { get; }
        public Js.Enumerable<JModifier> Modifiers { get; }

        public bool Hover { get; }
        public bool? Selectable { get; }

        public double Visibility { get; }

        public double Radius { get; }
        public double RadiusTopLeft { get; }
        public double RadiusTopRight { get; }
        public double RadiusBottomLeft { get; }
        public double RadiusBottomRight { get; }

    }

    namespace Gears {
        public class BaseModifierBindings<FinalJs> : HierarchicalBindings<FinalJs> {
            public Binding<Color> BackgroundColor { set { Apply(value); } }
            public Binding<Color> ForegroundColor { set { Apply(value); } }
            public Binding<Color> TextDecorationColor { set { Apply(value); } }
            public Binding<bool?> Selectable { set { Apply(value); } }
            public Binding<double> Visibility { set { Apply(value); } }            
            public Binding<double> Radius { set { Apply(value); } }
            public Binding<double> RadiusTopLeft { set { Apply(value); } }
            public Binding<double> RadiusTopRight { set { Apply(value); } }
            public Binding<double> RadiusBottomLeft { set { Apply(value); } }
            public Binding<double> RadiusBottomRight { set { Apply(value); } }

        }
    }

    namespace Gears {

        [Mix(typeof(BaseModifierBindings<JBaseModifier>))]
        [Scripts.Color]
        [ConstructorJs]
        public abstract partial class BaseModifier: Hierarchical {

            protected override string TagName => IsLink? "a" : base.TagName;

            public Modifiers Modifiers { get; } = new();

            private bool IsLink => InternalLink != null || ExternalLink != null;

            public string? ExternalLink { get; set; }
            public Tree.Node? InternalLink { get; set; }
            public bool OpenLinksInANewTab { get; set; }
            
            public FontFamilyGenome[]? CodeFontFamilies { get; set; } = null;


            private FontFamilies? fontFamilies = null;
            public FontFamilies FontFamilies {
                get {
                    if (fontFamilies == null)
                        fontFamilies = new();
                    return fontFamilies;
                }
                set {
                    fontFamilies = value;
                }
            }



            public FontWeight? Weight { get; set; } = null;
            public bool? Italic { get; set; } = null;





            //public string? Url = null;
            public string? Tooltip = null;

            public double? LineHeight = null;//line-height
            public double? LetterSpacing = null;//letter-spacing


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


            public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
                base.ModifyTagAndScript(context, tag, script);
                
                var url = GetUrl(context);
                if (url != null) {
                    tag.Name = "a";
                    tag["href"] = url;
                    tag["target"] = OpenLinksInANewTab ? "_blank" : "_self";
                    //tag.Style["display"] = "contents";

                }

                if (Tooltip != null) {
                    tag["title"] = Tooltip;
                }

                if (LineHeight != null) {
                    tag.Style["line-height"] = LineHeight;
                }

                if (LetterSpacing != null) {
                    tag.Style["letter-spacing"] = LetterSpacing + "em";
                }

                if (fontFamilies != null) {
                    tag.Style["font-family"] = string.Join(',', fontFamilies.Select(x => x.Name));
                }

                if (Weight != null) {
                    tag.Style["font-weight"] = (int)Weight.Value;
                }

                if (Italic != null) {
                    tag.Style["font-style"] = Italic.Value ? "italic" : "normal";
                }

                if (Modifiers.Any()) {

                    List<string> modifiersVariables = new();

                    foreach (var m in Modifiers) { 
                        var generated = m.Generate(tag.Id, context);
                        modifiersVariables.Add(generated.Id);
                        script.Add(generated.Script);
                    }

                    script.Add($"{tag.Id}.Modifiers = new Enumerable([{string.Join(',', modifiersVariables)}])");
                }



            }


            protected override Context ModifyContext(Context context) {
                context = base.ModifyContext(context);

                if (fontFamilies != null) {
                    context.FontFamilies = fontFamilies;
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


        }
    }
}