using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {



    /*[System.Diagnostics.DebuggerNonUserCode]
    public class TemplateJs : BlockJs {
        public TemplateJs() { }
    }*/


    [ConstructorJs]

    public class Template : Block {
        protected Genome<IAsset> assetGenome { get; }

        protected Template(Template other,
            string callerFilePath = "",
            int callerLineNumber = 0): base(other, callerLineNumber, callerFilePath) {
            assetGenome = other.assetGenome;                
        }
        public Template(Genome<IAsset> assetGenome, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            this.assetGenome = assetGenome;
        }
        /*public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }*/

        /*public override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return Task.FromResult <Tag?>(null);
        }*/

        protected override IEnumerable<KeyValuePair<string, string>> GetGeneratedBundings(Context context) {
            var template = assetGenome.Result.Text;
            yield return new("Html", $"()=>`{template}`");//
        }
    }
    


}