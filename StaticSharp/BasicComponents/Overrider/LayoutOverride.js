function LayoutOverride(element) {
    Block(element)
    element.isOverrider = true

    CreateSocket(element, "Content", element)

    element.Reactive = {
        Width: () => element.Content.Layer.Width, 
        Height: () => element.Content.Layer.Height, 

        // margins cannot be overriden, but must be translated outside
        MarginTop: () => element.Content.Layer.MarginTop,
        MarginBottom: () => element.Content.Layer.MarginBottom,
        MarginLeft: () => element.Content.Layer.MarginLeft,
        MarginRight: () => element.Content.Layer.MarginRight,

        Content: undefined,

        OverrideX: undefined,
        OverrideY: undefined,
        OverrideWidth: undefined,
        OverrideHeight: undefined,
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.Content,
        yield* element.Children
    })

    new Reaction(() => {
        element.Content.Layer.X = element.OverrideX != undefined ? element.OverrideX - element.X : 0
        element.Content.Layer.Y = element.OverrideY != undefined ? element.OverrideY - element.Y : 0

        element.Content.Layer.Width = element.OverrideWidth || element.Width
        element.Content.Layer.Height = element.OverrideHeight || element.Height
    })
}
