



/*function CreatePage() {
    let result = CreateInplace(document.body,Page)
    result.Content = function (parent) {
        let result = Create(ScrollLayout)
        result.Parent = parent
        result.Content = function (parent) {
            let result = Create(parent, Column)
            return result
        }(result)

        result.AddChild()

        return result
    }(result)

    Create(undefined, Page, (parent) => {
        parent.TopBar = Create(parent, Paragraph)
            .AddChild((parent) => {

            })

        parent.Content = function(parent){
            let result = Create(parent, ScrollLayout)
            result.Content = function (parent) {
                let result = Create(parent, Column)
                return result
            }(result)
        }()


        parent.Content = Create(parent, ScrollLayout).Modify((p) => {
            p.Content =
                Create(p, Column, (p) => {

                })
        })

        parent.AddChild(
            Create(parent, ScrollLayout, (parent) => {

            })
        )
    })
}*/









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



function Hierarchical(element) {
    element.isHierarchical = true

    

    element.Reactive = {


        IsRoot: () => element.Parent == undefined,

        NestingDepth: () => (element.IsRoot || element.overlaySign==1) ? 0 : (element.Parent.NestingDepth + 1),

        Root: () => element.Parent.Root,
        /*Parent: undefined() => {

            let v = GetParentElementByPredicate(element.parentElement, x => x.isHierarchical)

            return v
        },*/
        ParentBlock: () => GetParentElementByPredicate(element.Parent, x => x.isBlock),
        FirstChild: undefined,
        LastChild: undefined,
        NextSibling: undefined,
    }

    if (element.Parent) {
        if (element.dataset.property) {
            element.Parent[element.dataset.property] = element

        }
        if (element.dataset.child!=undefined) {
            
            if (!element.Parent.FirstChild) {
                element.Parent.FirstChild = element
                element.Parent.LastChild = element
            } else {
                element.Parent.LastChild.NextSibling = element
                element.Parent.LastChild = element
            }
        }
        
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
        
        if (typeof id === "number") { 
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


    
 

}
