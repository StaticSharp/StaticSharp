
function AbstractBoxShadow(element) {
    Modifier.call(this, element)
    this.isAbstractBoxShadow = true
    let modifier = this

    modifier.getBoxShadow = function () {
        throw Error("getBoxShadow not implemented")
    }
    

    new Reaction(() => {
        var coModifiers = element.Modifiers.filter(x => x.isAbstractBoxShadow)
        if (coModifiers[0] == modifier) {
            let shadow = coModifiers.map(x => x.getBoxShadow()).join(',')
            element.style.boxShadow = shadow
        }
    })

}