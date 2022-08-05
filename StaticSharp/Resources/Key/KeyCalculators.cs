using System.Drawing;

namespace StaticSharp.Gears;

public static partial class KeyCalculators {


    public static string GetKey(Color color) {
        return KeyUtils.Combine<Color>(color.ToArgb().ToString()) ;
    }

}




