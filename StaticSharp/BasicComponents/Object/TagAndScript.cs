

using Scopes;
using StaticSharp.Html;

namespace StaticSharp {


    namespace Gears {
        public class TagAndScript { 
            public Scopes.Group? Script { get; init; }
            public Tag Tag { get; init; }            

            public TagAndScript(Tag tag, Group? script) {
                Script = script;
                Tag = tag;
            }
        }
    }

}