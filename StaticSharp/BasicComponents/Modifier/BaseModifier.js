
function BaseModifier(element) {

    Hierarchical(element)

    element.isModifier = true

    element.Reactive = {
        Hover: false,
        Selectable: undefined,

        BackgroundColor: undefined,
        HierarchyBackgroundColor: () => element.BackgroundColor || element.Parent.HierarchyBackgroundColor,

        ForegroundColor: () => {            
            if (element.BackgroundColor == undefined)
                return undefined
            return element.BackgroundColor.contrastColor()
        },
        HierarchyForegroundColor: () => element.ForegroundColor || element.Parent.HierarchyForegroundColor,


        FontSize: undefined,
        HierarchyFontSize: () => element.FontSize || element.Parent.HierarchyFontSize,

        Radius: undefined,
        RadiusTopLeft: () => element.Radius,
        RadiusTopRight: () => element.Radius,
        RadiusBottomLeft: () => element.Radius,
        RadiusBottomRight: () => element.Radius,

        Visibility: 1
    }

    new Reaction(() => {
        let visibility = element.Visibility
        if (visibility == 0) {
            element.style.visibility = "hidden"
            element.style.opacity = ""
        } else if (visibility == 1) {
            element.style.visibility = ""
            element.style.opacity = ""
        } else {
            element.style.opacity = ""
            element.style.visibility = visibility
        }        
    })


    new Reaction(() => {
        element.style.borderTopLeftRadius       = ToCssSize(element.RadiusTopLeft)
        element.style.borderTopRightRadius      = ToCssSize(element.RadiusTopRight)
        element.style.borderBottomLeftRadius    = ToCssSize(element.RadiusBottomLeft)
        element.style.borderBottomRightRadius   = ToCssSize(element.RadiusBottomRight)
    })



    new Reaction(() => {
        let selectable = element.Selectable
        if (selectable!=undefined)
            element.style.userSelect = element.Selectable ? "text" : "none"
        else
            element.style.userSelect = ""
    })
    

    new Reaction(() => {
        if (element.BackgroundColor) {
            element.style.backgroundColor = element.BackgroundColor
        }
    })

    new Reaction(() => {
        if (element.ForegroundColor) {
            element.style.color = element.ForegroundColor
        }
    })
}
