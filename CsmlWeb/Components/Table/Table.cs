using CsmlWeb;
using CsmlWeb.Components;
using CsmlWeb.Html;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Csml {
    public enum TextAlignment {
        Left,
        Right,
        Center
    }

    public class TableHeader {
        public string Name;
        public TextAlignment Alignment = TextAlignment.Center;
        public class Cell {
            public object Element;
            public int Width;
            public int Height;

            public Cell(object element, int width, int height) {
                Element = element;
                Width = width;
                Height = height;
            }
        }
        public override string ToString() {
            return Name;
        }
    }
    public class TableHeader<T> : TableHeader where T : TableHeader<T> {

        protected T[] Sub;
        public int Num => Math.Max(1, Sub.Sum(x => x.Num));
        public int Depth => (Sub.Length == 0) ? 1 : 1 + Sub.Max(x => x.Depth);
        public IEnumerable<T> Flatten => (Sub.Length == 0) ? (new T[] { (T)this }) : Sub.SelectMany(x => x.Flatten);
        public TableHeader(string name, params T[] sub) {
            Name = name;
            Sub = sub;
        }

        public virtual void AddToMatrix(Cell[,] matrix, int x, int y, int width, int height) {
            matrix[x, y] = new Cell(this, width, height);
        }
    }

    public class Row : TableHeader<Row> {

        public Row(string name, TextAlignment alignment, params Row[] sub) : base(name, sub) {
            Alignment = alignment;
        }
        public Row(string name, params Row[] sub) : base(name, sub) {
            Alignment = TextAlignment.Right;
        }

        public override void AddToMatrix(Cell[,] matrix, int x, int y, int width, int height) {
            base.AddToMatrix(matrix, x, y, width, height);
            var yPosition = y;
            foreach (var s in Sub) {
                var n = s.Num;
                s.AddToMatrix(matrix, x + 1, yPosition, 1, n);
                yPosition += n;
            }
        }
    }



    public class Column : TableHeader<Column> {

        public Column(string name, TextAlignment alignment, params Column[] subColumns) : base(name, subColumns) {
            Alignment = alignment;
        }
        public Column(string name, params Column[] subColumns) : base(name, subColumns) {
            Alignment = TextAlignment.Center;
        }

        public override void AddToMatrix(Cell[,] matrix, int x, int y, int width, int height) {
            base.AddToMatrix(matrix, x, y, width, height);
            var xPosition = x;
            foreach (var s in Sub) {
                var n = s.Num;
                s.AddToMatrix(matrix, xPosition, y + 1, n, 1);
                xPosition += n;
            }
        }
    }


    public class Table : CsmlWeb.Html.Collection<Table> {
        TableHeader[] Headers = new TableHeader[0];
        public int UserDefinedNumColumns;
        public float RowHeaderWidth = 0.1f;
        private readonly TextAlign Align;

        public Table(params string[] headers) {
            Headers = headers.Select(x => new Column(x)).ToArray();
        }
        public Table(params TableHeader[] headers) {
            Headers = headers;
        }
        public Table(int numColumns, TextAlign align = TextAlign.Undefined) {
            UserDefinedNumColumns = numColumns;
            Align = align;
        }


        public Table SetRowHeaderWidth(float width) {
            RowHeaderWidth = width;
            return this;
        }


        private INode GenerateTable(Context context) {

            var rows = Headers.OfType<Row>();
            var columns = Headers.OfType<Column>();
            var alignments = columns.SelectMany(x => x.Flatten).Select(x => x.Alignment).ToArray();

            var numRows = rows.Sum(x => x.Num);
            var numColumns = columns.Sum(x => x.Num);

            var depthRows = (numRows > 0) ? rows.Max(x => x.Depth) : 0;
            var depthColumns = (numColumns > 0) ? columns.Max(x => x.Depth) : 0;
            var numElements = _innerList.Count();



            if ((numColumns == 0) & (numRows > 0)) {
                numColumns = numElements / numRows + (((numElements % numRows) > 0) ? 1 : 0);
            }

            if ((numColumns == 0) & (numRows == 0)) {
                numColumns = Math.Max(1, UserDefinedNumColumns);
                alignments = Enumerable.Repeat(TextAlignment.Center, numColumns).ToArray();
            }

            if ((numColumns > 0) & (numRows == 0)) {
                numRows = numElements / numColumns + (((numElements % numColumns) > 0) ? 1 : 0);
            }

            TableHeader.Cell[,] matrix = new TableHeader.Cell[depthRows + numColumns, depthColumns + numRows];
            var xPosition = depthRows;
            foreach (var c in columns) {
                var depth = c.Depth;
                var num = c.Num;
                c.AddToMatrix(matrix, xPosition, 0, num, depthColumns - depth + 1);
                xPosition += num;
            }
            var yPosition = depthColumns;
            foreach (var r in rows) {
                var depth = r.Depth;
                var num = r.Num;
                r.AddToMatrix(matrix, 0, yPosition, depthRows - depth + 1, num);
                yPosition += num;
            }

            var elements = _innerList.ToArray();
            for (int i = 0; i < elements.Length; i++) {
                var x = i % numColumns + depthRows;
                var y = i / numColumns + depthColumns;
                matrix[x, y] = new TableHeader.Cell(elements[i], 1, 1);
            }

            return null;

            //return new Tag("table").Attribute("align", Align.ToCss())
            //    .AddTag("tbody", a => {
            //        for (int r = 0; r < matrix.GetLength(1); r++) {
            //            a.AddTag("tr", tr => {
            //                if ((r == 0) & ((depthRows * depthColumns) > 0)) {
            //                    tr.AddTag("td", td => {
            //                        td.Attribute("rowspan", depthColumns.ToString());
            //                        td.Attribute("colspan", depthRows.ToString());
            //                    });
            //                }

            //                for (int c = 0; c < matrix.GetLength(0); c++) {
            //                    var cell = matrix[c, r];
            //                    if (cell != null) {
            //                        Tag node;
            //                        if (cell.Element is TableHeader) {
            //                            node = new Tag("th")
            //                                .Attribute("style", $"text-align:{(cell.Element as TableHeader).Alignment.ToString().ToLower()}")
            //                                .AddText((cell.Element as TableHeader).Name);
            //                        } else {
            //                            node = new Tag("td")
            //                                .Attribute("style", $"text-align:{alignments[c - depthRows].ToString().ToLower()}")
            //                                .If(Align != TextAlign.Undefined, x =>
            //                                    x.Attribute("style", $"text-align:{Align.ToCss()}"))
            //                                .Add((cell.Element as IElement).Generate(context));
            //                        }
            //                        if (cell.Width != 1) node.Attribute("colspan", cell.Width.ToString());
            //                        if (cell.Height != 1) node.Attribute("rowspan", cell.Height.ToString());
            //                        tr.Add(node);
            //                    }
            //                }
            //            });
            //        }
            //    });
        }


        //public override INode Generate(Context context) {
        //    return GenerateTable(context);
        //}
    }


}