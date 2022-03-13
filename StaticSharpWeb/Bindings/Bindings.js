




function ReactionBase(func) {
    this.dependencies = new Set()
    this.func = func

    this.addDependency = function (property) {
        this.dependencies.add(property)
        property.onChanged.add(this.onChangedHandler)
    }

    this.unsubscribe = function () {
        if (this.dependencies.size == 0) return

        for (let dependency of this.dependencies) {
            dependency.onChanged.delete(this.onChangedHandler)
        }
        this.dependencies.clear()
    }

    this.execute = function () {
        let oldReaction = Reaction.current
        Reaction.current = this
        this.unsubscribe()


        let result = this.func()

        Reaction.current = oldReaction
        return result
    }
}


function Reaction(func) {

    ReactionBase.call(this, func)

    let _this = this
    this.onChangedHandler = function () {
        Reaction.deferred.add(_this)
    }

    this.execute()
}
Reaction.prototype = Object.create(ReactionBase.prototype)
Reaction.prototype.constructor = Reaction

Reaction.beginDeferred = function () {
    if (!Reaction.deferred) {
        Reaction.deferred = new Set()
        return {
            end: function () {
                while (Reaction.deferred.size > 0) {
                    //console.log("Reaction.deferred.end", Reaction.deferred.size)
                    let d = Array.from(Reaction.deferred)
                    for (let reaction of d) {
                        reaction.execute()
                        Reaction.deferred.delete(reaction)
                    }

                    //d.forEach(x => x.execute())
                    //Reaction.deferred.clear()
                }
                //let l = Reaction.deferred;
                Reaction.deferred = undefined
                //l.forEach(x => x.execute())
            }
        };
    }
    return undefined;
}


function Binging(func, onChange) {
    //console.log("function Binging(func, onChange)", onChange)
    ReactionBase.call(this, func)

    this.dirty = true
    this.onChange = onChange


    let _this = this
    this.onChangedHandler = function () {
        _this.dirty = true
        _this.onChange()
    }
}
Binging.prototype = Object.create(ReactionBase.prototype);
Binging.prototype.constructor = Binging;


function Property(value) {
    this.onChanged = new Set()
    this.binding = undefined

    _this = this
    let onDependencyChanged = function () {
        _this.onChanged.forEach(x => x())
    }


    this.getValue = function() {
        //console.log(`getValue`)
        if (Reaction.current) {
            Reaction.current.addDependency(this)
        }
        if (this.binding) { //wechat if (this.binding?.dirty)
            if (this.binding.dirty) {
                this.value = this.binding.execute()
                this.binding.dirty = false
            }
        }
        return this.value
    }

    this.setValue = function(value) {
        //console.log("setValue " + value + " " + this.onChanged.size)
        if (typeof value === 'function') {
            if (this.binding) {
                if (this.binding.func === value) {
                    return
                }
                this.binding.unsubscribe()
            }
            this.binding = new Binging(value, onDependencyChanged)

        } else {
            if (this.value === value)
                return
            if (this.binding) {
                this.binding.unsubscribe()
                this.binding = undefined
            }
            this.value = value
        }

        var d = Reaction.beginDeferred()
        this.onChanged.forEach(x => x())
        if (d)
            d.end()

    }

    this.attach = function(object, name) {
        let property = this
        let accessorDescriptor = {
            get: function () {
                return property.getValue()
            },
            set: function (value) {
                property.setValue(value)

            }
        }
        Object.defineProperty(object, name, accessorDescriptor);
        return this
    }

    
    this.setValue(value)


}



Property.exists = function (target, name) {
    var propertyDescriptor = !!Object.getOwnPropertyDescriptor(target, name)
    var propertyFieldExists = target.hasOwnProperty("__" + name)
    return propertyDescriptor && propertyFieldExists
}

Property.nameAvailable = function (target, name) {
    if (Object.getOwnPropertyDescriptor(target, name))
        return false
    if (target.hasOwnProperty(name))
        return false
    return true
}

Object.defineProperty(Object.prototype, "Reactive", {
    get: function () {
        return new Proxy(
            this,
            {
                get(target, name, receiver) {
                    //console.log(`get target:${JSON.stringify(target)} name: ${name}`)
                    return target["__" + name]
                    //return Reflect.get(...arguments);
                },
                set: function (target, name, value, receiver) {
                    //console.log("set",target,name,value)
                    
                    if (target.hasOwnProperty("__" + name)) {
                        const propertyField = target["__" + name]
                        propertyField.setValue(value)
                    } else {
                        //
                        target["__" + name] = new Property(value).attach(target,name)
                    }
                }
            }
            )
    },
    set: function (obj) {
        let proxy = this.Reactive
        //console.log(proxy)

        let d = Reaction.beginDeferred()
        //отложенное выполнение реакций не нужно при создании свойств
        //тут оно используется т.к. следующий код можнт не только создать свойство
        //но и присвоить значение существующему свойству
        if (obj instanceof Object) {
            for (const [key, value] of Object.entries(obj)) {
                proxy[key] = value                
            }
        }
        if (d) d.end()


    }
});


Property.prototype.OnChanged = function (func) {
    console.log(this)
    let previous = this.getValue()

    return new Reaction(() => {
        let current = this.getValue()
        if (current != previous) {
            func(previous, current)
            previous = current
        }
    })
}
/*
Property.prototype.OnTruthify = function (func) {
    let previous = this.getValue()

    return new Reaction(() => {
        let current = this.getValue()
        if (current && !previous) {
            func(previous, current)
            previous = current
        }
    })
}*/


/*function OnChanged(property) {


}*/

function PropertyTest() {

    console.group("PropertyTest")

    let root = {
        Field: 7

    }

    console.assert( Property.nameAvailable(root, "A") == true)
    console.assert( Property.nameAvailable(root, "Field") == false)


    console.assert( Property.exists(root, "A") == false)

    root.Reactive.A = 0

    console.assert( Property.exists(root, "A") == true)
    console.assert( Property.nameAvailable(root, "A") == false)

    console.assert(root.A == 0)
    root.Reactive.A = 8
    console.assert(root.A == 8)

    root.Reactive.B = ()=>root.A*2
    console.assert( root.B == 16)

    root.Reactive.A = 10
    console.assert( root.B == 20)


    root.Reactive = {
        C: 7,
        D: () => root.C
    }

    console.assert( Property.exists(root, "C") == true)
    console.assert( Property.exists(root, "D") == true)
    console.assert( root.D == 7)

    new Reaction(() => {
        root.Field = root.C
    })
    console.assert(root.Field == root.C)


    root.C = 9
    console.assert(root.Field == root.C)
    console.assert(root.D == root.C)


    root.Reactive.D = ()=>root.C+1



    console.groupEnd()
}






/*




class ReactionBase {
    static current = undefined
    dependencies = new Set()

    static deferred = undefined

    static beginDeferred() {
        if (!Reaction.deferred) {
            Reaction.deferred = new Set()
            return {
                end: function () {
                    while (Reaction.deferred.size > 0) {
                        //console.log("Reaction.deferred.end", Reaction.deferred.size)
                        let d = Array.from(Reaction.deferred)
                        for (let reaction of d) {
                            reaction.execute()
                            Reaction.deferred.delete(reaction)
                        }

                        //d.forEach(x => x.execute())
                        //Reaction.deferred.clear()
                    }
                    //let l = Reaction.deferred;
                    Reaction.deferred = undefined
                    //l.forEach(x => x.execute())
                }
            };
        }
        return undefined;
    }



    constructor(func) {
        this.func = func
    }


    addDependency(property) {
        this.dependencies.add(property)
        property.onChanged.add(this.onChangedHandler)
    }

    unsubscribe() {
        if (this.dependencies.size == 0) return

        for (let dependency of this.dependencies) {
            dependency.onChanged.delete(this.onChangedHandler)
        }
        this.dependencies.clear()
    }

    execute() {
        let oldReaction = Reaction.current
        Reaction.current = this
        this.unsubscribe()


        let result = this.func()

        Reaction.current = oldReaction
        return result
    }


}







*/