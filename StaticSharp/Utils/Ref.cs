namespace StaticSharp.Gears {
    public class Ref<T> where T : struct {
        public T Value = default;

        public Ref() {}

        public Ref(T value) {
            Value = value;
        }


        public override string? ToString() {
            return Value.ToString();
        }

        public static implicit operator T(Ref<T> wrapper) {
            return wrapper.Value;
        }

        /*public static implicit operator Ref<T>(T value) {
            return new Ref<T>(value);
        }*/
    }
}
