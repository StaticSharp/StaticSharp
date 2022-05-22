namespace StaticSharp {
    public record FontStyle(
        FontWeight Weight = FontWeight.Regular,
        bool Italic = false
        ) : Gears.IKeyProvider {
        public string Key => Gears.KeyUtils.Combine<FontStyle>(Weight, Italic);
        public string CssFontStyle => Italic ? "italic" : "normal";
    }

}