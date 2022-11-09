using StaticSharp.Tree;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace StaticSharp.Alpha {
    public static partial class Static {
        public static Paragraph? MenuItem(Node node, string? text = null, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            if (node.Representative == null) {
                return null;
            }
            text ??= node.Representative.Title;
            return new Paragraph(text, callerFilePath, callerLineNumber) {
                Margins = 0,
                PaddingsVertical = 10,
                PaddingsHorizontal = 20,
                Children = {
                    {"Link", new LinkBlock(node).FillHeight().FillWidth() },
                    new Block {
                        BackgroundColor = new(e=>e.ParentBlock.ForegroundColor),
                        Visibility = new(e=>e.Parent["Link"].As<Js.Block>().Hover ? 0.10 : 0),
                        Depth = -1
                    }.FillHeight().FillWidth()
                }
            };
        }
    }
}