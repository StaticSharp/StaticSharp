StaticSharpClass("StaticSharp.AbstractBackdropFilter", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.getFilter = function () {
        throw Error("getFilter not implemented")
    }    

    new Reaction(() => {
        var coModifiers = element.Modifiers.filter(x => x.is("StaticSharp.AbstractBackdropFilter"))
        if (coModifiers[0] == modifier) {            
            let filter = coModifiers.filter(x => x.Enabled).map(x => x.getFilter()).join(' ')
            element.style.backdropFilter = filter
        }
    })

})