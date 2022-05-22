function Border() {
    let _this = this
    _this.Reactive = {
        Left: undefined,
        Top: undefined,
        Right: undefined,
        Bottom: undefined
    }
}


function BlockInitialization(element) {

    HierarchicalInitialization(element)

    element.Reactive = {
        

        Padding: new Border(),
        Margin: new Border(),

        LayoutX: undefined,
        LayoutY: undefined,
        LayoutWidth: undefined,
        LayoutHeight: undefined,

        X: () => element.LayoutX,
        Y: () => element.LayoutY,
        Width: () => element.LayoutWidth,
        Height: () => element.LayoutHeight,
    }
}





function BlockBefore(element) {
    HierarchicalBefore(element)

    let parent = element.parentElement
    if (parent.AddChild)
        parent.AddChild(element);

    XToStyle(element);
    
    YToStyle(element);

    

}

function BlockAfter(element) {
    HierarchicalAfter(element)
}