/**
 * Creates Layer for DOM element - a removable proxy to override elemetn's properties
 * @param {Element} element
 * @return {Proxy}
 */
function CreateLayer(element) {

    let backupProperties = new Map()

    /*let originalPropertiesView = new Proxy
    Object.defineProperty(originalPropertiesView, "A", {
        get: function () {
            return "B"
        }

    })*/

    function createBackupProperty(property) {
        //let currentProperty = target["__" + prop]
        let hasBinding = property.binding != undefined

        let propertyValueOrBinding = hasBinding ? property.binding.func : property.value
        let backupProperty = new Property()
        backupProperty.name = "Backup_" + property.name // TODO: ???
        backupProperty.object = property.object // TODO: needed to execute binding
        backupProperty.setValue(propertyValueOrBinding)        

        return backupProperty
    }

    function getBackupProperty(target, propertyName) {
        let backupProperty = backupProperties.get(propertyName)
        if (backupProperty == undefined) {
            if (!Property.exists(target, propertyName) ) {
                throw new Error(`Element does not have property "${propertyName}"`)
            }

            backupProperty = createBackupProperty(target.Reactive[propertyName])
            backupProperties.set(propertyName, backupProperty)
        }
        return backupProperty
    }


    let result = new Proxy(element,
        {
            //originalPropertiesView: originalPropertiesView,
            //originalProperties: originalProperties,


            get(target, propertyName, receiver) {
                if (propertyName == "backupProperties") { // TODO: think of better implementation
                    return backupProperties
                }

                var backupProperty = getBackupProperty(target, propertyName)
                
                let value = backupProperty.getValue()
                //console.log(value,backupProperty)

                return value
            },

            set(target, propertyName, value, receiver) {


                var backupProperty = getBackupProperty(target, propertyName)

                target[propertyName] = value
                //backupProperty.setValue(value)
                return true
            }
        }
    )

    
    
    return result
    //element.Layer = result
}

/**
 * Removes all properties overrides from layer
 * @param {Element} element
 */
function ClearLayer(element) {
    if (!element.Layer || !element.Layer.backupProperties) {
        // Layer could be set to element itself and has not originalProperties
        return
    }

    let d = Reaction.beginDeferred()
    element.Layer.backupProperties.forEach(function (backupProperty, key, map) {
        let propertyValueOrBinding = backupProperty.binding ? backupProperty.binding.func : backupProperty.value
        element[key] = propertyValueOrBinding
    })
    element.Layer.backupProperties.clear()

    d.end()
}

// TODO: finalize layer internface

/**
 * Clears element's layer if it exists or attach new clean layer to an element if layer doesn't exsit
 * @param {Element} element
 */
function CreateOrClearLayer(element) {
    if (element.Layer != undefined) {
        ClearLayer(element)
    } else {
        element.Layer = CreateLayer(element)  // TODO:
    }
}