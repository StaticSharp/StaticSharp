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
        protected IGenome<IAsset> assetGenome { get; }

        protected Template(Template other,
            string callerFilePath = "",
            int callerLineNumber = 0): base(other, callerFilePath, callerLineNumber) {
            assetGenome = other.assetGenome;                
        }
        public Template(IGenome<IAsset> assetGenome, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            this.assetGenome = assetGenome;
        }
        /*public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }*/

        /*public override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return Task.FromResult <Tag?>(null);
        }*/

        protected override async IAsyncEnumerable<KeyValuePair<string, string>> GetGeneratedBundingsAsync(Context context) {
            var template = (await assetGenome.CreateOrGetCached()).ReadAllText();
            yield return new("Html", $"()=>`{template}`");//
        }
    }
    


}