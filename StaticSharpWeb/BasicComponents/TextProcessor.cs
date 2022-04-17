using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace StaticSharp.Gears {

    public static class TextProcessor {

        public static void SplidText(string value, IList<IElement> elements, string callerFilePath, int callerLineNumber) {
            if (string.IsNullOrEmpty(value))
                return;

            var lines = value.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            SplitLine(lines[0], elements);

            for (int i = 1; i < lines.Length; i++) {
                elements.Add(new Space());
                SplitLine(lines[i], elements);
            }


            void SplitLine(string value, IList<IElement> elements) {
                int start = 0;
                int length = 0;
                void AddWord() {
                    if (length > 0) {
                        var word = value.Substring(start, length);
                        elements.Add(new Word(word, callerFilePath, callerLineNumber));
                    }
                }

                int spaces = 0;

                for (int i = 0; i < value.Length; i++) {
                    var c = value[i];
                    if (c == ' ') {
                        AddWord();
                        start = i + 1;
                        length = 0;
                        spaces++;

                    } else {
                        if (spaces > 0) {
                            //TODO: add content to spaces
                            elements.Add(new Space(callerFilePath, callerLineNumber));
                            spaces = 0;
                        }
                        length++;
                    }
                }

                AddWord();
            }


        }

        

    }
}