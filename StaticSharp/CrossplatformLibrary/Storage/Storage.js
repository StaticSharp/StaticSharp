var Storage = {}

Storage.Elements = {}

Storage.Restore = function (name, getter) {
    let storage = sessionStorage

    let valueString = storage.getItem(name)

    if (Storage.Elements[name] == undefined) {
        Storage.Elements[name] = new Reaction(() => {
            let newValue = getter()
            storage.setItem(name, typeof (newValue) + " " + newValue)
        })
    }

    if (valueString != null) {
        let seperator = valueString.indexOf(' ')
        let type = valueString.substring(0, seperator)
        let value = valueString.substring(seperator + 1)

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