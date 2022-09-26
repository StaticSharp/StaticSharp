using System;

namespace StaticSharp.Gears;
public class ObjectJs {

    [ThreadStatic] public static bool NotEvaluatableFound = false;

    protected static T NotEvaluatableObject<T>() where T: new() {
        NotEvaluatableFound = true;        
        return new();
    }
    protected static T NotEvaluatableValue<T>(){
        NotEvaluatableFound = true;
        return default;
    }
}

public static class ObjectJsStatic {

    [ConvertToJs("{0}")]
    public static T As<T>(this ObjectJs _) where T : ObjectJs, new() => new();
}
