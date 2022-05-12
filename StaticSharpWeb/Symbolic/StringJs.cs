namespace StaticSharp;

public class StringJs : SymbolJs {

    public StringJs(string value) : base(value) {
    }

    public static implicit operator StringJs(string value) => new($"\"{value}\"") { isConstant = true };

    public static NumberJs operator +(StringJs a, StringJs b) {
        return new($"({a.value}+{b.value})");
    }
    /*public static Number operator +(string a, String b) {
        return new($"({a}+{b.value})");
    }*/


}


