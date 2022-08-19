using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using StaticSharp.Html;

namespace StaticSharp.Gears;

public interface IAsset : IKeyProvider {
    public byte[] ReadAllBites();
    public string ReadAllText();
    public string MediaType { get; }
    public string ContentHash { get; }
    public string FileExtension { get; }
    public string? CharSet { get; }
    public string FilePath => ContentHash + FileExtension;

    public string GetDataUrlBase64() {

        /*var text = ReadAllText();
        return $"data:{MediaType};utf8,{text}";*/

        var base64 = Convert.ToBase64String(ReadAllBites());
        return $"data:{MediaType};base64,{base64}";
    }

    public string GetDataUrlXml() {
        var text = ReadAllText();

        var document = XDocument.Parse(text);

        //var svgElement = document.Elements().FirstOrDefault(x => x.Name.LocalName == "svg");
        
        using (StringWriter stringWriter = new StringWriter()) {
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter)) {
                xmlTextWriter.Formatting = Formatting.None;
                xmlTextWriter.QuoteChar = '\'';
                //writer.Settings.
                document.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
            }
            text = stringWriter.ToString();
        }

        text = text.PercentageEncode(new char[] {'#','%','"'});

        return $"data:{MediaType};utf8,{text}";
    }

}




