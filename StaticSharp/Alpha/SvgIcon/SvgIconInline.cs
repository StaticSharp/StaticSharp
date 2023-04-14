using AngleSharp.Dom;
using ColorCode.Compilation.Languages;
using NUglify.Html;
using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Xml;

namespace StaticSharp {

    public interface JSvgIconInline : JInline, JSvgIcon {
        //public double BaselineOffset  { get; } 
    }


    [RelatedScript("SvgIcon")]
    [ConstructorJs]
    public partial class SvgIconInline : Inline {

        SvgIcons.Icon icon;

        public double BaselineOffset { get; set; } = 0.14;

        public double Scale { get; set; } = 1;
        public SvgIconInline(SvgIcons.Icon icon, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerLineNumber, callerFilePath) {
            this.icon = icon;
        }

        public SvgIconInline(SvgIconInline other, string callerFilePath, int callerLineNumber) : base(other, callerLineNumber, callerFilePath) {
            icon = other.icon;
            Scale = other.Scale;
        }


        public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            base.ModifyTagAndScript(context, tag, script);

            tag.Style["display"] = "inline-block";

            tag.Style["width"] = $"{Scale * icon.Height / icon.Width}em";
            tag.Style["height"] = $"1em";
            tag.Style["transform"] = $"scale(1,{Scale}) translate(0, {BaselineOffset*100}%)";
            tag.Style["transform-origin"] = "bottom";

            var code = icon.GetSvg();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(code);

            //scale(0.5) translate(-100%, -100%)

            var content = new Tag(xml.DocumentElement.Name);
            XmlAttributeCollection attributes = xml.DocumentElement.Attributes;
            foreach (XmlAttribute a in attributes) {
                content.Attributes.Add(a.Name, a.Value);
            }
            content.Add(new PureHtmlNode(xml.InnerXml));
            content.Style["display"] = "block";
            content.Style["position"] = "relative";
            content.Style["transform"] = $"scale(1, {1/ Scale})";
            content.Style["transform-origin"] = "top";


            tag.Add(content);





            /*script.Add($"{tag.Id}.width = {icon.Width}");
            script.Add($"{tag.Id}.height = {icon.Height}");
            script.Add($"{tag.Id}.scale = {Scale}");*/



        }


    }





}