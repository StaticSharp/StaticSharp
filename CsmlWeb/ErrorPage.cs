using CsmlWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsmlWeb {

    public class ErrorPage : IPage {
        public Exception Exception { get; }
        public string Title => "Internal server error";

        public ErrorPage(Exception exception) => Exception = exception;

        private class CallStack {
            public string Place { get; init; }
            public string File { get; init; }
            public string Line { get; init; }

        }

        private static Tag ReplaceLinks(string stackTrace) {
            var regex = new Regex(@"(in (?<file>.*):line (?<line>\d*))");
            var matches = regex.Matches(stackTrace);
            var listOfCallStack = new List<CallStack>();
            var result = matches.Select(x => new CallStack() {
                Place = x.Groups[0]?.ToString(),
                File = x.Groups["file"]?.Value,
                Line = x.Groups["line"]?.Value
            });
            var tag = new Tag("div") {
                new Tag("script", new { type = "text/javascript"}){
                    "function findLine(file, line){fetch(`/api/v1/visual_studio/${file}/${line}`);" +
                    "return false;}"
                }
            };
            var stackTraceArray = stackTrace.Split(Environment.NewLine);
            foreach(var stackPart in stackTraceArray) {
                var added = false;
                foreach(var callStack in result) {
                    if(stackPart.Contains(callStack.Place)) {
                        tag.Add(stackPart.Replace(callStack.Place, ""));
                        tag.Add(new Tag("a", new {
                            href = "localhost/aaaa/aaa/aa",
                            onclick = $"let xhr = new XMLHttpRequest();" +
                            $"xhr.open('POST', '/api/v1/visual_studio'); " +
                            $"let requestBody = {{}};" +
                            $"requestBody['file'] = '{callStack.File.Replace("\\", "\\\\")}';" +
                            $"requestBody['line'] = '{callStack.Line}';" +
                            $"xhr.send(JSON.stringify(requestBody)); " +
                            $"return false; "
                        }) {
                            $"{callStack.File}:line {callStack.Line}"
                        });
                        tag.Add(new Tag("br"));
                        added = true;
                        break;
                    }
                }
                if(!added) {
                    tag.Add(stackPart);
                    tag.Add(new Tag("br"));
                }
            }

            return tag;
        }

        public async Task<string> GenerateHtmlAsync(Context context) {
            var head = new Tag("head"){
                new Tag("meta", new{ charset = "utf-8"}),
                new Tag("title"){
                    Title
                },
            };
            var body = new Tag("body") {
                new Tag("h1"){ Title }
            };
            body.Add(Exception.Message);
            if(!string.IsNullOrEmpty(Exception.InnerException?.Message)) {
                body.Add(Exception.InnerException.Message);
            }
            if(!string.IsNullOrEmpty(Exception.StackTrace)) {
                body.Add(ReplaceLinks(Exception.StackTrace));
            }
            var document = new Tag(null) {
                new Tag("!doctype",new{ html = ""}),
                head,
                body
            };
            return document.GetHtml();
        }
    }
}