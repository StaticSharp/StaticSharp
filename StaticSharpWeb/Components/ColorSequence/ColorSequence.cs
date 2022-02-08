using System.Drawing;
using System.Text;
using StaticSharpWeb.Html;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public abstract class ColorSequence<T> : IBlock
    {
        public abstract string GetGradient();
        public abstract float TotalDuration { get; }
        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            var tag = new Tag("div");
            var innerTag = new Tag("div", new { Class = "ColorSequence"});
            innerTag.Attributes.Add("style", $"background:linear-gradient(to right, {GetGradient()}); animation: animatedBackgroundPositionHorizontal {TotalDuration}s linear infinite;");
            context.Includes.Require(new Style(new AbsolutePath("ColorSequence.scss")));
            tag.Attributes.Add("style", "padding-bottom: 0.2em");
            tag.Add(innerTag);
            tag.Add(new JSCall(new AbsolutePath("ColorSequence.js")).Generate(context));
            return tag;
        }
    }

    public class ColorSequence : ColorSequence<ColorSequence>, IEnumerable {
        public struct ColorDuration {
            public string color;
            public float duration;
            private ColorDuration(string color, float duration) {
                this.color = color;
                this.duration = duration;
            }

            public ColorDuration(Color color, float duration) {
                this.color = "#" + color.ToRgba().ToString("x8");
                this.duration = duration;
            }
        }
        List<ColorDuration> elements = new List<ColorDuration>();
        public override float TotalDuration => elements.Sum(x => x.duration);

        public ColorSequence() { }
        
        public void Add(ColorDuration item) {
            elements.Add(item);
        }

        public void Add(Color color, float duration) {
            elements.Add(new(color, duration));
        }

        public override string GetGradient()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var totalDuration = TotalDuration;
            float currentPosition = 0;
            foreach (var e in elements) {
                if (currentPosition > 0) stringBuilder.Append(',');
                stringBuilder.Append($"{e.color} {100 * currentPosition / totalDuration}%,");
                currentPosition += e.duration;
                stringBuilder.Append($"{e.color} {100 * currentPosition / totalDuration/* - 0.5f*/}%");
            }
            return stringBuilder.ToString();
        }

        public IEnumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }

    public class ColorSequenceCos : ColorSequence<ColorSequenceCos>{
        float UserGefinedDuration;
        public override float TotalDuration => UserGefinedDuration;
        Color Min;
        Color Max;
        public ColorSequenceCos(Color min, Color max, float duration) {
            UserGefinedDuration = duration;
            Min = min;
            Max = max;
        }

        public override string GetGradient() {
            StringBuilder stringBuilder = new StringBuilder();
            int numSteps = 20;
            for (int i = 0; i <= numSteps; i++) {
                float p = i / (float)numSteps;
                float a = 2 * MathF.PI * p;
                var y = 0.5f * MathF.Cos(a) + 0.5f;
                var color = Min.Lerp(Max, y);
                if (i > 0) stringBuilder.Append(',');
                stringBuilder.Append($"#{color.ToRgb().ToString("x6")} {100 * p}%");
            }
            return stringBuilder.ToString();
        }
    }

    public static class ColorSequenceStatic {
        public static void Add(this IBlockContainer collection, ColorSequence item) {
            collection.AddBlock(item);
        }
    }

    public static class ColorSequenceCosStatic {
        public static void Add(this IBlockContainer collection, ColorSequenceCos item) {
            collection.AddBlock(item);
        }
    }
}