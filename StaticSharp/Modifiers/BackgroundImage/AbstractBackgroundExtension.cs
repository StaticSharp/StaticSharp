namespace StaticSharp {
    public static partial class AbstractBackgroundExtension {
        public static T Right<T>(this T _this) where T:AbstractBackground {
            _this.X = new(e => e.AsBlock().Width - e.Width);
            return _this;
        }
        public static T Bottom<T>(this T _this) where T : AbstractBackground {
            _this.Y = new(e => e.AsBlock().Height - e.Height);
            return _this;
        }

        public static T CenterX<T>(this T _this) where T : AbstractBackground {
            _this.X = new(e => (e.AsBlock().Width - e.Width) * 0.5);
            return _this;
        }
        public static T CenterY<T>(this T _this) where T : AbstractBackground {
            _this.Y = new(e => (e.AsBlock().Height - e.Height) * 0.5);
            return _this;
        }
        public static T Center<T>(this T _this) where T : AbstractBackground => _this.CenterX().CenterY();
    }

}
