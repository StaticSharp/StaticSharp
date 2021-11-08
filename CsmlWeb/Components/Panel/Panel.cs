using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsmlWeb.Html;

namespace CsmlWeb {
    readonly public ref struct Test {
    }

    public class Panel : IBlock {

        //public string _content;
        private string _content { get; set; }


        //public List<string> _privateList = new();
        //private string[] _classes;
        // public Panel(string content) {
        //     _content = content;
        // }

                public Panel(string content)
        {
            _content = content;
        }


        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            var componentClassName = this.GetType().Name;
            var componentName = nameof(Panel);
            var tag = new Tag("div", new { Class = componentName + " " + componentClassName }) {
                //new Tag("panel", new { style = "width: 100%; height: auto;"}),
                new JSCall(new RelativePath(componentName + ".js")).Generate(context)
            };
            //var componentName = nameof(Panel);
            context.Includes.RequireStyle(new Style(new RelativePath(componentName + ".scss")));
            context.Includes.RequireFont(new Font(new RelativePath("..\\..\\Fonts\\materialdesignicons"), FontWeight.Regular));
            //context.Includes.RequireStyle(new Style(new RelativePath(componentClassName + ".scss")));
            tag.Add(new JSCall(new RelativePath(componentName + ".js")).Generate(context));
            tag.Add(new Tag("div", new { Class = "Text"}));
            tag.Add(_content);
            return tag;
        }


        //public IEnumerator GetEnumerator() => _privateList.GetEnumerator();
    }

    public class Info : Panel {
        //public Info(string content, params string[] classes) : base(content, classes) { }
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
