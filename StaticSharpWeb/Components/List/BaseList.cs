using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaticSharpWeb.Html;

namespace StaticSharpWeb {

    public abstract class BaseList : IBlock, IEnumerable<object>
    {
        private struct ListContent {
            public string Text;
            public BaseList List;
        }
        public List<object> _baseList = new();
        private string _type { get; set;}

        public BaseList(string type) {
            _type = type;
        }

        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            var result = new Tag(_type, new { Class = "List" }) {
                new JSCall(new AbsolutePath("List.js")).Generate(context)
            };
            GenerateInnerTags(context, result, _baseList);
            return result;
        }

        public void GenerateInnerTags(Context context, Tag parentTag, List<object> _curBaseList) {
            foreach (var elem in _curBaseList) {
                if (elem is string) {
                    parentTag.Add(new Tag("li") { 
                        elem.ToString(),
                        //new JSCall(new RelativePath("List.js")).Generate(context)
                        });
                    
                }
                if (elem is BaseList curTag) {
                    var tag = new Tag(_type, new { Class = "List"}) {
                        //new JSCall(new RelativePath("List.js")).Generate(context)
                    }; 
                    GenerateInnerTags(context, tag, curTag._baseList);
                    parentTag.Add(tag);
                }
            }
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _baseList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _baseList.GetEnumerator();
        }
    }
    
    public static class BaseListStatic {
        public static void Add(this IBlockContainer collection, BaseList item) {
            collection.AddBlock(item);
        }
    }
}