StaticSharpClass("StaticSharp.Button", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        EventPropagation: false
    }

    element.addEventListener("click", () => {
        modifier.Script(modifier)
        if (!modifier.EventPropagation)
            event.stopPropagation()
    }, false)

})