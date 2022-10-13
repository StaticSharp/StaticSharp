using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [ConstructorJs]
    
    public class Paragraph : Block {
        protected override string TagName => "paragraph";
        //protected List<KeyValuePair<string?, IInline>> children { get; } = new();
        public Inlines Inlines { get; } = new();


        /*public static implicit operator Paragraph(string text) {
            string callerFilePath = "";
            int callerLineNumber = 0;

            var paragraph = new Paragraph(callerFilePath, callerLineNumber);
            paragraph.AppendLiteral(text, callerFilePath, callerLineNumber);
            return paragraph;
        }*/
        public Paragraph(Paragraph other,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(other, callerFilePath, callerLineNumber) {

            Inlines = new(other.Inlines);
        }


        public Paragraph(string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Inlines.AppendLiteral(text, callerFilePath, callerLineNumber);
        }

        public Paragraph(Inlines inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Inlines = new(inlines);
        }
        public Paragraph(Inline inline,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Inlines = new() { inline };
        }


        /*public void Add(IInline? value) {
            if (value != null) {
                children.Add(value);
            }
        }

        public void Add(string value,
            [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(new Text(value, true, callerFilePath, callerLineNumber));
        }*/







        /*public void Add(string? id, IInline? value) {
            if (value != null) {
                children.Add(new KeyValuePair<string?, IInline>(id, value));
            }
        }*/

        struct C {
            public List<int> l = new();
            public Ref<int> i = new(0);
            public C() {
            }
        }


        protected override async ValueTask ModifyHtmlAsync(Context context, Tag elementTag) {

            var p = new Tag("p");
            foreach (var i in Inlines) {
                p.Add(await i.Value.GenerateInlineHtmlAsync(context, i.Id, i.Format));
            }
            elementTag.Add(p);

        }


        
    }
}
