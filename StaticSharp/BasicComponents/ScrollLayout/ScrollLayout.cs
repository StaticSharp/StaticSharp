using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    public class Scrollable : Hierarchical {
        public Scrollable([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

    }


    //[RelatedStyle]
    [ConstructorJs]
    public class ScrollLayout : Block {

        public Block Content { get; set; } = new();
        public ScrollLayout(ScrollLayout other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
            Content = other.Content;
        }
        public ScrollLayout([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {            
            var content = await Content.GenerateHtmlAsync(context);
            content.AssignParentProperty("Content");
            elementTag.Add(content);
            await base.ModifyHtmlAsync(context, elementTag);
        }

    }

}