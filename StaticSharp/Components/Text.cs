using System;

namespace StaticSharpWeb.Components {

    public enum TextAlign {
        Left,
        Right,
        Center,
        Justify,
        Inherit,
        Undefined
    }

    public static class TextAlignStatic {

        public static string ToCss(this TextAlign align) => Enum.GetName(typeof(TextAlign), align)?.ToLower();
    }
}