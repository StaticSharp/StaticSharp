function HierarchicalInitialization(element) {
    console.log("HierarchicalInitialization", element, element.previousSibling)
    let parent = element.parentElement

    element.Reactive = {
        Id: undefined,
        Parent: parent
    }

    element.ById = function (id) {


    }
}

function HierarchicalBefore(element) {

}