namespace StaticSharp;

public class ObjectJs : SymbolJs {

    public ObjectJs() { }
    public ObjectJs(string value) : base(value) {}
    public SymbolJs this[string i] {
        get { return new SymbolJs($"{value}[\"{i}\"]"); }
    }

    public T As<T>() where T : ObjectJs, new() {
        return new T { value = value };
    }

}
