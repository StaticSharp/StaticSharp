function Layer(element) {
    this.originalProperties = new Map()

    new Proxy(this,
        {
            get(target, prop, receiver) {
                if (this.originalProperties.has(prop)) {
                    return this.originalProperties.get(prop)
                }

                return element[prop]
            },

            set(obj, prop, value) {
                if (!element.hasOwnPropery(prop)) {
                    throw new Error(`Element does not have property "${prop}"`);
                }

                if (!this.originalProperties.has(prop)) {
                    let currentProperty = element["__" + prop]
                    let propertyValueOrBinding = currentProperty.binding ? currentProperty.binding.func : currentProperty.value
                    let backupProperty = new Property()
                    backupProperty.setValue(propertyValueOrBinding)
                    this.originalProperties.set(prop, backupProperty)
                }

                element[prop] = value
                return true;
            }
        }
    );
}

Layer.prototype.clear = function () {
    let d = Reaction.beginDeferred()
    this.originalProperties.forEach(function (value, key, map) {
        this.element[key] = value
    })
    this.originalProperties.clear()
    d.end()
}