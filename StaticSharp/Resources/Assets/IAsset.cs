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
    public Task<string> GetFileExtensionAsync();/* {
        return MimeTypeMap.GetExtension(await GetMediaType());
    }*/
    public Task<string> GetMediaTypeAsync();/* {
        return MimeTypeMap.GetMimeType(await GetFileExtension());
    }*/
    public Task<string> GetContentHashAsync();
    public Task<FilePath> GetTargetFilePathAsync();
    public Task<byte[]> GetBytesAsync();
    public Task<string> GetTextAsync();
}


public abstract class AssetSync: IAsset {
    public Task<string> GetFileExtensionAsync() => Task.FromResult(GetFileExtension());
    public abstract string GetFileExtension();
    public Task<string> GetMediaTypeAsync() => Task.FromResult(GetFileExtension());
    public abstract string GetMediaType();
    public Task<string> GetContentHashAsync() => Task.FromResult(GetContentHash());
    public abstract string GetContentHash();
    public Task<FilePath> GetTargetFilePathAsync() => Task.FromResult(GetTargetFilePath());
    public abstract FilePath GetTargetFilePath();
    public Task<byte[]> GetBytesAsync() => Task.FromResult(GetBytes());
    public abstract byte[] GetBytes();
    public Task<string> GetTextAsync() => Task.FromResult(GetText());
    public abstract string GetText();
}




public class Asset {


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




