using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaticSharpWeb.Html;

namespace StaticSharpWeb {
    public sealed class UnorderedList : BaseList
    {
        public UnorderedList() : base("ul") {

        }
        
        public void Add(object item) {
            _baseList.Add(item);
        }
    }

    public sealed class OrderedList : BaseList
    {
        public OrderedList() : base("ol") {

        }

        public void Add(object item) {
            _baseList.Add(item);
        }
    }  
}