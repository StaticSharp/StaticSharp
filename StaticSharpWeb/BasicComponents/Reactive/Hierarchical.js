function getDomPath(el) {
    var stack = [];
    while (el.parentNode != null) {
        var sibCount = 0;
        var sibIndex = 0;
        for (var i = 0; i < el.parentNode.childNodes.length; i++) {
            var sib = el.parentNode.childNodes[i];
            if (sib.nodeName == el.nodeName) {
                if (sib === el) {
                    sibIndex = sibCount;
                }
                sibCount++;
            }
        }
        if (el.hasAttribute('id') && el.id != '') {
            stack.unshift(el.nodeName.toLowerCase() + '#' + el.id);
        } else if (sibCount > 1) {
            stack.unshift(el.nodeName.toLowerCase() + ':eq(' + sibIndex + ')');
        } else {
            stack.unshift(el.nodeName.toLowerCase());
        }
        el = el.parentNode;
    }
    return (stack.slice(1)).join(' > '); // removes the html element
}



function HierarchicalInitialization(element) {

    element.isHierarchical = true

    let parent = element.parentElement

    element.Reactive = {
        Id: undefined,
        Parent: parent,
        FirstChild: undefined,
        LastChild: undefined,
        NextSibling: undefined,

        Modifier: GetModifier(element.parentElement),
    }
    

    element.ById = function (id) {
        //console.log("ById called by", element,"with id = ", id)
        if (parent.isHierarchical) {

            let i = parent.FirstChild            
            while (i) {
                //console.log("trying to compare", i)
                if (i.Id == id) {
                    //console.log("found by id", i)
                    return i
                }
                i = i.NextSibling
            }

            //return parent.ById(id)

        } else {
            if (element.Id == id) {
                return element
            }
        }


    }

    /*element.ChildById = function (id) {

    }*/

}

function HierarchicalBefore(element) {
    
}

function HierarchicalAfter(element) {
    let parent = element.parentElement

    //console.log("Adding to search chain ", element, getDomPath(element))
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
