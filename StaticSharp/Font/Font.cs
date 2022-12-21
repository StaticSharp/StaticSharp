using System.Collections.Generic;

namespace StaticSharp {


    namespace Gears {
        public record Font(
            FontFamily Family,
            FontWeight Weight,
            bool Italic,
            List<Segment> Segments

        ) : IKeyProvider {
            public string Key => KeyUtils.Combine(GetType(),Family.Name, Weight, Italic);

            public HashSet<char> GetExistingChars(HashSet<char> chars) {
                var result = new HashSet<char>();
                int currentSegment = 0;
                foreach (var c in chars) {
                    for (int i = 0; i < Segments.Count; i++) {
                        var segment = Segments[currentSegment];
                        if (segment.Contains(i)) {
                            result.Add(c);
                            break;
                        }
                        currentSegment = (currentSegment + 1) % Segments.Count;
                    }
                }
                return result;
            }

            public static string ItalicToStyle(bool italic) => italic ? "italic" : "normal";

        }
    }
}