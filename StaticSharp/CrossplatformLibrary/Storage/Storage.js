var Storage = {}

Storage.Elements = {}

Storage.Restore = function (name, getter) {

    var valueString = localStorage.getItem(name)

    if (Storage.Elements[name] == undefined) {
        Storage.Elements[name] = new Reaction(() => {
            var newValue = getter()
            localStorage.setItem(name, typeof (newValue) + " " + newValue)
        })
    }

    if (valueString != null) {
        var seperator = valueString.indexOf(' ')
        var type = valueString.substring(0, seperator)
        var value = valueString.substring(seperator + 1)

        if (type == "undefined") {
            return undefined
        }
        if (type == "number") {
            return Number(value)
        }
        if (type == "boolean") {
            return (value === 'true')
        }
        if (type == "string") {
            return value
        }

    } else {
        return undefined
    }

    

}