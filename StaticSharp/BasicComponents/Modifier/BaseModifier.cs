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
        public BaseModifierJs() { }

        public float FontSize =>        NotEvaluatableValue<float>();
        public Color BackgroundColor => NotEvaluatableValue<Color>();
    }

    public class BaseModifierBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
        public BaseModifierBindings(Dictionary<string, string> properties) : base(properties) {
        }
        public Expression<Func<FinalJs, float>> FontSize { set { AssignProperty(value); } }
        public Expression<Func<FinalJs, Color>> BackgroundColor { set { AssignProperty(value); } }
    }



    namespace Gears {

        [RelatedScript]
        public abstract class BaseModifier: Hierarchical {

            public new BaseModifierBindings<BaseModifierJs> Bindings => new(Properties);

            public Color? ForgroundColor = null;
            


            public Space? DefaultSpace = null;


            public FontFamily[]? FontFamilies = null;
            public FontStyle? FontStyle = null;

            

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

                if (FontFamilies != null) {
                    tag.Style["font-family"] = string.Join(',', FontFamilies.Select(x => x.Name));
                }
                if (FontStyle != null) {
                    tag.Style["font-weight"] = (int)FontStyle.Weight;
                    tag.Style["font-style"] = FontStyle.CssFontStyle;
                }
            }


            protected async Task<Tag> GenerateHtmlWithChildrenAsync(Context context, string? id, Func<Context,IEnumerable<Task<Tag>?>> children, string tagName = "m") {

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
                /*if (FontSize != null) {
                    context.FontSize = FontSize.Value;
                }*/


                //context.Includes.Require(await context.GetCacheableFont());

                AddRequiredInclues(context);

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
            }
        }
    }
}