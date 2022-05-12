namespace StaticSharp {
    public record FontStyle(
        FontWeight FontWeight = FontWeight.Regular,
        bool Italic = false
        ) : Gears.IKeyProvider {
        public string Key => Gears.KeyUtils.Combine<FontStyle>(FontWeight, Italic);
    }

}