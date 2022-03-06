
//class NotifiableValue
var currentReaction = undefined



function CreateProperty(object, name, initialValue) {//source
    let propertyObject = {
        value: undefined,
        onChanged: new Set()
    }

    let accessorDescriptor = {
        get: function () {
            console.log(`getter ${name}`)
            if (currentReaction) {
                currentReaction.addDependency(propertyObject)
            }
            if (propertyObject.dirty) {
                propertyObject.value = propertyObject.calculateValue()
                propertyObject.dirty = false
            }
            return propertyObject.value
        },
        set: function (value) {
            if (typeof value === 'function') {
                // do something
            } else {
                console.group(`setter(value) ${name} = ${value}`)
                let isStartOfChain = BeginChange()

                propertyObject.value = value
                propertyObject.onChanged.forEach(x => x())

                if (isStartOfChain) EndChange()
                console.groupEnd()
            }

            
            /*
            propertyObject.reactions.map(x => x())
            if (isStartOfChain) {
                dirtyList.map(x=>x)
            }*/

        }
    }

    Object.defineProperty(object, name, accessorDescriptor);
    if (initialValue != undefined) {
        object[name] = initialValue
    }

}




class ReactionBase {
    static current = undefined
    dependencies = new Set()

    static deferred = undefined

    static beginDeferred() {
        if (!Reaction.deferred) {
            Reaction.deferred = new Set()
            return {
                end: function () {
                    let l = Reaction.deferred;
                    Reaction.deferred = undefined
                    l.forEach(x => x.execute())
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


class Reaction extends ReactionBase {

    onChangedHandler = function () {
        Reaction.deferred.add(this)
    }.bind(this)

    constructor(func) {
        super(func)
        this.execute()
    }
}

class Binging extends ReactionBase {
    dirty = true

    onChangedHandler = function () {
        this.dirty = true
        this.onChange()
    }.bind(this)

    constructor(func, onChange) {
        super(func)
        this.onChange = onChange
    }

    /*onChangedHandler() {
        this.dirty = true
        this.onChange()
    }*/
}


class Property {    
    onChanged = new Set()
    binding = undefined
    

    constructor(value) {
        this.setValue(value)
    }

    onDependencyChanged() {
        this.onChanged.forEach(x => x())
    }


    getValue() {
        //console.log(`getValue`)
        if (Reaction.current) {
            Reaction.current.addDependency(this)
        }
        if (this.binding?.dirty) {
            this.value = this.binding.execute()
            this.binding.dirty = false
        }
        return this.value
    }

    setValue(value) {
        //console.log("setValue " + value + " " + this.onChanged.size)
        if (typeof value === 'function') {
            if (this.binding) {
                if (this.binging.func === value)
                    return
                this.binding.unsubscribe()
            }
            this.binding = new Binging(value, this.onDependencyChanged.bind(this))

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
        d?.end()

    }

    attach(object, name) {
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

}




function CreateReaction(func) {
    let reactionObject = {
        dependencies: new Set(),
        onChangedHandler: function () {
            LazyReactions.add(reaction)
        },
        addDependency: function (property) {
            this.dependencies.add(property)
            property.onChanged.add(this.onChangedHandler)
        },
        unsubscribe: function () {
            for (dependency of this.dependencies) {
                dependency.onChanged.delete(this.onChangedHandler)
            }
        }
    }

    let reaction = function () {
        let oldReaction = currentReaction
        currentReaction = reactionObject
        reactionObject.unsubscribe()
        func()
        currentReaction = oldReaction
    }

    return reaction

}
/*
function CreateProperty(object, name, func) {
    let propertyObject = {
        dirty: true,
        value: undefined,
        validator: func,
        validate: function () {
            let oldCurrent = currentProperty
            let newValue = validator()
            if (newValue != this.value) {
                this.value = newValue;
            }
        }
    }

    
}*/


function Button(element) {


    let root = []

    //element.innerText = window.innerWidth

    root.push(new Property(window.innerWidth)
        .attach(window, "InnerWidth"))
    

    let createProperty = function(prew,i){
        return () => prew.getValue() + 1
    }

    for (let i = 0; i < 2000; i++) {
        root.push(new Property(createProperty(root[i],i)))
        //p = r
    }




    window.onresize = function (event) {
        let d = Reaction.beginDeferred()
        window.InnerWidth = window.innerWidth
        window.InnerHeight = window.innerHeight
        d.end()
    }

    element.onclick = () => {
        var t0 = performance.now()

        console.log(root[root.length - 1].getValue())

        console.log("Call to doSomething took " + (performance.now() - t0) + " milliseconds.");
    }
    



    new Reaction(() => {
        element.style.backgroundColor = `hsl(${window.InnerWidth},50%,60%)`
        element.innerText = window.InnerWidth
    })





    /*


    

    new Property(8).attach(root, "a")

    new Property(() => {
        console.log("calc b")
        return root.a + 2
    }).attach(root, "b")


    let property = new Property(8)
    console.log(root.b)*/

    


    let parent = element.parentElement;

    element.billboard = true;

    let previousIsBillboard = element.previousElementSibling.billboard;
    if (!previousIsBillboard) {
        element.style.marginTop = "16px"
    }


    /*CreateProperty(element, "a", 1)
    CreateProperty(element, "b", 2)
    CreateProperty(element, "c")


    

    CreateReaction(() => {
        console.group("reaction a + b + c")
        console.log(`a + b + c =  + ${element.a} + ${element.b} + ${element.c}`)
        console.groupEnd();
    })()

    CreateReaction(() => {
        console.group("reaction c = a + b")
        element.c = element.a + element.b
        console.groupEnd();
    })()*/






    //element.count = 9


    

    
    CreateProperty(element, "text", function () {
        return "Text"// + this.count;
    })


    


    //element.onAnchorsChanged = []
    //element.anchors = {}

    element.style.display = "flex"
    element.style.flexDirection = "column"
    element.style.justifyContent = "center"


    element.updateWidth = function () {
        let left = parent.anchors.fillLeft;
        let right = parent.anchors.fillRight;
        element.style.left = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";

        /*let contentSpace = parent.anchors.textRight - parent.anchors.textLeft;
        let contentWidth = Math.min(MaxContentWidth, contentSpace);
        let horizontalMargin = 0.5 * (contentSpace - contentWidth);

        element.anchors.textLeft = parent.anchors.textLeft - left + horizontalMargin;
        element.anchors.textRight = element.anchors.textLeft + contentWidth;

        element.onAnchorsChanged.map(x => x());*/
    }
    parent.onAnchorsChanged.push(element.updateWidth);

}