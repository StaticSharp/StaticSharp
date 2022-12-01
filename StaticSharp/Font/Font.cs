using System.Collections.Generic;

namespace StaticSharp {


    namespace Gears {
        public record Font(
            FontFamily FontFamily,
            FontStyle FontStyle,
            List<Segment> Segments

        ) : IKeyProvider {
            public string Key => KeyUtils.Combine(GetType(),FontFamily.Name,FontStyle);

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
        }

        //internal class FontFamilyConstants

    }

}