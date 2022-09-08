function _deleteScript() {
    let script = document.currentScript
    let parent = script.parentElement
    parent.removeChild(script)
    return parent
}

function _call(name, element) {
    var func = window[name]
    if (!func)
        throw new Error(`function ${name} not found`)
    func(element)
}

function Constructor(name) {
    var element = _deleteScript()
    _call(name, element)
    return element;
}
