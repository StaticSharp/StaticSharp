
StaticSharpClass("StaticSharp.Menu", (element) => {
    StaticSharp.Block(element)

    CreateSocket(element, "Button", element)
    CreateSocket(element, "Popup", element)

    element.Reactive = {
        Width: () => element.Button.Layer.Width, 
        Height: () => element.Button.Layer.Height, 

        MarginTop: () => element.Button.Layer.MarginTop,
        MarginBottom: () => element.Button.Layer.MarginBottom,
        MarginLeft: () => element.Button.Layer.MarginLeft,
        MarginRight: () => element.Button.Layer.MarginRight,

        Open: false
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.Button
        const popup = element.Popup
        if (popup.Exists)
            yield element.Popup
        yield* element.UnmanagedChildren
    })


    new Reaction(() => {
        if (element.Open)
            element.classList.add("open")
        else
            element.classList.remove("open");
    })


    element.addEventListener("click", () => {
        element.Open = !element.Open
        event.stopPropagation()
    }, false)



    new Reaction(() => {
        element.Button.Layer.X = 0
        element.Button.Layer.Y = 0
        element.Button.Layer.Width = () => element.Width
        element.Button.Layer.Height = () => element.Height
    })

    new Reaction(() => {
        element.Popup.Layer.Exists = () => element.Open
    })




    new Reaction(() => {
        if (!element.Popup.Exists)
            return

        element.Popup.Layer.X = element.Button.Width - element.Popup.Width
        element.Popup.Layer.Y = element.Button.Height + (element.Popup.MarginTop || 0)
        element.Popup.Layer.Depth = 2
    })


})
