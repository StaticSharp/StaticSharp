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

}
