using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        public abstract class AbstractModifier: CallerInfo {

            public Color? ForgroundColor = null;
            public Space? DefaultSpace = null;


            public FontFamily? FontFamily = null;
            public FontStyle? FontStyle = null;
            public float? FontSize = null;

            public AbstractModifier(string callerFilePath, int callerLineNumber)
            : base(callerFilePath, callerLineNumber) { }


            protected async Task<(Tag, Context)> GenerateHtmlAndContextAsync(Context context,  string tagName = "m") {

                var tag = new Tag(tagName);

                if (FontFamily != null) {
                    context.FontFamily = FontFamily;
                    tag.Style["font-family"] = "abc";
                }
                if (FontStyle != null) {
                    context.FontStyle = FontStyle;
                }
                if (FontSize != null) {
                    context.FontSize = FontSize.Value;
                }


                context.Includes.Require(await context.GetCacheableFont());


                return (tag,context);
            }
        }
    }

    public sealed class Body : AbstractModifier {

        public Body([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        public async Task<(Tag,Context)> GenerateHtmlAndContextAsync(Context context) {
            return await GenerateHtmlAndContextAsync(context,  "body");
        }
/*        public async Task<Tag> GenerateHtmlAsync(Context context, IEnumerable<IElement> children) {
            //.Select(x=>x.GenerateHtmlAsync(context))
            return await GenerateHtmlAsync(context, children, "body");
        }*/
    }

    public sealed class Modifier : AbstractModifier, IElement, IElementCollector {

        private List<IElement> children { get; } = new();
        public Modifier Children => this;
        public void Add(IElement value) {
            children.Add(value);
        }

        public Modifier([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }



        public async Task<Tag> GenerateHtmlAsync(Context context) {
            (Tag result, context) = await GenerateHtmlAndContextAsync(context);
            foreach (var i in children) {
                result.Add(await i.GenerateHtmlAsync(context));
            }
            return result;
        }

    }
}