using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StaticSharp {

    namespace Js {
        [Mix(typeof(MaterialDesignIcon))]
        [Mix(typeof(Block))]
        public partial class MaterialDesignIconBlock {
        }
    }

    [Mix(typeof(MaterialDesignIconBindings<Js.MaterialDesignIconBlock>))]
    [Mix(typeof(BlockBindings<Js.MaterialDesignIconBlock>))]
    [ConstructorJs("MaterialDesignIcon")]
    [ConstructorJs]
    public partial class MaterialDesignIconBlock : Block {

        MaterialDesignIcons.IconName Icon;
        protected MaterialDesignIconBlock(MaterialDesignIconBlock other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) { }
        public MaterialDesignIconBlock(MaterialDesignIcons.IconName icon, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Icon = icon;
        }
        protected override void ModifyHtml(Context context, Tag elementTag) {

            var code = MaterialDesignIcon.GetSvgTag(Icon, out var width, out var height);

            elementTag["data-width"] = width;
            elementTag["data-height"] = height;
            elementTag.Add(new PureHtmlNode(code));

            base.ModifyHtml(context, elementTag);

        }
    }





}