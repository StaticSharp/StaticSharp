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
        public Color BackgroundColor { get; set; }
        public Color HierarchyBackgroundColor { get; }
        public Color ForegroundColor { get; set; }
        public Color HierarchyForegroundColor { get; }
        public Js.Enumerable<JModifier> Modifiers { get; }

        public double Visibility { get; set; }

        /*public double Radius { get; set; }
        public double RadiusTopLeft { get; set; }
        public double RadiusTopRight { get; set; }
        public double RadiusBottomLeft { get; set; }
        public double RadiusBottomRight { get; set; }*/

    }


    [Scripts.Color]
    [ConstructorJs]
    public abstract partial class BaseModifier: Hierarchical {

        protected override string TagName => IsLink? "a" : base.TagName;
        public Modifiers Modifiers { get; } = new();
        private bool IsLink => InternalLink != null || ExternalLink != null;
        public string? ExternalLink { get; set; }
        public Tree.Node? InternalLink { get; set; }
        public bool OpenLinksInANewTab { get; set; }            
        //public FontFamilyGenome[]? CodeFontFamilies { get; set; } = null;

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

        public string? Tooltip = null;

        public double? LineHeight = null;//line-height
        public double? LetterSpacing = null;//letter-spacing


        public BaseModifier(BaseModifier other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
            Modifiers = new Modifiers(other.Modifiers);
            ExternalLink = other.ExternalLink;
            InternalLink = other.InternalLink;
            OpenLinksInANewTab = other.OpenLinksInANewTab;
            //CodeFontFamilies = other.CodeFontFamilies?.ToArray();
            FontFamilies = new FontFamilies(other.FontFamilies);
            Weight = other.Weight;
            Italic = other.Italic;
            Tooltip = other.Tooltip;
            LineHeight = other.LineHeight;
            LetterSpacing = other.LetterSpacing;
        }


        public string? GetUrl(Context context) {
            if (InternalLink != null) {
                var url = context.NodeToUrlRelativeToCurrentNode(InternalLink);
                return url.ToString();
            } else {
                return ExternalLink;
            }
        }


        public override void ModifyTagAndScript(Context context, Tag tag, Group scriptBeforeConstructor, Group scriptAfterConstructor) {
            base.ModifyTagAndScript(context, tag, scriptBeforeConstructor, scriptAfterConstructor);

            var url = GetUrl(context);
            if (url != null) {
                SetTagName(tag, "a");
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

            /*if (LetterSpacing != null) {
                tag.Style["letter-spacing"] = LetterSpacing + "em";
            }*/

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
                    var tagRename = m.TagRename;
                    if (tagRename != null) {
                        SetTagName(tag, tagRename);
                    }
                    
                    var generated = m.Generate(tag, context);
                    modifiersVariables.Add(generated.Id);
                    scriptBeforeConstructor.Add(generated.Script);
                }
                scriptBeforeConstructor.Add($"{tag.Id}.Modifiers = [{string.Join(',', modifiersVariables)}]");
            }



        }


        protected override Context ModifyContext(Context context) {
            context = base.ModifyContext(context);

            if (fontFamilies != null) {
                context.FontFamilies = fontFamilies;
            }
            /*if (CodeFontFamilies != null) {
                context.CodeFontFamilies = CodeFontFamilies;
            }*/

            if (LetterSpacing != null) {
                context.LetterSpacing = LetterSpacing.Value;
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