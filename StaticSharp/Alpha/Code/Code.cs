using System.Runtime.CompilerServices;

namespace StaticSharp {

    public static partial class Static {

        public static Inline Code(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {

            return new Inline(callerLineNumber, callerFilePath) {
                PaddingsHorizontal = 0.25,
                Radius = 3,
                BackgroundColor = new( e=>Color.Lerp(e.ParentBlock.HierarchyBackgroundColor, e.ParentBlock.HierarchyForegroundColor, 0.1)),
                FontFamilies = { new FontFamilyGenome("Roboto Mono") },
                Children = {
                    new Text(text, true, callerLineNumber, callerFilePath)
                }
            };        
        }
    
    }

}
