using StaticSharp.Html;
using System;
using System.Diagnostics;

namespace StaticSharp.Gears;


public class UserApiAttribute : Attribute { 

}

public abstract class CallerInfo{
    protected readonly int callerLineNumber;
    protected readonly string callerFilePath;

    public CallerInfo(int callerLineNumber, string callerFilePath) {
        this.callerLineNumber = callerLineNumber;
        this.callerFilePath = callerFilePath;
    }

    protected void ThrowInvalidUsage() { 
        throw new InvalidUsageException(callerLineNumber,callerFilePath);
    }

    protected void AddSourceCodeNavigationData(Tag tag, Context context) {
        if (context.DeveloperMode) {
            tag["data-caller-file-path"] = callerFilePath;
            tag["data-caller-line-number"] = callerLineNumber;
        }
    }
}
