using System;

public class Aggregator {
    [ThreadStatic] public static object Current = false;
}


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MixAttribute : Attribute {
    public MixAttribute(Type type) { }
}