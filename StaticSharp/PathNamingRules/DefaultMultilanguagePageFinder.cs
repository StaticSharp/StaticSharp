using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StaticSharp {
    public class DefaultMultilanguagePageFinder<LanguageEnum> : IPageFinder where LanguageEnum : struct, Enum {

        private Func<LanguageEnum, MultilanguageProtoNode<LanguageEnum>> RootNodeConstructor;
        public DefaultMultilanguagePageFinder(Func<LanguageEnum, MultilanguageProtoNode<LanguageEnum>> rootNodeConstructor) {
            RootNodeConstructor = rootNodeConstructor;
        }

        public Page? FindPage(string requestPath, out FilePath closest) {

            var defaultLanguagePart = new FilePath(default(LanguageEnum).ToString().ToLower());

            if (requestPath == null) {
                closest = defaultLanguagePart;
                return null;
            }
            
            var extension = Path.GetExtension(requestPath);
            if (string.IsNullOrEmpty(extension) || extension.ToLower() == ".html") {

                if (!string.IsNullOrEmpty(extension)) {
                    requestPath = requestPath[0..^extension.Length];
                    //requestPath = Path.GetFileNameWithoutExtension(requestPath);
                }

                string[] path = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                
                bool languageFound = false;
                LanguageEnum language =  default;
                if (path.Length > 0) {
                    var languageString = path[path.Length-1];
                    foreach (var i in Enum.GetValues<LanguageEnum>()) {
                        if (i.ToString().ToLower() == languageString) {
                            language = i;
                            path = path.SkipLast(1).ToArray();
                            languageFound = true;
                            break;
                        }
                    }
                }

                var result = RootNodeConstructor(language);
                closest = new FilePath();
                foreach (var pathPart in path) {                    
                    result = result.Children.FirstOrDefault(x => x.Name.ToLower() == pathPart.ToLower());
                    if (result == null) {
                        closest += defaultLanguagePart;
                        return null;
                    }
                    closest += pathPart;
                }

                closest += defaultLanguagePart;
                if (!languageFound)
                    return null;

                return result.Representative;
            } else {
                closest = defaultLanguagePart;
                return null;
            }

            
        }
    }




}
