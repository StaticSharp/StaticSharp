using System;

namespace StaticSharp.Gears;

public interface IAsset : IKeyProvider {
    public byte[] ReadAllBites();
    public string ReadAllText();
    public string MediaType { get; }
    public string ContentHash { get; }
    public string FileExtension { get; }
    public string? CharSet { get; }
    public string FilePath => ContentHash + FileExtension;

    public string GetDataUrl() {

        /*var text = ReadAllText();
        return $"data:{MediaType};utf8,{text}";*/

        var base64 = Convert.ToBase64String(ReadAllBites());
        return $"data:{MediaType};base64,{base64}";
    }
}




