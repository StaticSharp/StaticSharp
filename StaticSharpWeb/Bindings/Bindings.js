




function ReactionBase(func) {
    let _this = this

    _this.dependencies = new Set()
    _this.func = func
    
    _this.addDependency = function (property) {
        _this.dependencies.add(property)
        property.onChanged.add(_this.onChangedHandler)
    }

    _this.unsubscribe = function () {
        if (_this.dependencies.size == 0) return

        for (let dependency of _this.dependencies) {
            dependency.onChanged.delete(_this.onChangedHandler)
        }
        _this.dependencies.clear()
    }

    _this.execute = function () {
        let oldReaction = Reaction.current
        Reaction.current = _this
        _this.unsubscribe()


        let result = _this.func()

        Reaction.current = oldReaction
        return result
    }
}


function Reaction(func) {
    let _this = this

    ReactionBase.call(_this, func)

    
    _this.onChangedHandler = function () {
        Reaction.deferred.add(_this)
    }

    _this.execute()
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
    let _this = this

    ReactionBase.call(_this, func)

    _this.dirty = true
    _this.onChange = onChange

    
    _this.onChangedHandler = function () {
        _this.dirty = true
        //console.log("_this.dirty = true", _this, _this.onChange)
        
        _this.onChange()
    }
}
Binging.prototype = Object.create(ReactionBase.prototype);
Binging.prototype.constructor = Binging;


function Property(value) {

    let _this = this

    _this.onChanged = new Set()
    _this.binding = undefined

    
    _this.onDependencyChanged = function () {
        //console.log("onDependencyChanged", _this.binding.dirty, _this.binding)
        _this.onChanged.forEach(x => x())
    }


    _this.getValue = function() {
        //console.log("getValue")
        if (Reaction.current) {
            Reaction.current.addDependency(_this)
        }
        if (_this.binding) { //wechat if (this.binding?.dirty)
            if (_this.binding.dirty) {
                _this.value = _this.binding.execute()
                _this.binding.dirty = false
            }
        }
        return _this.value
    }

    _this.setValue = function(value) {
        //console.log("setValue " + value + " " + _this.onChanged.size)
        if (typeof value === 'function') {
            if (_this.binding) {
                if (_this.binding.func === value) {
                    return
                }
                _this.binding.unsubscribe()
                //console.log("change binding from", _this.binding.func, "to", value)
            }
            

            _this.binding = new Binging(value, _this.onDependencyChanged)


        } else {
            //console.log("value assigned", _this.value, "->", value, "will notify ", _this.onChanged.size)
            if (_this.binding) {
                _this.binding.unsubscribe()
                _this.binding = undefined
            }

            if (_this.value === value)
                return

            _this.value = value
        }

        var d = Reaction.beginDeferred()

        for (let i of _this.onChanged) {
            try {
                //console.log(i)
                i()
            } catch (e) {
                console.error(e)
            }            
        }
        //this.onChanged.forEach(x => x())
        if (d)
            d.end()

    }

    _this.attach = function(object, name) {
        //let property = this
        let accessorDescriptor = {
            get: function () {
                return _this.getValue()
            },
            set: function (value) {
                _this.setValue(value)

            }
        }
        Object.defineProperty(object, name, accessorDescriptor);
        return _this
    }

    
    _this.setValue(value)


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
                        //console.log("asigning existing property", name)
                        const propertyField = target["__" + name]
                        propertyField.setValue(value)
                    } else {
                        //console.log("creating new property", name)
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
    let _this = this
    let previous = _this.getValue()

    return new Reaction(() => {
        let current = _this.getValue()
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


    root.Reactive.B = () => root.A * 2
    console.assert( root.B == 16)

    root.Reactive.A = 10
    console.assert( root.B == 20)

    

    root.Reactive = {
        C: 7,
        D: () => root.C,
        //E: () => root.B*4,
    }

    console.assert( Property.exists(root, "C") == true)
    console.assert( Property.exists(root, "D") == true)

    console.assert(root.D == root.C)
    //console.assert(root.E == root.B*4)

    root.Reactive.B = () => root.A * 3
    //console.assert(root.B == 30)
    //console.assert(root.E == root.A * 3 * 4)

    /*root.Reactive.B = 10
    console.assert(root.E == 40)

    root.Reactive.B = () => root.A * 3
    console.assert(root.E == 10 * 3 * 4)*/


    new Reaction(() => {
        root.Field = root.C
    })
    console.assert(root.Field == root.C)

    let bReactionResult
    new Reaction(() => {
        bReactionResult = root.B
    })
    console.assert(bReactionResult == root.B)
    console.group("root.A = 20")
    root.A = 20
    console.groupEnd()
    console.assert(bReactionResult == root.B)


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