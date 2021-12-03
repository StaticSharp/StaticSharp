using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticSharpWeb.Html;

namespace StaticSharpWeb {
    public class Panel : IBlock {

        private string _content { get; set; }

        public Panel(string content)
        {
            _content = content;
        }


        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            var componentClassName = this.GetType().Name;
            var componentName = nameof(Panel);
            var tag = new Tag("div") {
                new Tag("div", new {Class = componentName + " " + componentClassName }){
                    _content
                }
            };
            context.Includes.Require(new Style(new RelativePath(componentName + ".scss")));
            context.Includes.Require(new Font(new RelativePath("..\\..\\Fonts\\materialdesignicons"), FontWeight.Regular));
            tag.Add(new JSCall(new RelativePath(componentName + ".js")).Generate(context));
            return tag;
        }

    }

    public class Info : Panel {
        public Info(string content = null) : base(content) { }
    }

    public class Error : Panel {
        public Error(string content = null) : base(content) { }
    }
    public class Bug : Panel {
        public Bug(string content = null) : base(content) { }
    }
    public class Note : Panel {
        public Note(string content = null) : base(content) { }
    }
    public class Success : Panel {
        public Success(string content = null) : base(content) { }
    }
    public class Warning : Panel {
        public Warning(string content = null) : base(content) { }
    }

    public static class PanelStatic {
        public static void Add(this IVerifiedBlockReceiver collection, Panel item) {
            collection.AddBlock(item);
        }
    }   
}
