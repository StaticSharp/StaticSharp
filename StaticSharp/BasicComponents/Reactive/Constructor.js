function _deleteScript() {
    let script = document.currentScript
    let parent = script.parentElement
    parent.removeChild(script)
    return parent
}

function Constructor() {
    var element = _deleteScript()
    for (let i of arguments) {
        i(element)
    }

    if (element.parentElement) {
        if (element.parentElement.tagName == "OVERLAY") {
            let overlay = element.parentElement
            let parent = overlay.parentElement
            //parent.removeChild(overlay)
            //document.body.appendChild(element)
            element.Parent = parent
            parent.Overlay = element
        } else {
            element.Parent = element.parentElement
        }
    }

    return element;
}

function Create(parent, ...constructors) {
    let primary = constructors[0]
    let tagName = primary.name.toLowerCase()
    
    let element = document.createElement(tagName)
    parent.appendChild(element)
    for (let i of constructors) {
        i(element)
    }
    return element;
    
}