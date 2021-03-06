using StaticSharpWeb;
using StaticSharpWeb.Components;
using StaticSharpWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

public class Table : IElement, IEnumerable {
    private int _colCount { get; set; }
    private string[] _headers { get; set; }
    //private List<string> _text = new();
    private List<object> _text = new();
    private Table() {

    }
    public Table (int colCount) {
        _colCount = colCount;
    }

    public Table(params string[] headers) {
        if (headers is null || headers.Length < 1) {
            throw new ArgumentException("Headers can not be null or empty");
        }
        _headers = headers;
        _colCount = headers.Length;
    }
    // public void Add(string item) {
    //         _text.Add(item);
    //     }
    public void Add(object item) {
        _text.Add(item);
    }
    public async Task<Tag> GenerateHtmlAsync(Context context)
    {
        if (_text.Count % _colCount != 0) {
            throw new Exception("Размер создаваемой таблицы не соответствует числу предполагаемых элементов таблицы.");
        }
        int textListIndexCounter = 0;
        //var componentName = nameof(Table);
        var tag = new Tag("table", new { Class = "Table", Align = "undefined"}) {
                new JSCall(AbsolutePath("Table.js")).Generate(context)
                };
        context.Includes.Require(new Style(AbsolutePath("Table.scss")));
        if (_headers != null) {
            for (int i = 0; i < _headers.Length; i++) {
                tag.Add(new Tag("th") {_headers[textListIndexCounter]});
                textListIndexCounter++;
            }
        }
        textListIndexCounter = 0;
        for (int i = 0; i < _text.Count / _colCount; i++) {
            var trTag = new Tag("tr");
            for (int j = 0; j < _colCount; j++) {
                var td = new Tag("td");
                if (_text[textListIndexCounter] is string)
                    td.Add(_text[textListIndexCounter].ToString());
                else if (_text[textListIndexCounter] is IElement block) {
                    td.Add(await block.GenerateHtmlAsync(context));
                    td.Attributes.Add("width", "1px");
                }
                if (_headers != null)
                    td.Attributes.Add("style", "text-align:left");
                trTag.Add(td);
                textListIndexCounter++;
            }
            tag.Add(trTag);
        }
        return tag;
    }

    public IEnumerator GetEnumerator()
    {
        return _text.GetEnumerator();
    }
}

    public static class TableStatic {
        public static void Add(this IElementContainer collection, Table item) {
            collection.AddElement(item);
        }
    }