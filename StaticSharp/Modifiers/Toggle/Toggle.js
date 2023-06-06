StaticSharpClass("StaticSharp.Toggle", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        Value: false,
    }

    element.addEventListener("click", () => {modifier.Value = !modifier.Value}, false)
})