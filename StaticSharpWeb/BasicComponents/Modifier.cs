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
                
                if (FontFamily != null) {
                    context.FontFamily = await FontFamily.CreateOrGetCached();
                }
                Debug.Assert(context.FontFamily != null);

                var fontStyle = FontStyle;
                fontStyle ??= context.Font?.Arguments.FontStyle;
                fontStyle ??= new FontStyle();
                //Debug.Assert(fontStyle != null);

                context.Font = await new Font(context.FontFamily, fontStyle).CreateOrGetCached();
                context.Includes.Require(context.Font);

                context.FontSize = FontSize ?? context.FontSize;
                context.TextMeasurer = context.Font.CreateTextMeasurer(context.FontSize);



                var tag = new Tag(tagName);




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

    public sealed class Modifier : AbstractModifier, IElement, IElementCollector<IElement> {

        private List<IElement> children { get; } = new();
        public Modifier Children => this;
        public void AddElement(IElement value) {
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