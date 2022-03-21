using StaticSharpWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
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

    


    public sealed class Paragraph : Item, IEnumerable, IElementContainer, IElement, IPlainTextProvider, IContainerConstraintsNone {

        protected override string TagName => "p";
        public object? Style { get; set; } = null;

        public struct Generators {
            public Func<Context, Task<Tag?>> Html;
            public Func<Context, Task<string>> PlaneText;
        }

        //protected List<object> Items { get; init; } = new();

        private List<Generators> Items { get; init; } = new();

        public IEnumerator GetEnumerator() => Items.GetEnumerator();



        public static implicit operator Paragraph(string text){
            return new Paragraph() { text };
        }
        /*public static implicit operator Paragraph(ParagraphIinterpolatedStringHandler paragraphIinterpolatedStringHandler) {
            return paragraphIinterpolatedStringHandler.Paragraph;
        }*/

        private void LineToTag(string value, Tag tag) {
            //tag.Add(value);
            var words = value.Split(new[] { " " }, StringSplitOptions.None);
            foreach (var i in words)
                tag.Add(new Tag("w") { i });
        }

        private Tag? StringToTag(string value) {
            if (string.IsNullOrEmpty(value))
                return null;
            var lines = value.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var result = new Tag(null);
            LineToTag(lines[0], result);

            for (int i = 1; i < lines.Length; i++) {
                result.Add(new Tag("n"));
                LineToTag(lines[i], result);
            }
            return result;
        }

        public void Add(string item) => Items.Add(new() {
            Html = context => Task.FromResult(StringToTag(item)),
            PlaneText = context => Task.FromResult(item)
        });

        public void Add(IInline item) => Items.Add(new() {
            Html = async context => await item.GenerateHtmlAsync(context),
            PlaneText = async context => item is IPlainTextProvider plainTextProvider
                ? await plainTextProvider.GetPlaneTextAsync(context)
                : string.Empty
        });

        public void Add(StaticSharpEngine.ITypedRepresentativeProvider<IInline> item) => Add(item.Representative);


        public void AddElement(IElement element) => Items.Add(new() {
            Html = async context => await element.GenerateHtmlAsync(context),
            PlaneText = async context => element is IPlainTextProvider plainTextProvider
                ? await plainTextProvider.GetPlaneTextAsync(context)
                : string.Empty
        });


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
                new JSCall(Layout.TextJsPath, null, "Before").Generate(context)
                );
        }


        public override async Task<Tag> Content(Context context) {
            return new Tag(null) {
                await Task.WhenAll(Items.Select(x => x.Html(context)))
            };
        }

        public override IEnumerable<Task<Tag>> After(Context context) {
            foreach (var i in base.After(context)) yield return i;
            yield return Task.FromResult(
                new JSCall(Layout.TextJsPath, null, "After").Generate(context)
                );
        }


        public async Task<INode> GenerateInlineHtmlAsync(Context context) {
            var result = new Tag("span");
            //var tasks = Items.OfType<IInline>().Select(x => x.GenerateInlineHtmlAsync(context));

            foreach (var item in Items) {
                result.Add(await item.Html(context));
            }

            return result;
        }

        public async Task<string> GetPlaneTextAsync(Context context)
             => string.Concat(await Task.WhenAll(Items.Select(x => x.PlaneText(context))));

        
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