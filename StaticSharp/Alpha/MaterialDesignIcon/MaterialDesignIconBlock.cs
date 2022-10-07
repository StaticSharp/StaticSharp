using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StaticSharp {
    [Mix(typeof(MaterialDesignIconBindings<MaterialDesignIconBlockJs>))]
    [Mix(typeof(BlockBindings<MaterialDesignIconBlockJs>))]

    //[ConstructorJs("Checkbox")]
    
    [ConstructorJs]
    public partial class MaterialDesignIconBlock : Block {

        MaterialDesignIcons.IconName Icon;
        protected override string TagName => "materialDesignIconBlock";
        public MaterialDesignIconBlock(CheckboxBlock other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) { }
        public MaterialDesignIconBlock(MaterialDesignIcons.IconName icon, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Icon = icon;
        }

        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            var code = MaterialDesignIcon.GetSvgTag(Icon, out var width, out var height);

            elementTag["data-width"] = width;
            elementTag["data-height"] = height;

            return new Tag(){
                new PureHtmlNode(code)
            };
        }
    }





}