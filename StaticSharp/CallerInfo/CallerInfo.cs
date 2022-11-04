using StaticSharp.Html;

namespace StaticSharp.Gears;

public abstract class CallerInfo{
    protected readonly string callerFilePath;
    protected readonly int callerLineNumber;

    public CallerInfo(string callerFilePath, int callerLineNumber) {
        this.callerFilePath = callerFilePath;
        this.callerLineNumber = callerLineNumber;
    }
    protected void ThrowInvalidUsage() { 
        throw new InvalidUsageException(callerFilePath, callerLineNumber);
    }

    protected void AddSourceCodeNavigationData(Tag tag, Context context) {
        if (context.DeveloperMode) {
            tag["data-caller-file-path"] = callerFilePath;
            tag["data-caller-line-number"] = callerLineNumber;
        }
    }
}
