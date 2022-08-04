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

function ConstructorInitialization(name) {
    var element = _deleteScript()
    _call(name + "Initialization", element)
    return element;
    /*try {
        eval(script)
    } catch (e) {
        console.error(script, "\n", e)
    }*/
}

function ConstructorBefore(name) {
    var element = _deleteScript()
    _call(name + "Before", element)
}


function ConstructorAfter(name) {
    var element = _deleteScript()
    _call(name + "After", element)
}