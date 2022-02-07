using StaticSharpWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb.Components {
    public class Grid : IEnumerable, IBlock, IVerifiedBlockReceiver {

        int MinElementWidthPx { get; }
        int MinColumns { get; }
        private Align AlignType { get; }
        private int Gap { get; }

        private readonly List<Generators> Items = new();

        public struct Generators {
            public Func<Context, Task<INode>> Html;
            public Func<Context, Task<string>> PlaneText;
        }

        public enum Align {
            Stretch,
            Center,
            Bottom,
            Top
        }

        public Grid(int minElementWidthPx, int minColumns = 1, Align align = default) {
            MinElementWidthPx = minElementWidthPx;
            MinColumns = minColumns;
            AlignType = align;
            Gap = 4;//gap;
        }



        public async Task<INode> GenerateBlockHtmlAsync(Context context) {
            var css = //$"grid-template-columns: repeat(auto-fit, minmax({MinElementWidthPx}, 1fr)); " +
                $"grid-template-columns: repeat(auto-fit, minmax( min({MinElementWidthPx}px, {100 / MinColumns}% - {Gap * (MinColumns - 1) / MinColumns}px), 1fr)); " +
                //$"grid-template-columns: repeat(3, 1fr); " + 
                $"align-items: {CssAlign};grid-gap: {Gap}px;";
            //context.EstimatedWidth = MinElementWidthPx;
            context.Includes.Require(new Style(new AbsolutePath(nameof(Grid) + ".scss")));
            var result = new Tag("div", new { style = css, Class = "grid" });
            foreach (var item in await Task.WhenAll(Items.Select(x => x.Html(context)))) {
                result.Add(item);
            }
            result.Add(new JSCall(new AbsolutePath(nameof(Grid) + ".js")).Generate(context));
            return result;
        }

        public void AddBlock(IBlock block) => Items.Add(new() {
            Html = async context => await block.GenerateBlockHtmlAsync(context),
        });

        

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

        public void Add(StaticSharpEngine.ITypedRepresentativeProvider<IInline> item) => Add(item.Representative);


        //public void Add(IBlock item) => Items.Add(new() {
        //    Html = async context => await item.GenerateBlockHtmlAsync(context),
        //    PlaneText = async context => item is IWideAnchorsProvider wideAnchorsProvider
        //        ? await wideAnchorsProvider.
        //        : string.Empty
        //});



        public IEnumerator GetEnumerator() => Items.GetEnumerator();



        private string CssAlign => AlignType switch {
            Align.Center => "center",
            Align.Top => "start",
            Align.Bottom => "end",
            _ => "stretch"
        };
    }

    public static class GridStatic {
        public static void Add<T>(this T collection, Grid item) where T : IVerifiedBlockReceiver {
            collection.AddBlock(item);
        }
    }
}
