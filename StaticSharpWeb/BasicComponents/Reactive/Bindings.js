




function ReactionBase(func) {
    let _this = this

    _this.triggeringProperties = new Set()
    _this.func = func
    
    _this.addTriggeringProperty = function (property) {
        _this.triggeringProperties.add(property)
        property.dependentReactions.add(_this)
    }

    _this.unsubscribeFromTriggeringProperties = function () {
        if (_this.triggeringProperties.size == 0) return

        for (let triggeringProperty of _this.triggeringProperties) {
            triggeringProperty.dependentReactions.delete(_this)
        }
        _this.triggeringProperties.clear()
    }

    _this.execute = function () {
        let oldReaction = Reaction.current
        Reaction.current = _this
        _this.unsubscribeFromTriggeringProperties()

        try {
            let result = _this.func()
            return result
        }
        finally {
            Reaction.current = oldReaction
        }
    }


    //_this.makeDirty = abstract function
    _this.dirtImmune = false
}


function Reaction(func) {
    let _this = this

    ReactionBase.call(_this, func)
    
    _this.makeDirty = function () {
        if (_this.dirtImmune)
            return;
        Reaction.deferred.add(_this)
    }

    if (Reaction.deferred) {
        Reaction.deferred.add(_this)
    } else {
        let d = Reaction.beginDeferred()

        try {
            _this.execute()
        } catch (e) {
            console.warn(e)
        }
        
        d.end()
    }
}

Reaction.prototype = Object.create(ReactionBase.prototype)
Reaction.prototype.constructor = Reaction

Reaction.beginDeferred = function () {
    if (!Reaction.deferred) {
        Reaction.deferred = new Set()
        return {
            end: function () {

                let maxIterations = 64;
                 
                while (Reaction.deferred.size > 0 & maxIterations > 0) {
                    //console.log("Reaction.deferred.end", Reaction.deferred.size)
                    let d = Array.from(Reaction.deferred)
                    for (let reaction of d) {
                        Reaction.deferred.delete(reaction)
                        try {
                            reaction.execute()
                        } catch (e) {
                            console.warn(e)
                        }
                        
                    }
                    maxIterations--
                    //d.forEach(x => x.execute())
                    //Reaction.deferred.clear()
                }

                if (maxIterations == 0) {
                    console.error("Recursive property binding", Reaction.deferred)
                }

                //let l = Reaction.deferred;
                Reaction.deferred = undefined
                //l.forEach(x => x.execute())

                

            }
        };
    }
    return {
        end: function () { }
    };
}


/*Reaction.beginNonReactive = function () {
    if (!Reaction.current) {
        let oldCurrent = Reaction.current
        Reaction.current = undefined
        return {
            end: function () {
                Reaction.current = oldCurrent
            }
        };
    }
    return {
        end: function () { }
    };
}*/



function Binging(func, onBecameDirty) {
    //console.log("function Binging(func, onChange)", onChange)
    let _this = this

    ReactionBase.call(_this, func)

    _this.dirty = true
    //_this.onBecameDirty = onBecameDirty

    
    _this.makeDirty = function () {
        if (_this.dirty)
            return

        _this.dirty = true        
        onBecameDirty()
    }
}
Binging.prototype = Object.create(ReactionBase.prototype);
Binging.prototype.constructor = Binging;



function Property(value) {

    let _this = this
    _this.name = ""
    _this.parent = null

    _this.dependentReactions = new Set()
    _this.binding = undefined

    _this.makeDirty = function () {
        _this.binding.makeDirty()
    }
    
    _this.onBindingBecameDirty = function () {
        _this.makeDependentReactionsDirty()
    }

    _this.makeDependentReactionsDirty = function () {
        _this.dependentReactions.forEach(x => x.makeDirty())
    }

    /*_this.dependsOn = function (property) {
        if (!_this.binding)
            return false
        _this.getValue()
        for (let i of _this.binding.dependencies) {
            if (i == property)
                return true
        }
        for (let i of _this.binding.dependencies) {
            if (i.dependsOn(property))
                return true
        }
        return false
    }

    _this.getRecursiveDependencies = function () {
        let result = new Set();
        if (!_this.binding)
            return result
        _this.getValue()
        
        _this._collectRecursiveDependencies(result)
        return result
    }

    _this._collectRecursiveDependencies = function (set) {
        for (let i of _this.binding.dependencies) {
            set.add(i)
            i._collectRecursiveDependencies(set)
        }
    }*/
    _this.executionInProgress = false
    
    _this.getValue = function() {
        //console.log("getValue")
        
        //console.log("getValue", _this.name, _this.binding?.dirty)

        if (Reaction.current) {
            Reaction.current.addTriggeringProperty(_this)
        }

        if (_this.binding) { //wechat (this.binding?.dirty) not supported
            if (_this.binding.dirty) {



                if (_this.executionInProgress) {


                    if (_this.reactionsWhoReceivedOldValue == undefined) {
                        _this.reactionsWhoReceivedOldValue = new Set()
                    }
                    _this.reactionsWhoReceivedOldValue.add(Reaction.current)                    

                    //console.log("getValue", _this.name, "executionInProgress")

                    return _this.value
                }
                try {

                    var oldValue = _this.value

                    try {
                        _this.executionInProgress = true
                        _this.value = _this.binding.execute()
                        
                    } finally {
                        _this.executionInProgress = false

                        _this.binding.dirty = false
                        //console.log("execute finished ", _this.object, _this.name, oldValue, "->", _this.value)


                        if (_this.reactionsWhoReceivedOldValue) {

                            if (_this.value !== oldValue) {

                                //var reactionsToPrint = Array.from(_this.reactionsWhoReceivedOldValue).map(x => x.func.name)
                                //console.log("ReactionsWhoReceivedOldValue", reactionsToPrint)

                                let d = Reaction.beginDeferred()
                                _this.reactionsWhoReceivedOldValue.forEach(x => x.makeDirty())
                                d.end()
                            }

                            if (_this.reactionsWhoReceivedOldValue) {
                                _this.reactionsWhoReceivedOldValue = undefined
                            }

                        }
                        
                            
                    }

                } catch (e) {


                    

                    console.error(e)
                }                
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
                _this.binding.unsubscribeFromTriggeringProperties()
                //console.log("change binding from", _this.binding.func, "to", value)
            }
            
            _this.binding = new Binging(value, _this.onBindingBecameDirty)


        } else {
            //console.log("value assigned", _this.value, "->", value, "will notify ", _this.onChanged.size)
            if (_this.binding) {
                _this.binding.unsubscribeFromTriggeringProperties()
                _this.binding = undefined
            }

            if (_this.value === value)
                return

            _this.value = value
        }

        var d = Reaction.beginDeferred()

        _this.makeDependentReactionsDirty()

        if (d)
            d.end()

    }

    _this.attach = function(object, name) {
        //let property = this
        _this.name = name
        _this.object = object
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

/*
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
}*/



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
