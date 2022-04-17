function _deleteScript() {
    let script = document.currentScript
    let parent = script.parentElement
    parent.removeChild(script)
    return parent
}

function ConstructorBefore(name) {
    var parent = _deleteScript()
    window[name + "Initialization"](parent)


    window[name + "Before"](parent)
}


function ConstructorAfter(name) {
    var parent = _deleteScript()
    window[name + "After"](parent)
}