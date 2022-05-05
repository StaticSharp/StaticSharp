using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp;

public sealed class Space: Reactive {

    public float? GrowBefore = null;
    public float? GrowBetween = null;
    public float? GrowAfter = null;

    public float? MinBetween = null;

    public Space([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        : base(callerFilePath, callerLineNumber) { }

    public override Task<Tag> GenerateHtmlAsync(Context context) {
        return Task.FromResult(new Tag("ws") { " "});
    }
}
