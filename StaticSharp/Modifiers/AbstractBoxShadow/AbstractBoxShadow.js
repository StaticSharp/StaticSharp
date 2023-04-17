
function AbstractBoxShadow(element) {
    Modifier.call(this, element)
    this.isAbstractBoxShadow = true
    let modifier = this

    modifier.getBoxShadow = function () {
        throw Error("getBoxShadow not implemented")
    }
    

    new Reaction(() => {
        var boxShadowModifiers = element.Modifiers.Where(x => x.isAbstractBoxShadow)
        if (boxShadowModifiers.First() == modifier) {
            let shadow = boxShadowModifiers.Select(x => x.getBoxShadow()).ToArray().join(',')
            element.style.boxShadow = shadow
        }
    })

}