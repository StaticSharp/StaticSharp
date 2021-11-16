using System.Drawing;
using System.Text;
using CsmlWeb.Html;
using CsmlWeb.ColorUtils;
using System.Collections;

namespace CsmlWeb {
    public abstract class ColorSequence<T> : IBlock
    {
        public abstract string GetGradient();
        public abstract float TotalDuration { get; }
        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            Tag tag = new Tag("div", new { Class = "ColorSequence"});
            tag.Attributes.Add("style", $"background:linear-gradient(to right, {GetGradient()}); animation: animatedBackgroundPositionHorizontal {TotalDuration}s linear infinite;");
            context.Includes.RequireStyle(new Style(new RelativePath("ColorSequence.scss")));
            tag.Add(new JSCall(new RelativePath("ColorSequence.js")).Generate(context));
            return tag;
        }
    }

    public class ColorSequence : ColorSequence<ColorSequence>, IEnumerable {
        public struct ColorDuration {
            public string color;
            public float duration;
        }
        List<ColorDuration> elements = new List<ColorDuration>();
        public override float TotalDuration => elements.Sum(x => x.duration);

        public ColorSequence() { }
        public ColorSequence this[string color, float duration] {
            get {
                if (duration <= 0) {
                    Log.Error.OnCaller("Invalod duration");
                }
                elements.Add(new ColorDuration() { color = color, duration = duration });
                return this;
            }
        }
        public ColorSequence this[Color color, float duration] {
            get {
                return this["#" + color.ToRgba().ToString("x8"), duration];
            }
            set {
                string color2 = color.ToRgba().ToString("x8");
                elements.Add(new ColorDuration() { color = color2, duration = duration});
            }
        }

        public ColorSequence this[Color color] {
            get {
                return this[color];
            }
            set {
                string c2 = color.ToRgba().ToString("x8");
            }
        }
        public void Add(ColorDuration item) {
            elements.Add(item);
        }

        public void Add() {

        }
        private ColorSequence(string color, float duration) {
            if (duration <= 0) {
                Log.Error.OnCaller("Invalid duration");
            }
            elements.Add(new ColorDuration() { color = color, duration = duration });
        }
        public ColorSequence(Color color, float duration) {   
                string color2 = color.ToRgba().ToString("x8");
                elements.Add(new ColorDuration() { color = color2, duration = duration});
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
        public static void Add(this IVerifiedBlockReceiver collection, ColorSequence item) {
            collection.AddBlock(item);
        }
    }

    public static class ColorSequenceCosStatic {
        public static void Add(this IVerifiedBlockReceiver collection, ColorSequenceCos item) {
            collection.AddBlock(item);
        }
    }
}