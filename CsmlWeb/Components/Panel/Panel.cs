using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsmlWeb.Html;

namespace CsmlWeb {
    public class Panel : IBlock {

        public string _content;
        //public List<string> _privateList = new();
        //private string[] _classes;
        // public Panel(string content) {
        //     _content = content;
        // }


        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            var tag = new Tag("div", new { Class = nameof(Panel) }) {
                //new Tag("panel", new { style = "width: 100%; height: auto;"}),
                new JSCall(new RelativePath("Panel.js")).Generate(context)
            };
            var componentName = nameof(Panel);
            context.Includes.RequireStyle(new Style(new RelativePath(componentName + ".scss")));
            tag.Add(new JSCall(new RelativePath(componentName + ".js")).Generate(context));
            return tag;
        }


        //public IEnumerator GetEnumerator() => _privateList.GetEnumerator();
    }

    public class Info : Panel
    {
        //public Info(string content, params string[] classes) : base(content, classes) { }
        //public Info(string content) : base(_content) { }
    }

    public static class PanelStatic {

        // public static void Add(this Panel collection, string value){
        //     //collection.AddBlock(value);
        //     collection._privateList.Add(value);
        // }

        // public static void AddString(this Panel _this, string value){
        // _this._content += value;
        // }

        public static void Add(this IVerifiedBlockReceiver collection, Panel item) {
            collection.AddBlock(item);
        }
    }   
}
