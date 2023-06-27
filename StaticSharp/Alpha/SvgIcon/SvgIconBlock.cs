using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Xml;

namespace StaticSharp {

    public interface JSvgIconBlock : JAspectBlockResizableContent, JSvgIcon {
    }

    [RelatedScript("SvgIcon")]
    [ConstructorJs]
    public partial class SvgIconBlock : AspectBlockResizableContent {

        SvgIcons.Icon icon;
        protected SvgIconBlock(SvgIconBlock other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
            icon = other.icon;
        }
        public SvgIconBlock(SvgIcons.Icon icon, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            this.icon = icon;
        }

        /*public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            base.ModifyTagAndScript(context, tag, script);

            var contentId = context.CreateId();

            

            SetNativeSize(script, tag.Id, icon.Width, icon.Height);
            script.Add($"{tag.Id}.content = {TagToJsValue(contentId)}");
        }*/

        public override void CreateContent(Context context, Tag tag, Group scriptBeforeConstructor, Group scriptAfterConstructor, string contentId, out double width, out double height) {
            var code = icon.GetSvg();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(code);
            xml.DocumentElement.SetAttribute("id", contentId);
            code = xml.OuterXml;
            tag.Add(new PureHtmlNode(code));
            width = icon.Width;
            height = icon.Height;
        }
    }





}