function Modifier(element) {
    Entity(this)
    this.isModifier = true
    let modifier = this


    let baseAs = this.as
    this.as = function (typeName) {
        let result = baseAs(typeName)
        if (result != undefined)
            return result

        let oldAs = this.as
        this.as = () => undefined
        result = as(element, typeName)
        this.as = oldAs
        return result
    }



    modifier.Reactive = {
        Enabled: true
    }

}