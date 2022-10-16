
/*function CalcLeft(container, child) {
    if (container.PaddingLeft != undefined) {
        return Math.max(container.PaddingLeft, First(child.MarginLeft,0))
    }
    if (container.MarginLeft == undefined) {
        return First(child.MarginLeft,0)
    }
    return Math.max(child.MarginLeft - container.MarginLeft, 0)
}*/

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
    

    BaseModifier(element)

    element.isBlock = true



    element.Reactive = {
        
        Depth: 0,

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


        X: () => First(element.LayoutX,0),
        Y: () => First(element.LayoutY,0),

        AbsoluteX: () => element.IsRoot ? 0 : Sum(element.Parent.AbsoluteX, element.Parent.ScrollXActual, element.X),
        AbsoluteY: () => element.IsRoot ? 0 : Sum(element.Parent.AbsoluteY, -element.Parent.ScrollYActual, element.Y),

        Width: () => First(element.LayoutWidth, element.InternalWidth),
        Height: () => First(element.LayoutHeight, element.InternalHeight),

        
        
    }

    DepthToStyle(element)


    XToStyle(element);
    YToStyle(element);
    WidthToStyle(element)
    HeightToStyle(element)

    

    



    new Reaction(() => {
        element.style.fontSize = ToCssSize(element.FontSize)
    })

    element.Events.MouseEnter = () => element.Hover = true
    element.Events.MouseLeave = () => element.Hover = false


    


}
