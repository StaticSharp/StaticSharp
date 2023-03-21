function Overrider(element) {
    Block(element)
    element.isOverrider = true

    CreateSocket(element, "Target", element)

    element.Reactive = {
        InternalWidth: undefined,
        InternalHeight: undefined,

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,
        
        Target: undefined,
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.Target,
        yield* element.Children
    })

    //let overrideJson = element.dataset.override.replace(/'/g, '"');// replaceAll("'", '"');
    //console.log("overrideJson", overrideJson)
    //let override = JSON.parse(overrideJson)
    //console.log("override", override)

    //new Reaction(() => {
        //let propertiesList = []
        //for (let memberName of Object.getOwnPropertyNames(element.Target)) {
        //    if (element.Target[memberName] instanceof Property && memberName.indexOf("__") == 0) {
        //        let propertyName = memberName.substring(2)
        //        //console.log(propertyName, /*override[propertyName], */element[propertyName], element.Target.Layer[propertyName], element.Target[propertyName])

        //        if (element.Layer[propertyName] != undefined && !(element.Target[propertyName] instanceof Object)) {
        //            element.Target.Layer[propertyName] = element.Layer[propertyName]
        //            console.log(`Set ${propertyName} = ${element.Layer[propertyName]}`)
        //        }



        //        //element.Target.Layer[propertyName] = /*override[propertyName] /*|| */element.Layer[propertyName] || element.Target.Layer[propertyName]
        //        //console.log(element.Target[propertyName])


        //    }
        //}
    //})

    new Reaction(() => {

        element.Target.Layer.X = element.OverrideX
        element.Target.Layer.Y = element.OverrideY
        element.Target.Layer.Width = element.OverrideWidth || element.Width
        element.Target.Layer.Height = element.OverrideHeight || element.Height

        //if (element.OverridePaddingLeft != undefined) {
        //    element.Target.Layer.PaddingLeft = element.OverridePaddingLeft - (element.PaddingLeft || 0)
        //    //element.PaddingLeft = undefined
        //} else {
        //    element.Target.PaddingLeft = undefined // hack
        //    element.PaddingLeft = element.Target.PaddingLeft
        //}

        //element.Target.Layer.PaddingLeft = element.OverridePaddingLeft || element.PaddingLeft
        //element.Target.Layer.PaddingRight = element.OverrideHeight || element.PaddingRight
        //element.Target.Layer.PaddingTop = element.OverridePaddingTop || element.PaddingTop
        //element.Target.Layer.PaddingBottom = element.OverridePaddingBottom || element.PaddingBottom

        element.Target.Layer.BackgroundColor = element.OverrideBackgroundColor || element.BackgroundColor
    })

    new Reaction(() => {
        element.InternalWidth = element.Target.Layer.Width
        element.InternalHeight = element.Target.Layer.Height

        element.MarginTop = element.Target.Layer.MarginTop
        element.MarginBottom = element.Target.Layer.MarginBottom
        element.MarginLeft = element.Target.Layer.MarginLeft
        element.MarginRight = element.Target.Layer.MarginRight
    })
}
