using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp;


public class SpaceJs : HierarchicalJs {
    public SpaceJs() { }
    public SpaceJs(string value) : base(value) {
    }

    public NumberJs GrowBefore => new($"{value}.GrowBefore");
    public NumberJs GrowBetween => new($"{value}.GrowBetween");
    public NumberJs GrowAfter => new($"{value}.GrowAfter");
    public NumberJs MinBetween => new($"{value}.MinBetween");
}


[ScriptBefore]
public sealed class Space: Hierarchical<SpaceJs>, IBlock {
    public Binding<NumberJs> GrowBefore { set; protected get; } = null!;
    public Binding<NumberJs> GrowBetween { set; protected get; } = null!;
    public Binding<NumberJs> GrowAfter { set; protected get; } = null!;
    public Binding<NumberJs> MinBetween { set; protected get; } = null!;
    public Space([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        : base(callerFilePath, callerLineNumber) { }


    public override void AddRequiredInclues(IIncludes includes) {
        base.AddRequiredInclues(includes);
        includes.Require(new Script(ThisFilePathWithNewExtension("js")));
    }
    public Task<Tag> GenerateHtmlAsync(Context context) {
        AddRequiredInclues(context.Includes);
        return Task.FromResult(new Tag("ws") {
            CreateScriptBefore()
        });
    }


}
