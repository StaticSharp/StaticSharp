
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




function BaseModifierInitialization(element) {
    HierarchicalInitialization(element)
    element.isModifier = true

    element.Reactive = {
        FontSize: undefined,
        HierarchyFontSize: () => GetModifierHierarchyProperty(element, "FontSize")
    }
}

function BaseModifierBefore(element) {
    HierarchicalBefore(element)
    let parent = element.parentElement;

    //let m = GetModifierHierarchyProperty(element, "FontSize")//.find(e => e.FontSize)
    console.log(element, element.HierarchyFontSize, element.FontSize)
    new Reaction(() => {
        element.style.fontSize = element.FontSize + "px"
    })

}

function BaseModifierAfter(element) {
}