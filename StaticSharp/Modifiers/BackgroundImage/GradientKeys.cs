namespace StaticSharp {


    public class GradientKeys<J>: List<string>{

        public Dictionary<string, string> Properties { get; } = new();

        string AddProperty(string value) {
            var name = "internal" + Properties.Count;
            Properties.Add(name, value);
            return name;
        }

        string BindingToInterpolatedStringFragment(Binding<J, Color> binding) {
            if (binding.IsExpression) {
                var property = AddProperty(binding.CreateScriptExpression());
                return $"${{e.{property}}}";
            } else {
                return $"#{binding.Value.ToHex()}";
            }
        }

        string BindingToInterpolatedStringFragment_FractionToPercentage(Binding<J, double> binding) {
            if (binding.IsExpression) {
                var property = AddProperty(binding.CreateScriptExpression());
                return $"${{e.{property}*100}}%";
            } else {
                return $"{binding.Value*100}%";
            }
        }


        public void Add(Binding<J, Color> color, Binding<J, double>? position = null) {
            var result = BindingToInterpolatedStringFragment(color);
            if (position.HasValue) {
                result += " "+BindingToInterpolatedStringFragment_FractionToPercentage(position.Value);
            }
            Add(result);
        }

        public override string ToString() {
            return string.Join(',', this);
        }

    }


    


}
