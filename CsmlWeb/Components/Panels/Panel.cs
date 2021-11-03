using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsmlWeb.Components.Panels {
    class Panel {

        private string _content;
        private string[] _classes;
        public Panel(string content, params string[] classes) {
            _content = content;
            _classes = classes;
        }

        
    }
}
