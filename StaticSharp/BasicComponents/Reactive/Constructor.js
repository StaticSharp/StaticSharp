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
    return element;
}
