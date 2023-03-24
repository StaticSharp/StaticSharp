function LayoutOverride(element) {
    Block(element)
    element.isOverrider = true

    CreateSocket(element, "Target", element)

    element.Reactive = {
        Width: () => element.Target.Layer.Width, 
        Height: () => element.Target.Layer.Height, 

        // margins cannot be overriden, but must be translated outside
        MarginTop: () => element.Target.Layer.MarginTop,
        MarginBottom: () => element.Target.Layer.MarginBottom,
        MarginLeft: () => element.Target.Layer.MarginLeft,
        MarginRight: () => element.Target.Layer.MarginRight,

        Target: undefined,

        OverrideX: undefined,
        OverrideY: undefined,
        OverrideWidth: undefined,
        OverrideHeight: undefined,

        OverridePaddingLeft: undefined,
        OverridePaddingRight: undefined,
        OverridePaddingTop: undefined,
        OverridePaddingBottom: undefined
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.Target,
        yield* element.Children
    })

    // Doesn't work for X,Y and Margins
    function translatePropertyValue(propertyName, overridePropertyName) {
        let finalValue = element[overridePropertyName] || element[propertyName]
        if (finalValue != undefined) {
            element.Target.Layer[propertyName] = finalValue
        }

        // Looks potentially useful
        //if (element[overridePropertyName] != undefined) {
        //    element[propertyName] = element[overridePropertyName]
        //}
    }

    new Reaction(() => {
        element.Target.Layer.X = element.OverrideX != undefined ? element.OverrideX - element.X : 0
        element.Target.Layer.Y = element.OverrideY != undefined ? element.OverrideY - element.Y : 0

        translatePropertyValue("Width", "OverrideWidth")
        translatePropertyValue("Height", "OverrideHeight")

        translatePropertyValue("PaddingLeft", "OverridePaddingLeft")
        translatePropertyValue("PaddingRight", "OverridePaddingRight")
        translatePropertyValue("PaddingTop", "OverridePaddingTop")
        translatePropertyValue("PaddingBottom", "OverridePaddingBottom")
    })
}
