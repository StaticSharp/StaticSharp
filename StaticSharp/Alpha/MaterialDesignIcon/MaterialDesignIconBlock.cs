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

        MaterialDesignIcons Icon;
        protected override string TagName => "materialDesignIconBlock";
        public MaterialDesignIconBlock(CheckboxBlock other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) { }
        public MaterialDesignIconBlock(MaterialDesignIcons icon, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Icon = icon;
        }

        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            var code = (await new HttpRequestGenome(MaterialDesignIconsStatic.GetSvgUri(Icon)).CreateOrGetCached()).ReadAllText();

            var document = XDocument.Parse(code);
            var svgElement = document.Elements().FirstOrDefault(x => x.Name.LocalName == "svg");
            
            if (svgElement==null)
                throw new Exception("Invalid svg file");

            svgElement.Attribute("id")?.Remove();

            try {
                var viewBox = svgElement.Attribute("viewBox").Value.Split(' ').Select(x => float.Parse(x)).ToArray();
                elementTag["data-width"] = viewBox[2];
                elementTag["data-height"] = viewBox[3];
            }
            catch {
                throw new Exception("Invalid svg file");
            }

            return new Tag(){
                new PureHtmlNode(svgElement.ToString())
            };
        }
    }





}