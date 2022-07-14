using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp {

    [System.Diagnostics.DebuggerNonUserCode]
    public class BaseModifierJs : HierarchicalJs {
        public BaseModifierJs() { }

        public float FontSize => throw new NotEvaluatableException();
        public Color BackgroundColor => throw new NotEvaluatableException();
    }
    


    namespace Gears {

        [ScriptBefore]
        [ScriptAfter]
        public abstract class BaseModifier: Hierarchical<BaseModifierJs> {

            public Color? ForgroundColor = null;
            


            public Space? DefaultSpace = null;


            public FontFamily[]? FontFamilies = null;
            public FontStyle? FontStyle = null;
            //public float? FontSize = null;
            public Binding<float> FontSize { set; protected get; }
            public Binding<Color> BackgroundColor { set; protected get; }


            public BaseModifier(string callerFilePath, int callerLineNumber)
            : base(callerFilePath, callerLineNumber) { }


            public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(ThisFilePathWithNewExtension("js")));
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

                AddRequiredInclues(context.Includes);

                var tag = new Tag(tagName,id) {
                    
                    Children = {
                        CreateScriptBefore(),
                        await children(context).SequentialOrParallel(),
                        CreateScriptAfter()
                    }
                };

                tag.Style = style;


                return tag;
            }
        }
    }
}