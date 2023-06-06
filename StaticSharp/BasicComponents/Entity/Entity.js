
StaticSharpClass("StaticSharp.Entity",(element) => {
    element.as = function (typeName) {
        if (element.is(typeName))
            return element
        return undefined
    }

    element.is = function (fullTypeName) {
        //console.log(fullTypeName, element.types)


        return element.types.includes(fullTypeName)
    }
})
