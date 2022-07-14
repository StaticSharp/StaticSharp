
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
    if (!element) return undefined
    if (element.isModifier) {
        return element;
    }
    return GetModifier(element.parentElement)
}

function BaseModifierInitialization(element) {
    HierarchicalInitialization(element)
    element.isModifier = true

    element.Reactive = {
        FontSize: undefined,
        BackgroundColor: undefined,
        HierarchyFontSize: () => element.FontSize || element.Modifier.HierarchyFontSize
        
    }
}

function BaseModifierBefore(element) {
    HierarchicalBefore(element)
    let parent = element.parentElement;


    new Reaction(() => {
        element.style.fontSize = ToCssSize(element.FontSize)
    })

}

function BaseModifierAfter(element) {
    HierarchicalAfter(element)

    
}