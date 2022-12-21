﻿using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Js {
        [Mix(typeof(SvgIcon))]
        [Mix(typeof(Block))]
        public partial class SvgIconBlock {
        }
    }

    [Mix(typeof(SvgIconBindings<Js.SvgIconBlock>))]
    [Mix(typeof(BlockBindings<Js.SvgIconBlock>))]
    [ConstructorJs("SvgIcon")]
    [ConstructorJs]
    public partial class SvgIconBlock : Block {

        Icons.Icon icon;
        protected SvgIconBlock(SvgIconBlock other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) { }
        public SvgIconBlock(Icons.Icon icon, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            this.icon = icon;
        }
        protected override void ModifyHtml(Context context, Tag elementTag) {

            var code = icon.GetSvg();

            elementTag["data-width"] = icon.Width;
            elementTag["data-height"] = icon.Height;
            elementTag.Add(new PureHtmlNode(code));

            base.ModifyHtml(context, elementTag);

        }
    }





}