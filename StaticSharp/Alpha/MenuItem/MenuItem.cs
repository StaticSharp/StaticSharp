using StaticSharp.Tree;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace StaticSharp.Alpha {
    public static partial class Static {



        public static Block? MenuItem(Node node, string? text = null, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            if (node.Representative == null) {
                return null;
            }
            text ??= node.Representative.Title;

            return new Paragraph(text,callerLineNumber, callerFilePath) {
                InternalLink = node,
                Margins = 0,
                PaddingsVertical = 10,
                PaddingsHorizontal = 20,

                Children = {
                    new LayoutOverride{
                        Content = new Block {
                            BackgroundColor = new(e=>e.Parent.HierarchyForegroundColor),
                            Visibility = new(e=>e.Parent.Parent.Hover ? 0.10 : 0),
                            Depth = -1
                        },

                        OverrideX = new(e => Js.Math.First(e.MarginLeft, 0)),
                        OverrideY = new(e => Js.Math.First(e.MarginTop, 0)),
                        OverrideWidth = new(e => Js.Math.Sum(e.Parent.Width, -e.Content.GetLayer().MarginLeft, -e.Content.GetLayer().MarginRight)),
                        OverrideHeight = new(e => Js.Math.Sum(e.Parent.Height, -e.Content.GetLayer().MarginTop, -e.Content.GetLayer().MarginBottom))
                    }
                }
            };
        }
    }
}