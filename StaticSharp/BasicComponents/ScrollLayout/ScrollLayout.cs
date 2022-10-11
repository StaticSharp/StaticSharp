using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    [ConstructorJs]
    public class ScrollLayout : Block {
        public Block Content { get; set; } = new();
        public ScrollLayout(ScrollLayout other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
            Content = other.Content;
        }
        public ScrollLayout([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        //public Column Children => this;
        /*public void Add(string? id, IBlock? value) {
            if (value != null) {
                Children.Add(id, value);
            }
        }*/

        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return await Content.GenerateHtmlAsync(context);
        }

    }

}