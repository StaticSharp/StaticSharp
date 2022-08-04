using StaticSharpWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[InterpolatedStringHandler]
public static class ParagraphIinterpolatedStringHandlerStatic {
    public static void AppendFormatted(this StaticSharpWeb.ParagraphIinterpolatedStringHandler collection, double item)
        //where Collection : IElementContainer
        {
        //collection.AddElement(item.Representative);
    }
}


namespace StaticSharpWeb {

    public static class ParagraphStatic {

        /*public static void Add<T>(this T collection, Paragraph item) where T : IBlockContainer, ITextAnchorsProvider {
            collection.AddBlock(item);
        }*/

        public static void Add<T>(this T collection, ParagraphIinterpolatedStringHandler item) where T : IElementContainer {
            collection.AddElement(item.Paragraph);
        }

        public static void Add<T>(this T collection, string text) where T : IElementContainer {
            collection.AddElement(new Paragraph() { text });
        }

    }

    public static class Alignment {
        public static readonly int PushLeft = 0;

    }

    [InterpolatedStringHandler]
    public class ParagraphIinterpolatedStringHandler: IElementContainer {
        public Paragraph Paragraph { get; } = new();

        public ParagraphIinterpolatedStringHandler(int literalLength, int formattedCount) {
            //builder = new StringBuilder(literalLength);
            //Console.WriteLine($"\tliteral length: {literalLength}, formattedCount: {formattedCount}");


        }


        public void AppendLiteral(string s) {
            Paragraph.Add(s);
            
        }

        public void AddElement(IElement element) {
            throw new NotImplementedException();
        }

        public void AppendFormatted(IInline item) {
            Paragraph.Add(item);
        }

         
        public void AppendFormatted(string text, int alignment = 0, string? format = null) {

        }

        public void AppendFormatted(StaticSharpEngine.ITypedRepresentativeProvider<IInline> item) {
            Paragraph.Add(item.Representative);
        }

        /*public void AppendFormatted(double item) {
            //Paragraph.Add(item.Representative);
        }*/


        public static implicit operator Paragraph(ParagraphIinterpolatedStringHandler paragraphIinterpolatedStringHandler) {
            return paragraphIinterpolatedStringHandler.Paragraph;
        }
    }


    /*public class SvgRowGenerator {
        Tag Result = new Tag(null);
        public 
    
    }*/


    

    public class Container : IEnumerable {
        public IEnumerator GetEnumerator() {
            throw new NotImplementedException();
        }

        void Add(IElement element) { 
        }
        void Add(string text) { 
        
        }
    }


    public class Modifier : Container { 
        
    
    }


    public class Text : Sequence {
        public Font? Font { get; set; }
        public float? FontSize { get; set; }
        public Color? Color { get; set; }

        


        public void SetTextPropertiesToContext(ref Context context) {
            if (Font!=null) context.Font = Font;
            if (FontSize != null) context.FontSize = (float)FontSize;
        }

        private static void AddDefultSpace(int count, Tag tag) {
            tag.Add(new String(' ', count));
            //tag.Add(new Tag("w"));
        }
        private static void LineToTag(string value, Tag tag, ITextMeasurer textMeasurer) {
            int start = 0;
            int length = 0;
            void AddWord() {
                if (length > 0) {
                    var word = value.Substring(start, length);
                    tag.Add(
                        new Tag("span", new {
                            w = textMeasurer.Measure(word).ToString("0.00", CultureInfo.InvariantCulture),
                        })
                    { word }
                    );
                }
            }

            int spaces = 0;

            for (int i = 0; i < value.Length; i++) {
                var c = value[i];
                if (c == ' ') {
                    AddWord();
                    start = i + 1;
                    length = 0;
                    spaces++;

                } else {
                    if (spaces > 0) {
                        AddDefultSpace(spaces, tag);
                        spaces = 0;
                    }
                    length++;
                }
            }

            AddWord();
        }

        protected static void StringToTag(string value, Tag tag, ITextMeasurer textMeasurer) {
            if (string.IsNullOrEmpty(value))
                return;

            var lines = value.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            LineToTag(lines[0], tag, textMeasurer);

            for (int i = 1; i < lines.Length; i++) {
                tag.Add(new Tag("n"));
                LineToTag(lines[i], tag, textMeasurer);
            }
        }

    }


    public sealed class Paragraph : Text, IEnumerable, IElementContainer, IElement, IPlainTextProvider, IContainerConstraintsNone {

        string TagName { get; set; } = "div";
        
        //protected override string TagName => "p";
        public object? Style { get; set; } = null;

        
        public struct Generators {
            public Func<Context, Task<Tag?>> Html;
            public Func<Context, Task<string>> PlaneText;
        }

        //protected List<object> Items { get; init; } = new();

        private List<object> Items { get; init; } = new();

        public IEnumerator GetEnumerator() => Items.GetEnumerator();



        public static implicit operator Paragraph(string text){
            return new Paragraph() { text };
        }
        /*public static implicit operator Paragraph(ParagraphIinterpolatedStringHandler paragraphIinterpolatedStringHandler) {
            return paragraphIinterpolatedStringHandler.Paragraph;
        }*/
        

        public void Add(string item) {
            Items.Add(item);
         }

        public void Add(IInline item) {
            Items.Add(item);
        }

        public void Add(StaticSharpEngine.ITypedRepresentativeProvider<IInline> item) => Add(item.Representative);


        public void AddElement(IElement element) {
            Items.Add(element);
        }




        /*public void Add(INonVisual item) => Items.Add(new() {
            Html = async context => await item.GenerateHtmlAsync(context),
            PlaneText = context => Task.FromResult("")
        });*/

        /*public async Task<Tag> GenerateHtmlAsync(Context context) {
            var result = new Tag("div", new {
                style = new {
                    MarginTop = $"{context.Theme.ParagraphSpacing}px",
                }
            });


            //var tasks = Items.OfType<IInline>().Select(x => x.GenerateInlineHtmlAsync(context));
            //context.Includes.Require(new Style(AbsolutePath("Paragraph.scss")));


            result.Add(new JSCall(Layout.TextJsPath).Generate(context));
            foreach (var item in await Task.WhenAll(Items.Select(x => x.Html(context)))) {
                result.Add(item);
            }
            return result;
        }*/


        public override IEnumerable<Task<Tag>> Before(Context context) {
            foreach (var i in base.Before(context)) yield return i;
            yield return Task.FromResult(
                new JSCall(Layout.AbsolutePath("Row.js"), new {
                    DefaultSpace = DefaultSpace,
                }, "Before").Generate(context)
                );
        }


        public override async Task<Tag> Content(Context context) {
            //Tag? currentTextTag = null;




            var textMeasurer = await context.Font.CreateOrGetCached().CreateTextMeasurer(context.FontSize);
            

            var result = new Tag(null);
            foreach (var i in Items) {
                if (i is string iString) {
                    /*if (currentTextTag == null) {
                        currentTextTag = new Tag("text",new { 
                            DataA = 9,//Ascent
                            DataD = 2,//Descent
                            DataSw = 5,//SpaceWidth
                        });
                    }*/
                    StringToTag(iString, result, textMeasurer);
                    continue;
                }
            
            }
            /*if (currentTextTag != null) {
                result.Add(currentTextTag);
            }*/
            return result;

                /*return new Tag(null) {
                    await Task.WhenAll(Items.Select(x => x.Html(context)))
                };*/
        }

        /*public override IEnumerable<Task<Tag>> After(Context context) {
            foreach (var i in base.After(context)) yield return i;
            yield return Task.FromResult(
                new JSCall(Layout.AbsolutePath("Row.js"), null, "After").Generate(context)
                );
        }*/

        /*public override async Task<Tag> GenerateHtmlAsync(Context context) {


            var result = await base.GenerateHtmlAsync(context);
            //context.Font.GetFontMetrics() 
            result.AttributesNotNull["f"] = $"10 2 5";
            return result;
        }*/

        public async Task<Tag> GenerateHtmlAsync(Context context) {

            SetTextPropertiesToContext(ref context);

            CacheableFont font = context.Font.CreateOrGetCached();
            var textMeasurer = font.CreateTextMeasurer(context.FontSize);



            return new Tag(TagName) {
                new JSCall(AbsolutePath("Item.js"), null, "Before").Generate(context),
                new JSCall(AbsolutePath("Row.js"), null, "Before").Generate(context),

                await Content(context),

                new JSCall(AbsolutePath("Item.js"), null, "After").Generate(context),
                new JSCall(AbsolutePath("Row.js"), null, "After").Generate(context),
            };
        }




        /*public async Task<INode> GenerateInlineHtmlAsync(Context context) {
            var result = new Tag("span");
            //var tasks = Items.OfType<IInline>().Select(x => x.GenerateInlineHtmlAsync(context));

            foreach (var item in Items) {
                result.Add(await item.Html(context));
            }

            return result;
        }*/

        public async Task<string> GetPlaneTextAsync(Context context) {
            return "";
            //=> string.Concat(await Task.WhenAll(Items.Select(x => x.PlaneText(context))));
        }



        //return string.Concat(await Task.WhenAll(Items.OfType<IPlainTextProvider>()
        //    .Select(x => x.GetPlaneTextAsync(context))));


        /*public void WriteHtml(StringBuilder builder, int indent = 0) {
            throw new System.NotImplementedException();
        }

        public void WritePlaneText(StringBuilder builder) {
            throw new System.NotImplementedException();
        }*/

        /*public void Add(CsmlEngine.ITypedRepresentativeProvider<CsmlEngine.IRepresentative> item) {
           items.Add(item.Representative);
        }*/
    }

    
}