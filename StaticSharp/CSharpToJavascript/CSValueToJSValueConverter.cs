using System.Drawing;
using System.Globalization;

namespace StaticSharp.Gears {

    public interface IConvertableToJsValue {
        string ToJsValue();
    }

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

            if (value is IConvertableToJsValue convertableToJsValue) {
                return convertableToJsValue.ToJsValue();
            }

            if (value is double valueAsDouble) {
                return valueAsDouble.ToString();
            }
            
            if (value is float valueAsFloat) {
                return valueAsFloat.ToString();
            }

            return value.ToString() ?? "";
        }

    }
    
    
}