using System.Drawing;
using System.Globalization;

namespace StaticSharp.Gears {
    public partial class CSValueToJSValueConverter {

        public static string ObjectToJsValue(object? value) {
            if (value == null)
                return "null";
            if (value is bool valueAsBool) {
                return valueAsBool.ToString().ToLower();
            }
            if (value is string valueAsString) {
                return "\"" + valueAsString + "\"";
            }

            if (value.GetType().IsEnum) {
                return "\"" + value.ToString() + "\"";
            }

            if (value is Color valueAsColor) {                
                var hex = valueAsColor.A.ToString("X2") + valueAsColor.B.ToString("X2") + valueAsColor.G.ToString("X2") + valueAsColor.R.ToString("X2");
                return $"new Color(0x{hex})";//{valueAsColor.A},{valueAsColor.R},{valueAsColor.G},{valueAsColor.B}
            }

            if (value is double valueAsDouble) {
                return valueAsDouble.ToInvariant();
            }
            
            if (value is float valueAsFloat) {
                return valueAsFloat.ToInvariant();
            }

            return value.ToString() ?? "";
        }

    }
    
    
}