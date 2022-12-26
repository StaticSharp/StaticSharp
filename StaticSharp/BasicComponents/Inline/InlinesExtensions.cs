using System;

namespace StaticSharp {
    public static class InlinesExtensions {


        public static Inlines ProcessTextFragments(this Inlines x, Func<string,string> func) {
            foreach (var i in x.Values) {
                if (i is Text text) {
                    text.Value = func(text.Value);
                } else if (i is Inline inline){
                    inline.Children.ProcessTextFragments(func);
                }
            }
            return x;
        }

        public static Inlines UnderscoreToNbsp(this Inlines x) {
            return x.ProcessTextFragments(s => s.UnderscoreToNbsp());
        }
        public static string UnderscoreToNbsp(this string x) {
            return x.Replace('_', '\u00A0');
        }

    }

}