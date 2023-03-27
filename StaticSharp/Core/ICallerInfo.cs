namespace StaticSharpWeb {
    public interface ICallerInfo {
        string CallerFilePath { get; }
        int CallerLineNumber { get; }
    }
}