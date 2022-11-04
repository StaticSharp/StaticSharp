using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StaticSharp.Gears {

    public class ErrorPage : IPageGenerator {
        public Exception Exception { get; }
        public string Title => "Internal server error";

        public ErrorPage(Exception exception) => Exception = exception;

        private class CallStack {
            public string Place { get; init; }
            public string File { get; init; }
            public string Line { get; init; }

            public CallStack(string place, string file, string line) {
                Place = place;
                File = file;
                Line = line;
            }

            public CallStack() {
            }
        }

        private static Tag ReplaceLinks(string stackTrace) {
            var regex = new Regex(@"(in (?<file>.*):line (?<line>\d*))");
            var matches = regex.Matches(stackTrace);
            var listOfCallStack = new List<CallStack>();
            var result = matches.Select(x => 
                new CallStack(x.Groups[0]?.ToString(), x.Groups["file"]?.Value, x.Groups["line"]?.Value));
            var tag = new Tag("div") {
                /*new Tag("script"){
                    ["type"] = "text/javascript",
                    Children = {
                        "function findLine(file, line){fetch(`/api/v1/visual_studio/${file}/${line}`);" +
                        "return false;}"
                    }
                }*/
            };
            var stackTraceArray = stackTrace.Split(Environment.NewLine);
            foreach (var stackPart in stackTraceArray) {
                var added = false;
                foreach (var callStack in result) {
                    if (stackPart.Contains(callStack.Place)) {
                        tag.Add(stackPart.Replace(callStack.Place, ""));
                        tag.Add(new Tag("a") {
                            ["href"] = "localhost/aaaa/aaa/aa",
                            ["onclick"] = $"let xhr = new XMLHttpRequest();" +
                            $"xhr.open('POST', '/api/v1/visual_studio'); " +
                            $"let requestBody = {{}};" +
                            $"requestBody['file'] = '{callStack.File.Replace("\\", "\\\\")}';" +
                            $"requestBody['line'] = '{callStack.Line}';" +
                            $"xhr.send(JSON.stringify(requestBody)); " +
                            $"return false; ",

                            Children = {
                                $"{callStack.File}:line {callStack.Line}"
                            }
                        }
                        );
                        tag.Add(new Tag("br"));
                        added = true;
                        break;
                    }
                }
                if (!added) {
                    tag.Add(stackPart);
                    tag.Add(new Tag("br"));
                }
            }

            return tag;
        }

        public async Task<string> GeneratePageHtmlAsync(Context context) {
            var head = new Tag("head"){
                new Tag("meta"){
                    ["charset"] = "utf-8"
                },
                new Tag("title"){
                    Title
                },
            };
            var body = new Tag("body") {
                new Tag("h1"){ Title }
            };
            body.Add(Exception.Message);
            if (!string.IsNullOrEmpty(Exception.InnerException?.Message)) {
                body.Add(Exception.InnerException.Message);
            }
            if (!string.IsNullOrEmpty(Exception.StackTrace)) {
                body.Add(ReplaceLinks(Exception.StackTrace));
            }
            var document = new Tag(null) {
                new Tag("!doctype"){
                    ["html"] = ""
                },
                head,
                body
            };
            return document.GetHtml();
        }
    }
}