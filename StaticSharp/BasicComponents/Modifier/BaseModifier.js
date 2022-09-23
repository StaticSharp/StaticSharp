
function BaseModifier(element) {

    Hierarchical(element)

    element.isModifier = true

    element.Reactive = {

        Selectable: undefined,

        BackgroundColor: undefined,
        ForegroundColor: () => {            
            if (element.BackgroundColor == undefined)
                return undefined
            return element.BackgroundColor.contrastColor()
        },

        FontSize: undefined,
        HierarchyFontSize: () => element.FontSize || element.Parent.HierarchyFontSize,
    }


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
