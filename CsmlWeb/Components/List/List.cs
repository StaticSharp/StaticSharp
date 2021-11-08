using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsmlWeb.Html;

namespace CsmlWeb {

    public abstract class BaseList : IBlock
    // TODO uncomment 
    //IEnumerable<BaseList>
    {
        private struct ListContent {
            public string Text;
            public BaseList List;
            
        }

        //public List<BaseList> _baseList = new();
        private List<object> _baseList = new();
        //public List<string> _text = new();
        public string _curText { get; set; }
        private string _type { get; set;}

        public BaseList(string type) {
            _type = type;
        }

        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            var result = new Tag(_type, new { Class = "List" }) {
                new JSCall(new RelativePath("List.js")).Generate(context)
            };
            foreach(var element in _baseList){
                result.Add(element switch {
                    //TODO
                    //BaseList baselist => baselist.Generate(), 
                    string text => new Tag("li") { text }
                    //_ => throw new InvalidUsageException()
                });
            }
            return result;
            // Не нужно ниже
            // Генерирует по порядку, т.к 2 списка (сначала один, потом другой) => объединить в 1
            // var componentName = "List";
            // var tag = new Tag(_type, new { Class = componentName }) {
            //     new JSCall(new RelativePath("List.js")).Generate(context)
            // };
            // context.Includes.RequireStyle(new Style(new RelativePath(componentName + ".scss")));
            // //if (_baseList.Count != 0)
            //  //   Generate(tag, _baseList, context);
            // //Generate2(tag, _text, context);
            // //AllGenerate(tag, this, context);
            // return tag;
        }

        // public void Generate(Tag parentTag, List<BaseList> _baseList, Context context) {
        //     foreach (var i in _baseList) {
        //         if (i._baseList.Count != 0) {
        //             //parentTag.Add(new Tag("div", new { Class = "ListElemAsList"}));
        //             Generate(parentTag, i._baseList, context);
        //         }
        //         parentTag.Add(new Tag("li", new { Class = "UnorderedListElement"}){
        //             new JSCall(new RelativePath("List.js")).Generate(context)
        //         });
        //         if (i._text.Count != 0) {
        //             foreach (var t in i._text) {
        //                 parentTag.Add(new Tag("div", new { Class = "Text" }){
        //                     //i._text[0],
        //                     t,
        //                     //Внутренности _baseList в виде стринг. Проверить если есть внутри что-то
        //                     //Generate2(parentTag, ,context),
        //                     new JSCall(new RelativePath("List.js")).Generate(context)
        //                 }); 
        //             }
        //         }
        //     }
        // } 

        // public void Generate2(Tag parentTag, List<string> text, Context context) {
        //     foreach (var i in text) {
        //         parentTag.Add(new Tag("li", new { Class = "UnorderedListElement"}){
        //             new JSCall(new RelativePath("List.js")).Generate(context)
        //         });
        //         parentTag.Add(new Tag("div", new { Class = "Text" }){
        //             i,
        //             new JSCall(new RelativePath("List.js")).Generate(context)
        //         });
        //     }
        // }

        public void Generate()
        {
            // Реализовать обертку вложенных список по аналогии с Generate выше
        }

        //TODO: uncomment
        // public IEnumerator<object> GetEnumerator()
        // {
        //     return _baseList.GetEnumerator();
        // }

        // IEnumerator IEnumerable.GetEnumerator()
        // {
        //     return _baseList.GetEnumerator();
        // }

    }
    public sealed class UnorderedList : BaseList, IBlock
    {
        //public IEnumerable<ListWithChildren<UnorderedList>> _listElements { get; set; }
        //private static string[] list = new string[] {"List", "Unordered"};

        //private IEnumerable<ListWithChildren<UnorderedList>> list { get; set; }
        public UnorderedList() : base("ul") {

        }

        public async Task<INode> GenerateBlockHtmlAsync(Context context) {
            // return ListWithChildren.GenerateBlockHtmlAsync
            // var tag = new Tag(null);
            // foreach (var i in list) {
            //     tag.Add(await i.GenerateBlockHtmlAsync(context));
            // }
            // return tag;
            return new Tag(null);
        }
        //List<UnorderedList>

        //Как-то чтобы не просто стринги передавались, а еще и листы другие, например Add<T> 53 строка переписать

        // private string[] _text { get; set; }

        // public UnorderedList(string[] text) {
        //     _text = text;
        // }

        // public async Task<INode> GenerateBlockHtmlAsync(Context context)
        // {
        //     var componentName = "List";
        //     var tag = new Tag("ul", new { Class = componentName }) {
        //         new JSCall(new RelativePath("List.js")).Generate(context)
        //     };
        //     context.Includes.RequireStyle(new Style(new RelativePath(componentName + ".scss")));
        //         //_text[0]
        //         //todo return every element not only [0]
        //         //some foreach but it will automaticly add new div tag to li

        //     //Add<T>
        //     Generate(tag, _text, context);
        //     return tag;
        // }

        // public void Generate(Tag parentTag, string[] text, Context context) {
        //     foreach (var i in text) {
        //         parentTag.Add(new Tag("li", new { Class = "UnorderedListElement"}){
        //             new JSCall(new RelativePath("List.js")).Generate(context)
        //         });
        //         parentTag.Add(new Tag("div", new { Class = "Text" }){
        //             i,
        //             new JSCall(new RelativePath("List.js")).Generate(context)
        //         });
        //     }
        // } 
    }  
    public static class UnorderedListStatic {
        public static void Add(this IVerifiedBlockReceiver collection, UnorderedList item) {
            collection.AddBlock(item);
        }
    }
}