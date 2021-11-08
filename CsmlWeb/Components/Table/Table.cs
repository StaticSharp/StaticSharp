using CsmlWeb;
using CsmlWeb.Components;
using CsmlWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Table : IBlock, IEnumerable {
    private int _colCount { get; set; }
    private string[] _headers { get; set; }
    //private int _rowCount { get; set; }
    private List<string> _text = new();
    public Table (int colCount) {
        _colCount = colCount;
        //_rowCount = rowCount;
    }

    public Table(params string[] headers) {
        _headers = headers;
        _colCount = headers.Length;
    }
    public void Add(string item) {
            _text.Add(item);
        }
    public async Task<INode> GenerateBlockHtmlAsync(Context context)
    {
        if (_text.Count % _colCount != 0) {
            throw new Exception("Размер создаваемой таблицы не соответствует числу предполагаемых элементов таблицы.");
        }
        int textListIndexCounter = 0;
        //var componentName = nameof(Table);
        var tag = new Tag("table", new { Class = "Table", Align = "undefined"}) {
                new JSCall(new RelativePath("Table.js")).Generate(context)
                };
        context.Includes.RequireStyle(new Style(new RelativePath("Table.scss")));
        if (_headers != null) {
            for (int i = 0; i < _headers.Length; i++) {
                tag.Add(new Tag("th", new { style = "text-align:center" }) {_headers[textListIndexCounter]});
                textListIndexCounter++;
            }
        }
        textListIndexCounter = 0;
        for (int i = 0; i < _text.Count / _colCount; i++) {
            var trTag = new Tag("tr");
            for (int j = 0; j < _colCount; j++) {
                trTag.Add(new Tag("td", new { style = "text-align:center"}) {_text[textListIndexCounter]});
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
        public static void Add(this IVerifiedBlockReceiver collection, Table item) {
            collection.AddBlock(item);
        }
    }