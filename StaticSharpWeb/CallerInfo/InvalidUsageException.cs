using System;
namespace StaticSharp;

public class InvalidUsageException : Exception {
    protected readonly string callerFilePath;
    protected readonly int callerLineNumber;

    public InvalidUsageException(string callerFilePath, int callerLineNumber) {
        this.callerFilePath = callerFilePath;
        this.callerLineNumber = callerLineNumber;
    }
}
