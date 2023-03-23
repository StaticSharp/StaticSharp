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




                if (!target.hasOwnProperty(prop)) {
                    throw new Error(`Element does not have property "${prop}"`)
                }

                /*if (value == undefined) // if override value = undefined, then remove override, use original value
                {
                    if (originalProperties.has(prop)) {
                        let backupProperty = originalProperties.get(prop)
                        let currentProperty = target["__" + prop]
                        let propertyValueOrBinding = backupProperty.binding ? backupProperty.binding.func : backupProperty.value
                        currentProperty.setValue(propertyValueOrBinding)
                        originalProperties.delete(prop)
                    }

                    return true
                }*/

                if (!originalProperties.has(prop)) {
                    let currentProperty = target["__" + prop]
                    let currentPropertyHasBinding = currentProperty.binding != undefined

                    
                    let propertyValueOrBinding = currentPropertyHasBinding ? currentProperty.binding.func : currentProperty.value
                    let backupProperty = new Property()
                    backupProperty.name = "Backup_"+currentProperty.name // TODO: ???
                    backupProperty.object = currentProperty.object // TODO: needed to execute binding
                    backupProperty.setValue(propertyValueOrBinding)

                    //Copy subscriptions
                    if (currentPropertyHasBinding) {
                        
                        for (let p of currentProperty.binding.triggeringProperties) {
                            
                            backupProperty.binding.addTriggeringProperty(p)

                            //console.log(p, [...backupProperty.binding.triggeringProperties])
                        }
                    }

                    //backupProperty.getValue() //To establish subscriptions

                    originalProperties.set(prop, backupProperty)

                    if (Reaction.current) {
                        if (Reaction.current.unsubscribeFromTriggeringProperty(currentProperty)) {
                            //console.log("addTriggeringProperty", backupProperty)
                            Reaction.current.addTriggeringProperty(backupProperty)
                        }                        
                    }
                }

                target[prop] = value
                return true
            }
        }
    )

    
    
    return result
    //element.Layer = result
}


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