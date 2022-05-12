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

function ConstructorBefore(name, script) {
    var element = _deleteScript()

    _call(name + "Initialization",element)

    //let script = "";//"element.X = () => element.LayoutX+10"
    eval(script)
    _call(name + "Before", element)
}


function ConstructorAfter(name) {
    var element = _deleteScript()
    _call(name + "After", element)
}