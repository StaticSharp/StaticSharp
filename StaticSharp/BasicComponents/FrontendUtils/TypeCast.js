function as(element, typeName) {
    if (element == undefined)
        return undefined

    if (typeof (element.as) == "function") {
        return element.as(typeName)
    }

    return undefined
}


function convert(element, typeName) {
    var result = as(element, typeName)

    if (result == undefined) {
        let message = `Failed to convert element to type ${typeName}`
        console.error(message, element)
        throw new Error(message)
    }

    return result
}