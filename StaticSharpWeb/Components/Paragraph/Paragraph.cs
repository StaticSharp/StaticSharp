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

        public static void Add<T>(this T collection, ParagraphIinterpolatedStringHandler item) where T : IElementContainer, IColumn {
            collection.AddElement(item.Paragraph);
        }

        public static void Add<T>(this T collection, string text) where T : IElementContainer, IColumn {
            collection.AddElement(new Paragraph() { text });
        }

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

    


    public sealed class Paragraph : IEnumerable, IElementContainer, IElement, IPlainTextProvider, IContainerConstraints<IColumn> {


        public object? Style { get; set; } = null;

        public struct Generators {
            public Func<Context, Task<INode>> Html;
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


        public void Add(string item) => Items.Add(new() {
            Html = context => Task.FromResult(new TextNode(item) as INode),
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

        public async Task<INode> GenerateHtmlAsync(Context context) {
            var result = new Tag("div", new {
                style = new {
                    MarginTop = $"{context.Theme.ParagraphSpacing}px",
                }
            });
            /*if (context.Parents.FirstOrDefault(x => x is IFontProvider) is IFontProvider fontProvider) {
                var font = fontProvider?.Font with { Weight = FontWeight.Regular };
                result.Attributes.Add("style", font.GenerateUsageCss(context));
            }*/

            //var tasks = Items.OfType<IInline>().Select(x => x.GenerateInlineHtmlAsync(context));
            //context.Includes.Require(new Style(AbsolutePath("Paragraph.scss")));


            result.Add(new JSCall(Layout.TextJsPath).Generate(context));
            foreach (var item in await Task.WhenAll(Items.Select(x => x.Html(context)))) {
                result.Add(item);
            }
            return result;
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