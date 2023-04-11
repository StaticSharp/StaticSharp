function Entity(element) {
    element.isEntity = true
    element.as = function (typeName) {
        if (element.is(typeName))
            return element
        return undefined
    }

    element.is = function (typeName) {
        return element["is" + typeName] != undefined
    }
}