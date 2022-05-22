namespace StaticSharp.Gears;

public interface IFile {
    byte[] Content { get; }
    string MediaType { get; }
    string? CharSet { get; }
}




