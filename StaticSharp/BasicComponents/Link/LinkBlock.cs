using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    /*public partial class LinkBlockWrap : Block { 
    
    
    }*/




    namespace Js {
        [Mix(typeof(Js.Link))]
        [Mix(typeof(Block))]
        public partial class LinkBlock {
        }
    }

    /*[Mix(typeof(LinkBindings<Js.LinkBlock>))]
    [Mix(typeof(BlockBindings<Js.LinkBlock>))]
    [Mix(typeof(Link))]
    [ConstructorJs("Link")]
    [ConstructorJs]
    public partial class LinkBlock : Block {
        protected override string TagName => "a";

        public LinkBlock(string url, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "" ) : base(callerFilePath, callerLineNumber) {
            //Url = url;
        }
        public LinkBlock(Tree.Node node, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerFilePath, callerLineNumber) {
            //Node = node;
        }
        public LinkBlock([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerFilePath, callerLineNumber) {
        }
        public LinkBlock(LinkBlock other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
            //Node = other.Node;
        }

        protected override Task ModifyHtmlAsync(Context context, Tag elementTag) {
            //SetHref(context, elementTag);
            return base.ModifyHtmlAsync(context, elementTag);
        }


    }*/





}