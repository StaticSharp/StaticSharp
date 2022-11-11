using System;
using System.Linq;

namespace StaticSharp.Gears;

public static class MultilanguageRedirect {
    public static string GenerateHtml<LanguageEnum>() where LanguageEnum : struct, Enum {

        var languages = string.Join(",", Enum.GetNames<LanguageEnum>().Select(x => $"\"{x.ToLower()}\""));
        
        //if (navigator.userLanguage) return [navigator.userLanguage];
        //if (navigator.browserLanguage) return [navigator.browserLanguage];

        var script = $$"""
            function matchLanguage(pageLanguages) {
                function getBrowserLanguages() {
                    if (!navigator) return [];
                    if (navigator.languages) return navigator.languages;                    
                    return [navigator.language];
                }
                var browserLanguages = getBrowserLanguages().map(x => x.replace("-", "_").toLowerCase())
                console.log(pageLanguages,browserLanguages)
                for (let i of browserLanguages) {
                    if (pageLanguages.includes(i)) {
                        return i
                    }
                }
                for (let b of browserLanguages) {
                    for (let p of pageLanguages) {
                        if (b.startsWith(p)) {
                            return p
                        }
                    }
                }
                return pageLanguages[0]
            }
            var extension = (window.location.protocol=="file:") ? ".html" : ""
            window.location.replace(matchLanguage([{{languages}}]) + extension)
            """;
        
        var html = $"""
            <!DOCTYPE html>
            <html>
                <head>
                    <script>{script}</script>
                    <meta http-equiv="refresh" content="0; url={default(LanguageEnum).ToString().ToLower()}" />
                </head>
                <body></body>
            </html>
            """;

        return html;
    }

}