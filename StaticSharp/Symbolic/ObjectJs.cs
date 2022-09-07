using System;
namespace StaticSharp;


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
