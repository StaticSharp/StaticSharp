Object.defineProperty(Object.prototype, "Events", {
    get: function () {

        const prefix = "___"

        return new Proxy(
            this,
            {
                get(target, name, receiver) {
                    return target[prefix + name]
                },
                set: function (target, name, value, receiver) {
                    name = name.toLowerCase()

                    function add(event) {
                        target[prefix + name] = event
                        target.addEventListener(name, event.handler, event)
                    }

                    if (target.hasOwnProperty(prefix + name)) {
                        let event = target[prefix + name]
                        if (event !== undefined)
                            target.removeEventListener(name, event.handler)
                    }

                    //console.log("set", target, name, typeof (value), value)

                    if (value !== undefined) {

                        if (typeof (value) === "function") {
                            add({
                                handler: value,
                                capture: false,
                            })
                        } else if (typeof (value) === "object") {
                            if (typeof (value.handler) !== "function") {
                                console.error("If the value is an object, it must have a 'handler' field of type 'function'")
                            }
                            value.capture = false
                            add(value)
                        } else {
                            console.warn(`Type '${typeof(value)}' is invalid for event. To remove event handler assign 'undefined'`)
                        }
                    }
                }
            }
        )
    }/*,
    set: function (obj) {
    }*/
});