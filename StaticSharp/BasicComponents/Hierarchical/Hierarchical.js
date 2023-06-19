
/*function GetParentElementByPredicate(firstParentToCompare, predicate) {
    var p = firstParentToCompare
    while (p != undefined) {
        if (predicate(p)) {
            
            return p
        } else {
            p = p.parentElement
        }
    }
    return undefined
}*/

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
    let currentChildren = [...element.children];
    targetChildren = [...targetChildren]


    // Remove any current children that are not in the target children
    for (let i = 0; i < currentChildren.length; i++) {
        let child = currentChildren[i];
        //console.log(child, targetChildren.includes(child))
        if (!targetChildren.includes(child)) {
            element.removeChild(child);
            //i--; // Decrement i to account for the removed child
        }
    }

    for (let i = 0; i < targetChildren.length; i++) {
        let child = targetChildren[i];
        if (element.children[i] != child) {
            element.insertBefore(child, element.children[i]);
        }
    }


    /*currentChildren = [...element.children]

    // Add or move target children to match the targetChildren array
    for (let i = 0; i < targetChildren.length; i++) {
        let child = targetChildren[i];
        let currentIndex = currentChildren.indexOf(child);

        if (currentIndex === -1) {
            // Child is not in current children, add it to end of element
            element.appendChild(child);
        } else if (currentIndex < i) {
            // Child is in current children but needs to be moved before current child
            let nextSibling = targetChildren[i + 1] || null;
            element.insertBefore(child, nextSibling);
        }
        // Otherwise, child is already in correct position, do nothing
    }*/
}

/*function SyncChildren(element, targetChildren) {
    let currentChildren = [...element.children]

    if (currentChildren.length === targetChildren.length)
        return

    let ic = 0
    let it = 0
    while (it < targetChildren.length) {

    }

    for (let i = 0; i < targetChildren.length; i++) {
        let current = 
    }

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

}*/



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
var parentGetDepth = 1

StaticSharpClass("StaticSharp.Hierarchical", (element) => {

    StaticSharp.Entity(element)


    CreateCollectionSocket(element, "UnmanagedChildren", element)
    CreateSocket(element, "NextSibling", () => element.Parent)

    CreateLayer(element)


    element.Reactive = {
        Exists: true,

        /*IsRoot: () => element.Parent == undefined,

        NestingDepth: () => (element.IsRoot || element.overlaySign==1) ? 0 : (element.Parent.NestingDepth + 1),*/

        Root: () => element.Parent.Root,

        //Parent: undefined,
        //Place: undefined,

        Parent: undefined,

        //Place: undefined,
        //UnmanagedChildren: Enumerable.Empty(),
        //ExistingUnmanagedChildren: e => e.UnmanagedChildren.Where(x => x.Exists)
    }
  



    
 

})
