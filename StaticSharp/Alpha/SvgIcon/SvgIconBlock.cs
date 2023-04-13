using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Xml;

namespace StaticSharp {

    public interface JSvgIconBlock : JAspectBlock, JSvgIcon {
    }

    [Mix(typeof(SvgIconBindings<JSvgIconBlock>))]
    [Mix(typeof(BlockBindings<JSvgIconBlock>))]
    [ConstructorJs("SvgIcon")]
    [ConstructorJs]
    public partial class SvgIconBlock : AspectBlock {

        SvgIcons.Icon icon;
        protected SvgIconBlock(SvgIconBlock other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
            icon = other.icon;
        }
        public SvgIconBlock(SvgIcons.Icon icon, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            this.icon = icon;
        }

        public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            base.ModifyTagAndScript(context, tag, script);

            var contentId = context.CreateId();

            var code = icon.GetSvg();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(code);
            xml.DocumentElement.SetAttribute("id", contentId);
            code = xml.OuterXml;
            tag.Add(new PureHtmlNode(code));

            SetNativeSize(script, tag.Id, icon.Width, icon.Height);
            script.Add($"{tag.Id}.content = {TagToJsValue(contentId)}");
        }

    }





}