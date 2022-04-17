namespace StaticSharp.Symbolic;

public class Symbol{
    public string value = "";
    public bool isConstant = false;
    public Symbol() {}
    public Symbol(string value) {
        this.value = value;
    }

    public override string ToString() {
        return value;
    }

    /*public Symbol(int value) {
        this.value = value;
    }
    public static implicit operator Symbol(int value) => new Symbol(value);

    public Symbol(float value) {
        this.value = value;
    }
    public static implicit operator Symbol(float value) => new Symbol(value);*/



    /*public static Symbol operator -(Symbol a, Symbol b) {
        return new Symbol(a.value - b.value);
    }
    public static Symbol operator *(Symbol a, Symbol b) {
        return new Symbol(a.value * b.value);
    }*/
}
