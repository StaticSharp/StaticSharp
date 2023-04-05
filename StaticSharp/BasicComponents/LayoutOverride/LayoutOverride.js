function LayoutOverride(element) {
    Block(element)
    element.isLayoutOverride = true

    CreateSocket(element, "Child", element)

    element.Reactive = {
        Width: () => element.Child.Layer.Width, 
        Height: () => element.Child.Layer.Height, 

        // margins cannot be overriden, but must be translated outside
        MarginTop: () => element.Child.Layer.MarginTop,
        MarginBottom: () => element.Child.Layer.MarginBottom,
        MarginLeft: () => element.Child.Layer.MarginLeft,
        MarginRight: () => element.Child.Layer.MarginRight,

        Child: undefined,

        OverrideX: undefined,
        OverrideY: undefined,
        OverrideWidth: undefined,
        OverrideHeight: undefined,
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.Child
        yield* element.UnmanagedChildren
    })

    new Reaction(() => {
        element.Child.Layer.X = element.OverrideX != undefined ? element.OverrideX - element.X : 0
        element.Child.Layer.Y = element.OverrideY != undefined ? element.OverrideY - element.Y : 0

        element.Child.Layer.Width = element.OverrideWidth || element.Width
        element.Child.Layer.Height = element.OverrideHeight || element.Height
    })
}
