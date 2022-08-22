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


    }

    public class BaseModifierBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
        public BaseModifierBindings(Dictionary<string, string> properties) : base(properties) {}
        
        public Expression<Func<FinalJs, Color>> BackgroundColor { set { AssignProperty(value); } }

        public Expression<Func<FinalJs, Color>> ForegroundColor { set { AssignProperty(value); } }

    }


    public static partial class BaseModifierStatic { // For bindings

        public static T BackgroundColor<T>(this T _this, Expression<Func<BaseModifierJs, Color>> expression) where T : BaseModifier {
            _this.Bindings.BackgroundColor = expression;
            return _this;
        }
        public static T BackgroundColor<T>(this T _this, Color value) where T : BaseModifier {
            _this.Bindings.BackgroundColor = e => value;
            return _this;
        }

        public static T ForegroundColor<T>(this T _this, Expression<Func<BaseModifierJs, Color>> expression) where T : BaseModifier {
            _this.Bindings.ForegroundColor = expression;
            return _this;
        }
        public static T ForegroundColor<T>(this T _this, Color value) where T : BaseModifier {
            _this.Bindings.ForegroundColor = e => value;
            return _this;
        }

    }

    public static partial class BaseModifierStatic {
        
        public static T Url<T>(this T _this, string url) where T: BaseModifier {
            _this.Url = url;
            return _this;
        }
        public static T Title<T>(this T _this, string title) where T : BaseModifier {
            _this.Title = title;
            return _this;
        }
        public static T FontFamilies<T>(this T _this, FontFamily[] fontFamilies) where T : BaseModifier {
            _this.FontFamilies = fontFamilies;
            return _this;
        }
        public static T FontStyle<T>(this T _this, FontStyle fontStyle) where T : BaseModifier {
            _this.FontStyle = fontStyle;
            return _this;
        }

        public static T FontStyle<T>(this T _this, FontWeight weight = FontWeight.Regular,bool italic = false) where T : BaseModifier {
            _this.FontStyle = new FontStyle(weight, italic);
            return _this;
        }

        public static T LineHeight<T>(this T _this, float lineHeight) where T : BaseModifier {
            _this.LineHeight = lineHeight;
            return _this;
        }

        public static T LetterSpacing<T>(this T _this, float letterSpacing) where T : BaseModifier {
            _this.LetterSpacing = letterSpacing;
            return _this;
        }

    }


    namespace Gears {

        [RelatedScript]
        public abstract class BaseModifier: Hierarchical {

            public new BaseModifierBindings<BaseModifierJs> Bindings => new(Properties);

            public FontFamily[]? FontFamilies = null;
            public FontStyle? FontStyle = null;
            public string? Url = null;
            public string? Title = null;

            public float? LineHeight = null;//line-height
            public float? LetterSpacing = null;//letter-spacing

            public BaseModifier(string callerFilePath, int callerLineNumber)
            : base(callerFilePath, callerLineNumber) { }


            /*public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(ThisFilePathWithNewExtension("js")));
            }*/

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
                if (Url != null) {
                    tag.Name = "a";
                    tag["href"] = Url;
                }

                if (Title != null) {
                    tag["title"] = Title;
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

            /*
             
             */



            /*protected async Task<Tag> GenerateHtmlWithChildrenAsync(Context context, string? id, Func<Context,IEnumerable<Task<Tag>?>> children, string tagName = "m") {

                Dictionary<string, object> style = new();


                if (FontFamilies != null) {
                    context.FontFamilies = FontFamilies;
                    style["font-family"] = string.Join(',', FontFamilies.Select(x => x.Name));
                    //tag.Style["font-family"] = "abc";
                }
                if (FontStyle != null) {
                    context.FontStyle = FontStyle;
                    style["font-weight"] = (int)FontStyle.Weight;
                    style["font-style"] = FontStyle.CssFontStyle;

                }

                var tag = new Tag(tagName,id) {
                    
                    Children = {
                        await CreateScript(context),
                        //CreateScriptBefore(),
                        await children(context).SequentialOrParallel(),
                        //CreateScriptAfter()
                    }
                };

                tag.Style = style;


                return tag;
            }*/
        }
    }
}