using StaticSharpWeb.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class ToDo : IElement {
        public static bool Enabled { get; set; } = false;
        public string Text { get; private set; }
        public bool ShowText { get; private set; }
        public ToDo(string text, bool showText = false, bool suppressWarning = false) {
            Text = text;
            ShowText = showText;
            //if (!suppressWarning) { Log.ToDo.OnObject(this, text); }
        }

        public async Task<Tag> GenerateHtmlAsync(Context context) {
            if (!Enabled) { return new Tag(null); }
            var result = new Tag("span") { Text };
            result.AttributesNotNull.Add("class", nameof(ToDo));
            result.AttributesNotNull.Add("title", Text);
            context.Includes.Require(new Style(AbsolutePath("ToDo.scss")));
            return result;
        }
    }

    public static class ToDoStatic {
        //public static void Add<T>(this T collection, ToDo item) where T : IVe {
        //    collection.AddBlock(item);
        //}
    }
}
