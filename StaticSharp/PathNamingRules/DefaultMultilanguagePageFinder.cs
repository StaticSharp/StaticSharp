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

        public IPageGenerator? FindPage(string requestPath) {
            if (requestPath == null) {
                return null;
            }

            var extension = Path.GetExtension(requestPath);
            if (string.IsNullOrEmpty(extension) || extension.ToLower() == ".html") {

                if (!string.IsNullOrEmpty(extension)) {
                    requestPath = Path.GetFileNameWithoutExtension(requestPath);
                }

                string[] path = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

                LanguageEnum language = default;
                if (path.Length > 0) {
                    var languageString = path[path.Length-1];
                    
                    foreach (var i in Enum.GetValues<LanguageEnum>()) {
                        if (i.ToString().ToLower() == languageString) {
                            language = i;
                            path = path.SkipLast(1).ToArray();
                            break;
                        }
                    }
                }

                var result = RootNodeConstructor(language);
                //var pathList = new List<string[]>();
                //var root = new αRoot(language).Children.FirstOrDefault().Name;
                foreach (var pathPart in path) {
                    result = result.Children.FirstOrDefault(x => x.Name.ToLower() == pathPart.ToLower());
                    if (result == null) return null;
                }

                return result.Representative;


                /*if (path.Length == 0) {
                    return RootNodeConstructor(default).Representative;
                }

                var htmlName = path.Last();
                htmlName = htmlName[..htmlName.LastIndexOf('.')].ToLower();

                var lastIndexOf_ = htmlName.LastIndexOf('_');

                if (lastIndexOf_ != -1) {
                    var languagePart = htmlName[(lastIndexOf_ + 1)..];
                    language = Enum.GetValues<LanguageEnum>().FirstOrDefault(i => htmlName.EndsWith(i.ToString().ToLower()));
                    htmlName = htmlName[..lastIndexOf_];
                }
                path[^1] = htmlName;

                if (path.Length == 1 && path[0] == "index")
                    return RootNodeConstructor(language).Representative;*/




            } else {
                return null;
            }

            
        }
    }




}
