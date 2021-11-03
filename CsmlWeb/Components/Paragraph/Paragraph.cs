using CsmlWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CsmlWeb {

    public static class ParagraphStatic {

        public static void Add<T>(this T collection, Paragraph item) where T : IVerifiedBlockReceiver, ITextAnchorsProvider {
            collection.AddBlock(item);
        }

        public static void Add<T>(this T collection, ParagraphIinterpolatedStringHandler item) where T : IVerifiedBlockReceiver, ITextAnchorsProvider {
            collection.AddBlock(item.Paragraph);
        }
    }

    [InterpolatedStringHandler]
    public class ParagraphIinterpolatedStringHandler {
        public Paragraph Paragraph { get; } = new();
        public void AppendLiteral(string s) {
            Paragraph.Add(s);
        }

        public void AppendFormatted(IInline item) {
            Paragraph.Add(item);
        }

        public void AppendFormatted(CsmlEngine.ITypedRepresentativeProvider<IInline> item) {
            Paragraph.Add(item);
        }
    }

    public sealed class Paragraph : IEnumerable, IInline, IBlock, IPlainTextProvider {

        public struct Generators {
            public Func<Context, Task<INode>> Html;
            public Func<Context, Task<string>> PlaneText;
        }

        //protected List<object> Items { get; init; } = new();

        private List<Generators> Items { get; init; } = new();

        public IEnumerator GetEnumerator() => Items.GetEnumerator();

        public void Add(string item) => Items.Add(new() {
            Html = context => Task.FromResult(new TextNode(item) as INode),
            PlaneText = context => Task.FromResult(item)
        });

        public void Add(IInline item) => Items.Add(new() {
            Html = async context => await item.GenerateInlineHtmlAsync(context),
            PlaneText = async context => item is IPlainTextProvider plainTextProvider
                ? await plainTextProvider.GetPlaneTextAsync(context)
                : string.Empty
        });

        public void Add(CsmlEngine.ITypedRepresentativeProvider<IInline> item) => Add(item.Representative);

        /*public void Add(INonVisual item) => Items.Add(new() {
            Html = async context => await item.GenerateHtmlAsync(context),
            PlaneText = context => Task.FromResult("")
        });*/

        public async Task<INode> GenerateBlockHtmlAsync(Context context) {
            var result = new Tag("div", new { Class = nameof(Paragraph) });
            if (context.Parents.FirstOrDefault(x => x is IFontProvider) is IFontProvider fontProvider) {
                var font = fontProvider?.Font with { Weight = FontWeight.Regular };
                result.Attributes.Add("style", font.GenerateUsageCss(context));
            }

            //var tasks = Items.OfType<IInline>().Select(x => x.GenerateInlineHtmlAsync(context));
            context.Includes.RequireStyle(new Style(new RelativePath("Paragraph.scss")));
            result.Add(new JSCall(new RelativePath("Paragraph.js")).Generate(context));
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