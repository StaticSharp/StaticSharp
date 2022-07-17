using System.Drawing;

namespace StaticSharp.Gears {
    public partial class Static {

        public static string ObjectToJsValue(object? value) {
            if (value == null)
                return "null";
            if (value is bool valueAsBool) {
                return valueAsBool.ToString().ToLower();
            }
            if (value is string valueAsString) {
                return "\"" + valueAsString + "\"";
            }
            if (value is Color valueAsColor) {                
                var hex = valueAsColor.A.ToString("X2") + valueAsColor.B.ToString("X2") + valueAsColor.G.ToString("X2") + valueAsColor.R.ToString("X2");
                return $"new Color(0x{hex})";//{valueAsColor.A},{valueAsColor.R},{valueAsColor.G},{valueAsColor.B}
            }

            return value.ToString() ?? "";
        }

    }
    
    
}