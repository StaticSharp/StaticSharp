using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp;

public sealed class Modifier: Element, IElementCollector<IElement> {


    public float? FontSize = null;
    public Color? ForgroundColor = null;
    public Space? DefaultSpace = null;

    public Font? Font = null;
    private List<IElement> children { get; } = new();
    public Modifier Children => this;
    public void AddElement(IElement value) {
        children.Add(value);
    }

    public Modifier([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        : base(callerFilePath, callerLineNumber) { }

    public override async Task<Tag> GenerateHtmlAsync(Context context) {
        return new Tag("m") {
            await Task.WhenAll(children.Select(x=>x.GenerateHtmlAsync(context)))
        };
    }
}
