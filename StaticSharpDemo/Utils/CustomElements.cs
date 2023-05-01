using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Utils {
    public static class CustomElements {
        public static Block Separator(
                [CallerLineNumber] int callerLineNumber = 0,
                [CallerFilePath] string callerFilePath = ""
                ) {
            return new LayoutOverride {
                Child = new Block(callerLineNumber, callerFilePath) {
                    MarginsVertical = 75,
                    Height = new(x => 1 / Js.Window.DevicePixelRatio),
                    BackgroundColor = new(e => e.Parent.HierarchyForegroundColor),
                    Visibility = 0.5
                },
                OverrideX = new(e => Js.Num.First(e.MarginLeft, 0)),
                OverrideWidth = new(e => Js.Num.Sum(e.Parent.Width, -e.Child.GetLayer().MarginLeft, -e.Child.GetLayer().MarginRight))
            };
        }

        public static Block CodeBlockFromThisFileRegion(
            string regionName,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") {

            return CodeBlockScrollable(LoadFile(callerFilePath).GetCodeRegion(regionName).Highlight(new CSharpHighlighter()), callerLineNumber, callerFilePath);
        }


    }

}