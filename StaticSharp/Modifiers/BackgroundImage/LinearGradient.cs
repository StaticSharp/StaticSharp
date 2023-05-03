using Scopes.C;
using StaticSharp.Gears;


namespace StaticSharp {

    public interface JLinearGradient : JAbstractBackground {
        double Angle { get; set; }
    }

    public partial class LinearGradient : AbstractBackground {
        public GradientKeys<JLinearGradient> Keys { get; } = new();
        public override IdAndScript Generate(string elementId, Context context) {
            var result = base.Generate(elementId, context);

            if (Keys.Properties.Count > 0) {
                result.Script.Add(new Scope($"{result.Id}.Reactive = "){
                    Keys.Properties.Select(x => $"{x.Key}:{x.Value},")
                });
            }

            var keys = Keys.ToString();
            result.Script.Add($"{result.Id}.RawImage = e=>`linear-gradient(${{e.Angle||0}}turn, {keys})`");
            return result;
        }
    }


}
