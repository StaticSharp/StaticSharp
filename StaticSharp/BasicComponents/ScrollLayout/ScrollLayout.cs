using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    namespace Js {
        public interface ScrollLayout : Block {
            public Block Content { get; }
            public double ScrollX  { get; }
            public double ScrollY  { get; }
            public double ScrollXActual  { get; }
            public double ScrollYActual  { get; }
        }
    }

    namespace Gears {
        public class ScrollLayoutBindings<FinalJs> : BlockBindings<FinalJs> {
            public Binding<double> ScrollX { set { Apply(value); } }
            public Binding<double> ScrollY { set { Apply(value); } }
        }
    }

    [Mix(typeof(ScrollLayoutBindings<Js.ScrollLayout>))]
    [ConstructorJs]
    public partial class ScrollLayout : Block {
        public required Block Content { get; set; }
        public ScrollLayout(ScrollLayout other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            Content = other.Content;
        }
        public ScrollLayout([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag) {            
            var content = Content.GenerateHtml(context,new Role(false, "Content"));
            elementTag.Add(content);
            base.ModifyHtml(context, elementTag);
        }

    }

}