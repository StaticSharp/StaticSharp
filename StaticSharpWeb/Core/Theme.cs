using System.Drawing;

namespace StaticSharpWeb {
    public class Theme {

        public virtual Color Surface => Color.FromArgb(64,64,64);
        public virtual Color OnSurface => Surface.ContrastTextColor(0.9f);




        public virtual float BaseSize => 16;

        public virtual float ParagraphFontSize => BaseSize;
        public virtual float ParagraphSpacing => 0.6f * BaseSize;
        public virtual float HeadingSpacing => 2 * BaseSize;

        public virtual Color HeadingAnchorIconColor => Color.LightGray;

    }
}