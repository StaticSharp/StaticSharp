
function CalcLeft(container, child) {
    if (container.PaddingLeft != undefined) {
        return Math.max(container.PaddingLeft, First(child.MarginLeft,0))
    }
    if (container.MarginLeft == undefined) {
        return First(child.MarginLeft,0)
    }
    return Math.max(child.MarginLeft - container.MarginLeft, 0)
}

function CalcOffset(container, child, sideName) {
    let paddingName = "Padding" + sideName
    let marginName = "Margin" + sideName
    if (container[paddingName] != undefined) {
        return Math.max(container[paddingName], First(child[marginName], 0))
    }
    if (container[marginName] == undefined) {
        return First(child[marginName], 0)
    }
    if (child[marginName] == undefined) {
        return 0
    }
    return Math.max(child[marginName] - container[marginName], 0)
}


function Block(element) {

    Hierarchical(element)

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

        

        InternalWidth: undefined,
        InternalHeight: undefined,


        X: () => element.LayoutX,
        Y: () => element.LayoutY,

        Width: () => First(element.LayoutWidth, element.InternalWidth),
        Height: () => First(element.LayoutHeight, element.InternalHeight),



        Hover: false
    }

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
