function GetParentElementByPredicate(firstParentToCompare, predicate) {
    var p = firstParentToCompare
    while (p != undefined) {
        if (predicate(p)) {
            return p
        } else {
            p = p.parentElement
        }
    }
    return undefined
}



function Hierarchical(element) {


    element.isHierarchical = true


    

    element.Reactive = {
        IsRoot: () => element.Parent == undefined,

        NestingDepth: () => (element.IsRoot || element.overlaySign==1) ? 0 : (element.Parent.NestingDepth + 1),

        Root: () => element.Parent.Root,
        Parent: () => GetParentElementByPredicate(element.parentElement, x => x.isHierarchical),
        ParentBlock: () => GetParentElementByPredicate(element.Parent, x => x.isBlock),
        FirstChild: undefined,
        LastChild: undefined,
        NextSibling: undefined,
        //ParentModifier: GetModifier(element.parentElement),
        //Modifier: () => element.ParentModifier,



    }

    element.Children = {}
    element.Children[Symbol.iterator] = function* () {
        var i = element.FirstChild
        while (i != undefined) {
            yield i
            i = i.NextSibling
        }
    };

    element.Sibling = function (id) {
        return element.Parent.Child(id)        
    }

    element.Child = function (id) {
        let i = element.FirstChild
        console.warn(typeof id, id)
        
        if (typeof id === "number") { 
            console.warn("element.FirstChild",element.FirstChild)
            for (let i = 0; i < id; i++) {
                
                i = i.NextSibling
                console.warn(i)
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
    }


    if (element.Parent) {

        if (!element.Parent.FirstChild) {
            element.Parent.FirstChild = element
            element.Parent.LastChild = element
        } else {
            element.Parent.LastChild.NextSibling = element
            element.Parent.LastChild = element
        }
    }
 

}
