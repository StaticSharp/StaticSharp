using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsmlWeb.Html;

namespace CsmlWeb {
    public sealed class MyUnorderedList : BaseList
    {
        //public int _listCount { get; set; }
        public MyUnorderedList() : base("ul") {

        }

        public void Add(string text) {
            //_text.Add(text);
            //_baseList[0]._text.Add(text); в каждый конкретный бейс лист
            //_listCount += 1;
            //_baseList = new List<BaseList>();
            //_baseList.Add(new MyUnorderedList());
            //_baseList[_baseList.Count - 1]._curText = text;
            //_baseList.Add(_baseList[0]._text);
        }

        public void Add(MyUnorderedList item) {
            //TODO
            //_baseList.Add(item);
        }
    }

    public static class MyUnorderedListStatic {
        public static void Add(this IVerifiedBlockReceiver collection, MyUnorderedList item) {
            collection.AddBlock(item);
        }
    }
}