StaticSharpClass("StaticSharp.Hover", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        Value: false,
    }

    new Reaction(() => {
        if (!element.Exists) {
            modifier.Value = false
        }
    })

    element.addEventListener("mouseenter", () => modifier.Value = true, false)
    element.addEventListener("mouseleave", () => modifier.Value = false, false)
})