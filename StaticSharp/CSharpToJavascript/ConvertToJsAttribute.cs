using System;

namespace StaticSharp.Gears;
public class ConvertToJsAttribute : Attribute {

    public string? Format { get; }
    public ConvertToJsAttribute(string? format) {
        Format = format;
    }    
}
