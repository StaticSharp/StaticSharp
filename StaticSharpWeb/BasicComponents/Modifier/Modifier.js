function ModifierInitialization(element) {
    BaseModifierInitialization(element)
}

function ModifierBefore(element) {

    BaseModifierBefore(element)

    let parent = element.parentElement;
    element.AddChild = function (child) {
        if (parent.AddChild)
            parent.AddChild(child)
    }

}

function ModifierAfter(element) {
    BaseModifierAfter(element)
}