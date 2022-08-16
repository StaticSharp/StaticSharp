
function GetModifierHierarchyProperty(element, propertyName) {
    if (!element) return undefined
    if (element.isModifier) {
        let property = element[propertyName]
        if (property != undefined) {
            return property
        }
    }
    return GetModifierHierarchyProperty(element.parentElement, propertyName)
}


function GetModifier(element) {
    //console.log("GetModifier", element)
    if (!element) return undefined
    if (element.isModifier) {
        return element;
    }
    return GetModifier(element.parentElement)
}




function BaseModifier(element) {
    //console.log("element.Modifier",element.Modifier)
    if (element.isModifier)
        return
    //Hierarchical(element)
    element.isModifier = true

    element.Reactive = {
        Modifier: element,
        //FontSize: undefined,
        //BackgroundColor: undefined,
        ForegroundColor: () => {            
            if (element.BackgroundColor == undefined)
                return undefined
            return element.BackgroundColor.contrastColor()
        },
        HierarchyFontSize: () => element.FontSize || element.ParentModifier.HierarchyFontSize
        
    }

    new Reaction(() => {
        element.style.fontSize = ToCssSize(element.FontSize)
    })

    new Reaction(() => {
        if (element.BackgroundColor) {
            element.style.backgroundColor = element.BackgroundColor// element.BackgroundColor.toString(16)
        }
    })

    new Reaction(() => {
        if (element.ForegroundColor) {
            element.style.color = element.ForegroundColor// element.BackgroundColor.toString(16)
        }
    })
}
