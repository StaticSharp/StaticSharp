using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    public enum TextAlignmentHorizontal {
        Left,
        Center,
        Right,
        Justify,
        JustifyIncludingLastLine
    }


    namespace Js {
        public interface Paragraph : Block {
            public TextAlignmentHorizontal TextAlignmentHorizontal { get; }

        }
    }

    namespace Gears {
        public class ParagraphBindings<FinalJs> : BlockBindings<FinalJs> {
            public Binding<TextAlignmentHorizontal> TextAlignmentHorizontal { set { Apply(value); } }
        }
    }


    
    [ConstructorJs]
    [Mix(typeof(ParagraphBindings<Js.Paragraph>))]
    public abstract partial class ParagraphBase : Block {

        protected abstract Inlines GetInlines();
        protected ParagraphBase(ParagraphBase other,
                int callerLineNumber = 0,
                string callerFilePath = ""
                ) : base(other, callerLineNumber, callerFilePath) {}
        public ParagraphBase([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }


        protected override void ModifyHtml(Context context, Tag elementTag) {

            var p = new Tag("p");
            foreach (var i in GetInlines()) {
                var child = i.Value.GenerateHtml(context, new Role(true, i.Key));
                p.Add(child);
            }
            p.Add("\n");
            elementTag.Add(p);

            base.ModifyHtml(context, elementTag);
        }

    }
}
