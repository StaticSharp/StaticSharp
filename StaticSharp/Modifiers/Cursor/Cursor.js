StaticSharpClass("StaticSharp.Cursor", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        Option: "Pointer",
    }

    new Reaction(() => {
        var option = modifier.Option
        if ((option === undefined) || (option === ""))
            element.style.cursor = ""
        else
            element.style.cursor = CamelToKebab(option);
    })
})