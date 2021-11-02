using System.Collections.Generic;
using System.Threading.Tasks;
using CsmlWeb.Html;

namespace CsmlWeb {
    public class UnorderedList : List<UnorderedList>, IBlock
    {
        // private string _tag { get; set; }
        // public string[] _classes { get; set; }
        // public string _childTag { get; set; }
        // public UnorderedList(string tag, string[] classes, string childTag) {
        //     _tag = tag;
        //     _classes = classes;
        //     _childTag = childTag;
        // }

        private string[] _text { get; set; }

        public UnorderedList(string[] text) {
            _text = text;
        }
        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            var componentName = "List";
            var tag = new Tag("ul", new { Class = componentName }) {
                new JSCall(new RelativePath(componentName + ".js")).Generate(context)
            };
            context.Includes.RequireStyle(new Style(new RelativePath(componentName + ".scss")));
            //tag.Add(new Tag("li", new { Class = "UnorderedListElement"}));
            //tag.Add(new Tag("div", new { Class = "Text"}) {
                //_text[0]
                //todo return every element not only [0]
                //some foreach but it will automaticly add new div tag to li
            //});

            //Add<T>
            Generate(tag, _text);
            return tag;
        }

        public void Generate(Tag parentTag, string[] text) {
            foreach (var i in text) {
                parentTag.Add(new Tag("li", new { Class = "UnorderedListElement"}));
                parentTag.Add(new Tag("div", new { Class = "Text" }){
                    i
                });
            }
        } 
    }  
    public static class UnorderedListStatic {
        public static void Add(this IVerifiedBlockReceiver collection, UnorderedList item) {
            collection.AddBlock(item);
        }

        // private string Generate(string[] classes) {
            
        // }

        // private void Generate(Tag parent, INode elements, Context context) {
        //     foreach (var i in elements) {
 
        //     }
        // }
    }
}