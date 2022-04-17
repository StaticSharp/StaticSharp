namespace StaticSharp.Symbolic;
public class Number : Symbol {

    public Number(string value) : base(value) {
    }

    public static implicit operator Number(double value) => new(value.ToString()) { isConstant = true };

    public static Number operator +(Number a, double b) {
        return new($"({a.value}+{b})");
    }
    public static Number operator +(double a, Number b) {
        return new($"({a}+{b.value})");
    }

}


