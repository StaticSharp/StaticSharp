/*function Border() {
    let _this = this
    _this.Reactive = {
        Left: undefined,
        Top: undefined,
        Right: undefined,
        Bottom: undefined
    }
}*/


function BlockInitialization(element) {

    HierarchicalInitialization(element)
    element.isBlock = true
    element.Reactive = {
        
        PaddingLeft: undefined,
        PaddingTop: undefined,
        PaddingRight: undefined,
        PaddingBottom: undefined,

        MarginLeft: undefined,
        MarginTop: undefined,
        MarginRight: undefined,
        MarginBottom: undefined,

        LayoutX: undefined,
        LayoutY: undefined,
        LayoutWidth: undefined,
        LayoutHeight: undefined,

        X: () => element.LayoutX,
        Y: () => element.LayoutY,
        Width: () => element.LayoutWidth,
        Height: () => element.LayoutHeight,

        Hover: false
    }
}





function BlockBefore(element) {
    HierarchicalBefore(element)

    let parent = element.parentElement
    if (parent.AddChild)
        parent.AddChild(element);

    
}

function BlockAfter(element) {
    HierarchicalAfter(element)

    XToStyle(element);

    YToStyle(element);

    element.addEventListener('mouseenter', e => {
        let d = Reaction.beginDeferred()
        element.Hover = true
        d.end()
    });

    element.addEventListener('mouseleave', e => {
        let d = Reaction.beginDeferred()
        element.Hover = false
        d.end()
    });
    



}