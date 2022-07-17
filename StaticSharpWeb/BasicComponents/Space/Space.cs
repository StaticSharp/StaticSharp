using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp;

[System.Diagnostics.DebuggerNonUserCode]
public class SpaceJs : HierarchicalJs {
    public float GrowBefore =>  throw new NotEvaluatableException();
    public float GrowBetween => throw new NotEvaluatableException();
    public float GrowAfter =>   throw new NotEvaluatableException();
    public float MinBetween =>  throw new NotEvaluatableException();
}


[ScriptBefore]
public sealed class Space: Hierarchical<SpaceJs>, IBlock {
    public Binding<float> GrowBefore  { set; private get; }
    public Binding<float> GrowBetween { set; private get; }
    public Binding<float> GrowAfter   { set; private get; }
    public Binding<float> MinBetween  { set; private get; }

    public override string TagName => "ws";
    public Space([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        : base(callerFilePath, callerLineNumber) { }


    public override void AddRequiredInclues(IIncludes includes) {
        base.AddRequiredInclues(includes);
        includes.Require(new Script(ThisFilePathWithNewExtension("js")));
    }

    public override Task<Tag?> GenerateHtmlChildrenAsync(Context context) {
        return Task.FromResult<Tag>(null);
    }

    /*public Task<Tag> GenerateHtmlAsync(Context context,string? id) {
        AddRequiredInclues(context.Includes);
        return Task.FromResult(new Tag("ws",id) {
            CreateScriptBefore()
        });
    }*/


}
