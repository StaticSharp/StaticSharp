StaticSharpClass("StaticSharp.Modifier", (modifier, element) => {
    StaticSharp.Entity(modifier)

    let baseAs = modifier.as
    modifier.as = function (typeName) {
        

        let result = baseAs(typeName)
        if (result != undefined)
            return result

        let oldAs = modifier.as
        modifier.as = () => undefined
        result = as(element, typeName)
        modifier.as = oldAs
        return result
    }



    modifier.Reactive = {
        Enabled: true
    }

})