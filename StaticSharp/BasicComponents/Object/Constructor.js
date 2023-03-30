function _deleteScript() {
    let script = document.currentScript
    let parent = script.parentElement
    parent.removeChild(script)
    return parent
}


var currentSocket = undefined
var parentStack = []
function getCurrentParent() {
    if (parentStack.length>0)
        return parentStack[parentStack.length - 1]
    return undefined
}

function SetCurrentSocket(propertyName) {
    var element = _deleteScript()
    var parent = getCurrentParent()
    currentSocket = parent.Reactive[propertyName]
}

function AssignToParentProperty(propertyName) {
    var element = _deleteScript()
    let parent = getCurrentParent()
    parent[propertyName] = element
}

function AssignPreviousTagToParentProperty(propertyName) {
    var element = _deleteScript()
    let lastChild = element.lastChild 
    let parent = getCurrentParent()
    parent[propertyName] = lastChild
}


function Constructor() {
    var element = _deleteScript()
    //element.Parent = parent
    
    
    for (let i of arguments) {
        i(element)
    }
    if (currentSocket) {
        currentSocket.setValue(element)
    }

    currentSocket = element.Reactive.FirstChild
    parentStack.push(element)

    return element;
}

function Pop() {
    let element = _deleteScript()

    currentSocket = element.Reactive.NextSibling
    parentStack.pop()

    if (typeof (element.AfterChildren) === "function")
        element.AfterChildren()

}


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

    let previousValue = undefined

    property.onAssign = function () {
        //console.log("onAssign", property.getValue())
        let newValue = property.getValue()

        if (previousValue == newValue)
            return

        let d = Reaction.beginDeferred()

        if (previousValue) {
            //console.log("deleting previous")
            //previousValue.place.setValue(undefined)
            previousValue.place = undefined
            previousValue.Parent = undefined
            CreateOrClearLayer(previousValue)

            // if reparenting element is a part of element linked list, 
            // than subsequent elements of list are also effectively reparented
            let nextSibling = previousValue.NextSibling
            while (nextSibling) {
                nextSibling.Parent = undefined
                CreateOrClearLayer(nextSibling)
                nextSibling = nextSibling.NextSibling
            }
        }
        if (newValue != undefined) {
            if (newValue.place) {
                newValue.place.setValue(undefined)
            }
            newValue.place = property
            newValue.Parent = parentExpression
            CreateOrClearLayer(newValue)
            
            // if reparenting element is a part of element linked list, 
            // than subsequent elements of list are also effectively reparented
            let nextSibling = newValue.NextSibling
            while (nextSibling) {
                nextSibling.Parent = parentExpression
                CreateOrClearLayer(nextSibling)
                nextSibling = nextSibling.NextSibling
            }
        }
        previousValue = newValue

        d.end()

    }

    return property
}


function CreateCollectionSocket(element, name, parentExpression) {
    let firstChildPropertyName = name + "First"
    
    let firstChildProperty = CreateSocket(element, firstChildPropertyName, parentExpression)

    var value = new DomLinkedList(firstChildProperty)

    element[name] = value
    element["Existing"+name] = value.Where(x=>x.Exists)

    /*Object.defineProperty(element, name, {
        get: function () {
            return value
        }
    });*/
}





function Create(parent, ...constructors) {
    let primary = constructors[0]
    let tagName = CamelToKebab(primary.name)
    
    let element = document.createElement(tagName)
    parent.appendChild(element)
    element.Parent = parent
    for (let i of constructors) {
        i(element)
    }
    return element;
    
}