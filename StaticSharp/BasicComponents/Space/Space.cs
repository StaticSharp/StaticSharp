using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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



public class SpaceBindings<FinalJs> : HierarchicalBindings<FinalJs> where FinalJs : new() {
    public SpaceBindings(Dictionary<string, string> properties) : base(properties) {
    }
    public Expression<Func<SpaceJs, float>> GrowBefore { set { AssignProperty(value); } }
    public Expression<Func<SpaceJs, float>> GrowBetween { set { AssignProperty(value); } }
    public Expression<Func<SpaceJs, float>> GrowAfter { set { AssignProperty(value); } }
    public Expression<Func<SpaceJs, float>> MinBetween { set { AssignProperty(value); } }
}

    [ScriptBefore]
public sealed class Space: Hierarchical, IBlock {

    public new SpaceBindings<HierarchicalJs> Bindings => new(Properties);

    public override string TagName => "ws";
    public Space([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        : base(callerFilePath, callerLineNumber) { }


    public override void AddRequiredInclues(IIncludes includes) {
        base.AddRequiredInclues(includes);
        includes.Require(new Script(ThisFilePathWithNewExtension("js")));
    }

    public override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
        return Task.FromResult<Tag>(null);
    }

    /*public Task<Tag> GenerateHtmlAsync(Context context,string? id) {
        AddRequiredInclues(context.Includes);
        return Task.FromResult(new Tag("ws",id) {
            CreateScriptBefore()
        });
    }*/


}
