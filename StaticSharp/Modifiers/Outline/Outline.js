StaticSharpClass("StaticSharp.Outline", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        Enabled: true,
        Style: "Solid",
        Color: () => element.HierarchyForegroundColor,
        Width: 1,
        Offset: 0        
    }
    

    new Reaction(() => {
        var last = element.Modifiers.findLast(x => x.is("StaticSharp.Outline") && x.Enabled)
        if (last == modifier) {
            element.style.outlineStyle = modifier.Style.toLowerCase()
            element.style.outlineColor = modifier.Color.toString()
            element.style.outlineWidth = `${modifier.Width}px`
            element.style.outlineOffset = `${modifier.Offset}px`
        }
    })
})