using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using MimeTypes;
using StaticSharp.Html;

namespace StaticSharp.Gears;



public interface IAsset {
    public string Extension { get; }
    public string ContentHash { get; }

    public byte[] Data { get; }
    public string Text { get; }
}

public interface IAssetAsyncData {
    public Task<byte[]> GetDataAsync();

    public Task<string> GetContentHashAsync();
}




public static class AssetExtension {

    public static FilePath GetTargetFilePath(this IAsset asset) {
        return new(asset.ContentHash + asset.Extension);
    }

    public static string GetMediaType(this IAsset asset) {
        var extension = asset.Extension;
        var mediaType = MimeTypeMap.GetMimeType(extension);
        return mediaType;
    }


    public static string GetDataUrlBase64(this IAsset asset) {

        /*var text = ReadAllText();
        return $"data:{MediaType};utf8,{text}";*/

        var base64 = Convert.ToBase64String(asset.Data);
        return $"data:{asset.GetMediaType()};base64,{base64}";
    }

    public static string GetDataUrlXml(this IAsset asset) {
        var text = asset.Text;
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

        text = text.PercentageEncode(new char[] { '#', '%', '"' });

        return $"data:{asset.GetMediaType()};utf8,{text}";
    }
}



/*public class Asset {


    public string FileExtension { get; init; }
    public string MediaType { get; init; }

    private string? contentHash = null;

    public string ContentHash {
        get {
            if (contentHash == null) {
                contentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();
            }
            return contentHash;
        }    
    } 
    
    public string? CharSet { get; init; }
    
    private byte[]? content = null;

    public Func<byte[]> ContentCreator { get; init; }

    public Func<Task<bool>>? ContentValidator { get; set; }

    public Asset(Func<byte[]> contentCreator, string fileExtension, string mediaType, string? contentHash = null, string? charSet = null) {
        FileExtension = fileExtension;
        MediaType = mediaType;
        this.contentHash = contentHash;        
        CharSet = charSet;
        ContentCreator = contentCreator;
    }

    public virtual byte[] ReadAllBites() {
        if (content == null) {
            content = ContentCreator();
        }
        return content;
    }
    public string ReadAllText() {
        var data = ReadAllBites();
        return FileUtils.ReadAllText(data, CharSet);
    }

    public FilePath FilePath => new(ContentHash + FileExtension);   

}*/




