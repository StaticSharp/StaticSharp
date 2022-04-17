namespace StaticSharp.Symbolic;

public class Object : Symbol {

    public Object() { }
    public Object(string value) : base(value) {}
    public Symbol this[string i] {
        get { return new Symbol($"{value}[\"{i}\"]"); }
    }
}
