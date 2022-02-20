namespace StaticSharpGears;

public interface IFile {
    IAwaitable<Func<Stream>> Content { get; }
    IAwaitable<string> MediaType { get; }
    IAwaitable<string?> CharSet { get; }
}




