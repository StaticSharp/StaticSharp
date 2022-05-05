using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {




    public abstract class Reactive : CallerInfo,  IReactiveObjectCs {
        public Reactive(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        //public abstract Task<Tag> GenerateHtmlAsync(Context context);

        public virtual void AddRequiredInclues(IIncludes includes) {
            includes.Require(new Script(AbsolutePath("Math.js")));
            includes.Require(new Script(AbsolutePath("Constants.js")));
            includes.Require(new Script(AbsolutePath("Constructor.js")));
            includes.Require(new Script(AbsolutePath("Bindings.js")));
        }

        public string GeneratePropertyDeclaration() {
            return GetType().FullName;
        }

        public Tag CreateScriptBefore() {
            return CreateScriptBefore(GetType().Name);
        }
        public Tag CreateScriptBefore(string name) {
            return new Tag("script") {
                new PureHtmlNode($"ConstructorBefore(\"{name}\")")
            };
        }

        public Tag CreateScriptAfter() {
            return CreateScriptAfter(GetType().Name);
        }
        public Tag CreateScriptAfter(string name) {
            return new Tag("script") {
                new PureHtmlNode($"ConstructorAfter(\"{name}\")")
            };
            /*
            return new Tag("script") {
                new PureHtmlNode(
@$"let s = document.currentScript;
let p = s.parentElement;
p.removeChild(s);
{name}After(p);")
            };*/
        }


    }
    
}