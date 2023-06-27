StaticSharpClass("StaticSharp.UserSelect", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        Option: "None"
    }

    new Reaction(() => {
        var option = modifier.Option
        
        if ((option === undefined) || (option === ""))
            element.style.userSelect = ""
        else
            element.style.userSelect = CamelToKebab(option);
    })
})