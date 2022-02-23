using System.Drawing;

namespace StaticSharpWeb {
    public class Theme {

        public virtual Color Surface => Color.FromArgb(64,64,64);
        public virtual Color OnSurface => Surface.ContrastTextColor(0.9f);


        public virtual float BaseSpacing => 16;

        public virtual float ParagraphSpacing => BaseSpacing;
        public virtual float HeadingSpacing => 2 * BaseSpacing;

        public virtual Color HeadingAnchorIconColor => Color.LightGray;

    }
}