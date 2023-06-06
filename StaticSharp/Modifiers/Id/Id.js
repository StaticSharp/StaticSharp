StaticSharpClass("StaticSharp.Id", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        Value: undefined,
    }

    new Reaction(() => {
        var value = modifier.Value || ""
        element.id = value
    })
})