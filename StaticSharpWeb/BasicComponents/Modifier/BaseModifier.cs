using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace StaticSharp {


    public class BaseModifierJs : HierarchicalJs {
        public BaseModifierJs() { }
        public BaseModifierJs(string value) : base(value) {
        }

        public NumberJs FontSize => new($"{value}.FontSize");
    }
    


    namespace Gears {

        [ScriptBefore]
        [ScriptAfter]
        public abstract class BaseModifier: Hierarchical<BaseModifierJs> {

            public Color? ForgroundColor = null;
            public Space? DefaultSpace = null;


            public FontFamily? FontFamily = null;
            public FontStyle? FontStyle = null;
            //public float? FontSize = null;
            public Binding<NumberJs> FontSize { set; protected get; } = null!;
            public BaseModifier(string callerFilePath, int callerLineNumber)
            : base(callerFilePath, callerLineNumber) { }


            public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(ThisFilePathWithNewExtension("js")));
            }


            protected async Task<Tag> GenerateHtmlWithChildrenAsync(Context context, Func<Context,IEnumerable<Task<Tag>>> children, string tagName = "m") {                

                if (FontFamily != null) {
                    context.FontFamily = FontFamily;
                    //tag.Style["font-family"] = "abc";
                }
                if (FontStyle != null) {
                    context.FontStyle = FontStyle;
                }
                /*if (FontSize != null) {
                    context.FontSize = FontSize.Value;
                }*/


                context.Includes.Require(await context.GetCacheableFont());

                AddRequiredInclues(context.Includes);

                var tag = new Tag(tagName) { 
                    CreateScriptBefore(),
                    await Task.WhenAll(children(context)),
                    CreateScriptAfter()
                };

                return tag;
            }
        }
    }
}