function CreateLayer(element) {

    let originalProperties = new Map()

    let result = new Proxy(element,
        {
            get(target, prop, receiver) {
                if (prop == "originalProperties") { // TODO: think of better implementation
                    return originalProperties
                }

                if (originalProperties.has(prop)) {

                    return originalProperties.get(prop).getValue()
                }

                return target[prop]
            },

            set(target, prop, value, receiver) {
                if (!target.hasOwnProperty(prop)) {
                    throw new Error(`Element does not have property "${prop}"`);
                }

                if (!originalProperties.has(prop)) {
                    let currentProperty = target["__" + prop]
                    let propertyValueOrBinding = currentProperty.binding ? currentProperty.binding.func : currentProperty.value
                    let backupProperty = new Property()
                    backupProperty.name = currentProperty.name // TODO: ???
                    backupProperty.object = currentProperty.object // TODO: needed to execute binding
                    backupProperty.setValue(propertyValueOrBinding)
                    originalProperties.set(prop, backupProperty)
                }

                target[prop] = value
                return true;
            }
        }
    );

    return result
    //element.Layer = result
}


function ClearLayer(element) {
    if (!element.Layer || !element.Layer.originalProperties) {
        // Layer could be set to element itself and has not originalProperties
        return
    }

    let d = Reaction.beginDeferred()
    element.Layer.originalProperties.forEach(function (backupProperty, key, map) {
        let propertyValueOrBinding = backupProperty.binding ? backupProperty.binding.func : backupProperty.value
        element[key] = propertyValueOrBinding
    })
    element.Layer.originalProperties.clear()
    d.end()
}