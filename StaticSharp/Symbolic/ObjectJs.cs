using System;
namespace StaticSharp;


public class ObjectJs : SymbolJs {

    [ThreadStatic] public static bool NotEvaluatableFound = false;


    public ObjectJs() {}

    public static T NotEvaluatableObject<T>() where T: new() {
        NotEvaluatableFound = true;        
        return new();
    }

    public static T NotEvaluatableValue<T>(){
        NotEvaluatableFound = true;
        return default;
    }

}
