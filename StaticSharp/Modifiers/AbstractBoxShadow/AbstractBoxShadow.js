StaticSharpClass("StaticSharp.AbstractBoxShadow", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.getBoxShadow = function () {
        throw Error("getBoxShadow not implemented")
    }    

    new Reaction(() => {

        try {
            var coModifiers = element.Modifiers.filter(x => x.is("StaticSharp.AbstractBoxShadow"))
        } catch {
            console.error("8888")
            console.log(element.Modifiers[4])
        }



        if (coModifiers[0] == modifier) {
            let shadow = coModifiers.map(x => x.getBoxShadow()).join(',')
            element.style.boxShadow = shadow
        }
    })

})