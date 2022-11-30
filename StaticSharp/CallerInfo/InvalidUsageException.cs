using System;



namespace StaticSharp.Gears;

public class InvalidUsageException : Exception {
    protected readonly int callerLineNumber;
    protected readonly string callerFilePath;   

    public InvalidUsageException(int callerLineNumber, string callerFilePath) {
        this.callerLineNumber = callerLineNumber;
        this.callerFilePath = callerFilePath;        
    }

    public override string? StackTrace {
        get { 
            var stackTrace = base.StackTrace;
            return "StaticSharp.Image.ModifyHtmlAsync(StaticSharp.Context, StaticSharp.Html.Tag) in Image.cs";
        }
    } 


}
