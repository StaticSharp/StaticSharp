namespace StaticSharp.Gears;
public class NotEvaluatable<T> {
    T value;
    public NotEvaluatable(T value) {
        this.value = value;
    }

    public static implicit operator T(NotEvaluatable<T> d) {
        if (d == null)
            throw new NotEvaluatableException();
        return d.value;
    }
}
