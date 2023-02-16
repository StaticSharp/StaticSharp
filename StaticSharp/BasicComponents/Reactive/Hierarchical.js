
function GetParentElementByPredicate(firstParentToCompare, predicate) {
    var p = firstParentToCompare
    while (p != undefined) {
        if (predicate(p)) {
            
            return p
        } else {
            /*if (p.tagName == "SCROLLABLE") {
                console.error("SCROLLABLE", p.parentElement)
            }*/
            p = p.parentElement
        }
    }
    return undefined
}


function SocketProperty(value) {
    let _this = this
    Property.call(_this, value)
}


function CreateSocketProperty(element,name) {
    let property = new Property()
    property.attach(element, name)



}


/**
 *  @typedef Hierarchical
 *  @property {Hierarchical} Root
 *  @property {Hierarchical} Parent
 *  @property {Hierarchical} FirstChild
 *  @property {Hierarchical} NextSibling
 *  @property {Enumerable<Hierarchical>} Children
*/

/**
 *  @param {Hierarchical} element
 */
function Hierarchical(element) {
    element.isHierarchical = true

    CreateSocket(element, "FirstChild", element)
    CreateSocket(element, "NextSibling", () => element.Parent)

    element.Reactive = {


        IsRoot: () => element.Parent == undefined,

        NestingDepth: () => (element.IsRoot || element.overlaySign==1) ? 0 : (element.Parent.NestingDepth + 1),

        Root: () => element.Parent.Root,

        Parent: undefined,

        /*AppendChildSocket: e => {
            if (!e.FirstChild) {
                return element.Reactive.FirstChild
            } else {
                return element.Children.Last().Reactive.NextSibling
            }
        }*/
    }

    

    element.Children = new Enumerable(function* () {
        var i = element.FirstChild
        while (i != undefined) {
            yield i
            i = i.NextSibling
        }
    })

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* element.Children
    })




    GetHtmlNode = function (node) {
        let parentNode = node.parentNode
        let parentTag = parentNode.tagName
        if (parentTag == "A") {
            return parentNode
        }
        return node
    }

    new Reaction(() => {
        let currentChildren =[...element.children]
        let tergetChildren = [...element.HtmlNodesOrdered.Select(x => GetHtmlNode(x))]

        let equal = currentChildren.length === tergetChildren.length && currentChildren.every(function (value, index) { return value === tergetChildren[index] })

        if (!equal) {
            console.log(currentChildren, tergetChildren, element)
            for (let i of currentChildren) {
                i.remove()
            }
            for (let i of tergetChildren) {
                element.appendChild(i)
            }
            //currentChildren = [...element.children]
            //console.log("after:", currentChildren, tergetChildren)
        }
        
    })



    /*element.ConnectChild = function (socketProperty, child) {
        element.Place = socketProperty
        element.Place.setValue(child)
        child.Parent = element
    }


    element.AssignElementToProperty = function (child, propertyName) {

        if (!Property.exists(element, propertyName)) {
            console.error(`No property ${propertyName} in ${element} `)
        }

        element.ConnectChild(element.Reactive[propertyName], child)
    }


    element.AppendChild = function (child) {       
        element.ConnectChild(element.AppendChildSocket, child)
    }*/

    //console.log("currentParent", currentParent)





    /*OnChanged(
        () => element.FirstChild,
        (p, c) => {
            if (p) {
                p.Parent = undefined
                p.remove()
            }
            if (c) {
                c.Parent = element
                //let node = c.GetHtmlNode()
                //element.insertAdjacentElement('afterbegin', node)
            }
            //console.log("element.NextSibling", element.Parent.Children.Last())
        }
    )

    OnChanged(
        () => element.NextSibling,
        (p, c) => {
            if (p) p.Parent = undefined
            if (c) c.Parent = () => element.Parent
            //console.log("element.NextSibling", element.Parent.Children.Last())
        }
    )*/



    

    



    
    //element.Children[Symbol.iterator] = ;

    /*element.Sibling = function (id) {
        return element.Parent.Child(id)        
    }

    element.Child = function (id) {
        let i = element.FirstChild
        
        if (typeof id === "number") { 
            for (let n = 0; n < id; n++) {                
                i = i.NextSibling
            }

        } else {
            while (i) {
                if (i.id == id) {
                    return i
                }
                i = i.NextSibling
            }
        }
        //console.warn(`element ${element.tagName} do not have child "${id}"`)
        return i
    }*/


    
 

}
