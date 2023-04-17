

using Scopes;

namespace StaticSharp {


    namespace Gears {
        public class IdAndScript {
            public Scopes.Group Script { get; init; }
            public string Id { get; init; }

            public IdAndScript(string id, Group script) {
                Id = id;
                Script = script;
                
            }
        }
    }

}