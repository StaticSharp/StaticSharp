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
        public Inlines Children { get; } = new();


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

            Children = new(other.Children);
        }



        public Paragraph(string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Children.AppendLiteral(text, callerFilePath, callerLineNumber);
        }

        public Paragraph(Inlines inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Children = new(inlines);
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


        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {



            /*var c = context;
            c.GetUniqueId();

            var d = c;
            d.GetUniqueId();*/


            var result = new Tag("p");
            foreach (var i in Children) {
                result.Add(await i.Value.GenerateInlineHtmlAsync(context, i.Id, i.Format));
            }
            return result;
        }

        /*async Task<Tag> IInline.GenerateInlineHtmlAsync(Context context, string? id) {
            return new Tag() {
                await Task.WhenAll(children.Select(x=>x.Value.GenerateInlineHtmlAsync(context,x.Key)))
            };
        }*/

        
    }
}
