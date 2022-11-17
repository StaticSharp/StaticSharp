using System;
namespace StaticSharp;

public class InvalidUsageException : Exception {
    protected readonly int callerLineNumber;
    protected readonly string callerFilePath;   

    public InvalidUsageException(int callerLineNumber, string callerFilePath) {
        this.callerLineNumber = callerLineNumber;
        this.callerFilePath = callerFilePath;        
    }
}
