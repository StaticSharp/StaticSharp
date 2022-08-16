function Hierarchical(element) {
    element.isHierarchical = true

    let parent = element.parentElement

    element.Reactive = {
        Parent: parent,
        ParentBlock: () => {
            var p = element.Parent
            while (p != undefined) {
                if (p.isBlock) {
                    return p
                } else {
                    p = p.Parent
                }
            }
            return undefined
        },
        FirstChild: undefined,
        LastChild: undefined,
        NextSibling: undefined,
        ParentModifier: GetModifier(element.parentElement),
        Modifier: () => element.ParentModifier,
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
        //console.log("Sibling called by", element,"with id = ", id)
        if (parent.isHierarchical) {
            return parent.Child(id)
        }
    }

    element.Child = function (id) {
        let i = element.FirstChild
        while (i) {
            if (i.id == id) {
                return i
            }
            i = i.NextSibling
        }
    }

    if (parent.isHierarchical) {
        if (!parent.FirstChild) {
            parent.FirstChild = element
            parent.LastChild = element
        } else {
            parent.LastChild.NextSibling = element
            parent.LastChild = element
        }
    }

}
