
function StaticSharpClass(name, constructor) {

    const parts = name.split(".")
    const className = parts[parts.length - 1]
    parts.length = parts.length - 1



    let currentNamespace = window
    for (let i of parts) {
        currentNamespace[i] = currentNamespace[i] || {}
        currentNamespace = currentNamespace[i]
    }

    function constructorWrapper(_this, ...arguments) {
        _this.types = _this.types || []
        _this.types.push(name)
        constructor(_this, ...arguments)
    }

    currentNamespace[className] = constructorWrapper
    currentNamespace[className].tagName = CamelToKebab(className)
}


//Move to FrontendUtils
function CamelToKebab(value) {
    return value.replace(
        /[A-Z]+(?![a-z])|[A-Z]/g,
        (substring, offset) => (offset ? "-" : "") + substring.toLowerCase()
    )
}

/**
 * @param {Hierarchical} element
 * @param {Hierarchical | function():Hierarchical} parentExpression
 * @param {string} name
 * */
function CreateSocket(element,name,parentExpression) {

    let property = new Property()
    property.attach(element, name)

    //let previousValue = undefined

    property.onAssign = function (previousValue, newValue) {

        if (previousValue == newValue)
            return

        if (previousValue) {
            previousValue.place = undefined
            previousValue.Parent = undefined
            previousValue.Layer()

            // if reparenting element is a part of element linked list, 
            // than subsequent elements of list are also effectively reparented
            let nextSibling = previousValue.NextSibling
            while (nextSibling) {
                nextSibling.Parent = undefined
                nextSibling.Layer()
                nextSibling = nextSibling.NextSibling
            }
        }
        if (newValue) {
            

            if (newValue.place) {
                newValue.place.setValue(undefined)
            }
            newValue.place = property
            newValue.Parent = parentExpression
            newValue.Layer()
            
            // if reparenting element is a part of element linked list,
            // than subsequent elements of list are also effectively reparented

            if (parentExpression != undefined) {
                let nextSibling = newValue.NextSibling
                while (nextSibling) {
                    nextSibling.Parent = parentExpression
                    nextSibling.Layer()
                    nextSibling = nextSibling.NextSibling
                }
            }
        }
    }

    return property
}


function CreateCollectionSocket(element, name, parentExpression) {
    let firstChildPropertyName = name + "First"
    
    let firstChildProperty = CreateSocket(element, firstChildPropertyName, parentExpression)

    var value = new DomLinkedList(firstChildProperty)

    element[name] = value
    element["Existing"+name] = value.Where(x=>x.Exists)

}


function Create(parent, ...constructors) {
    let primary = constructors[0]
    let tagName = primary.tagName 
    
    let element = document.createElement(tagName)
    parent.appendChild(element)
    element.Parent = parent
    for (let i of constructors) {
        i(element)
    }
    return element;    
}