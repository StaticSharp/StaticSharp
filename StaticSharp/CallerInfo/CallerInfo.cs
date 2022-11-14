using StaticSharp.Html;
using System;
using System.Diagnostics;

namespace StaticSharp.Gears;


public class UserApiAttribute : Attribute { 

}

public abstract class CallerInfo{
    protected readonly string callerFilePath;
    protected readonly int callerLineNumber;

    public CallerInfo(string callerFilePath, int callerLineNumber) {
        this.callerFilePath = callerFilePath;
        this.callerLineNumber = callerLineNumber;
        

        /*StackTrace stackTrace = new StackTrace(true);
        for (int i = 0; i < stackTrace.FrameCount; i++) { 
            var frame = stackTrace.GetFrame(i);
            var name = (frame?.GetFileName()??"<>")+ frame?.GetFileLineNumber().ToString();
            if (name != null)
                Console.WriteLine(name);
        }*/

        
        //Console.WriteLine("StackTrace: '{0}'", stackTrace.GetFrame(0));

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
