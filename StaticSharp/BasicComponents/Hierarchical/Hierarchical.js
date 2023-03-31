
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

/*
function SocketProperty(value) {
    let _this = this
    Property.call(_this, value)
}


function CreateSocketProperty(element,name) {
    let property = new Property()
    property.attach(element, name)



}*/


function SyncChildren(element, targetChildren) {
    let currentChildren = [...element.children]

    let equal = currentChildren.length === targetChildren.length && currentChildren.every(function (value, index) { return value === targetChildren[index] })

    if (!equal) {
        //console.log(element, currentChildren, targetChildren)
        for (let i of currentChildren) {
            i.remove()
        }
        for (let i of targetChildren) {
            element.appendChild(i)
        }
    }

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

    CreateCollectionSocket(element, "UnmanagedChildren", element)
    CreateSocket(element, "NextSibling", () => element.Parent)

    element.Reactive = {
        Exists: true,

        IsRoot: () => element.Parent == undefined,

        NestingDepth: () => (element.IsRoot || element.overlaySign==1) ? 0 : (element.Parent.NestingDepth + 1),

        Root: () => element.Parent.Root,

        Parent: undefined,

    }



    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* element.ExistingUnmanagedChildren
    })




    GetHtmlNode = function (node) {
        let parentNode = node.parentNode
        if (parentNode == undefined) {
            return node
        }
        let parentTag = parentNode.tagName
        if (parentTag == "A") {
            return parentNode
        }
        return node
    }

    new Reaction(() => {
        //let currentChildren =[...element.children]
        let tergetChildren = [...element.HtmlNodesOrdered.Select(x => GetHtmlNode(x))]

        SyncChildren(element, tergetChildren)        
    })



    
 

}
