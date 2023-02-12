using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp {


    namespace Js {
        public class Link {
            //public string HRef => NotEvaluatableValue<string>();
            public bool NewTab { get; }
        }
    }

    namespace Gears {
        public class LinkBindings<FinalJs> : Bindings<FinalJs> {
            //public Binding<string> HRef { set { Apply(value); } }
            public Binding<bool> NewTab { set { Apply(value); } }
        }

        public class Link {
            
        }
    }

    



    




    /*public partial class NodeLink : Link {

        
        public NodeLink(StaticSharpEngine.INode node, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Node = node;
        }
        public NodeLink(NodeLink other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
            Node = other.Node;
        }

        

        protected override Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag, string? format) {            
            return base.GenerateInlineHtmlInternalAsync(context, elementTag, format);
        }
    }*/



}