

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

function GetClipRect(clippingElement, contentX, contentY, contentWidth, contentHeight ) {
    let left = -contentX
    let top = -contentY
    let right = -clippingElement.Width + contentWidth + contentX
    let bottom = -clippingElement.Height + contentHeight + contentY
    let offsets = `${top}px ${right}px ${bottom}px ${left}px`
    let round = ""
    if (
        clippingElement.RadiusTopLeft != undefined
        || clippingElement.RadiusTopRight != undefined
        || clippingElement.RadiusBottomLeft != undefined
        || clippingElement.RadiusBottomRight != undefined
    ) {
        round = ` round ${clippingElement.RadiusTopLeft || 0}px ${clippingElement.RadiusTopRight || 0}px ${clippingElement.RadiusBottomRight || 0}px ${clippingElement.RadiusBottomLeft || 0}px`
    }
    return `inset(${offsets}${round})`
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

        Width: e => First(e.LayoutWidth, e.InternalWidth),
        Height: e => First(e.LayoutHeight, e.InternalHeight),

        ClipByParent: false
        
    }

    DepthToStyle(element)


    XToStyle(element);
    YToStyle(element);
    WidthToStyle(element)
    HeightToStyle(element)

    //element.style.clipPath = "path('M 0 200 L 0,75 A 5,5 0,0,1 150,75 L 200 200 z')"

    new Reaction(() => {
        if (element.Parent == undefined)
            return

        if (element.ClipByParent) {

            element.style.clipPath = GetClipRect(element.Parent, element.X, element.Y, element.Width, element.Height)


            /*let left = -element.X
            let top = -element.Y
            let right = -element.Parent.Width + element.Width - element.X
            let bottom = -element.Parent.Height + element.Height - element.Y
            let offsets = `${top}px ${right}px ${bottom}px ${left}px`
            let round = ""
            if (
                element.Parent.RadiusTopLeft != undefined
                || element.Parent.RadiusTopRight != undefined
                || element.Parent.RadiusBottomLeft != undefined
                || element.Parent.RadiusBottomRight != undefined
            ) {
                round = ` round ${element.Parent.RadiusTopLeft || 0}px ${element.Parent.RadiusTopRight || 0}px ${element.Parent.RadiusBottomRight || 0}px ${element.Parent.RadiusBottomLeft || 0}px`
            }
            element.style.clipPath = `inset(${offsets}${round})`*/
        } else {
            element.style.clipPath = ""
        }

    })


    


    new Reaction(() => {
        element.style.fontSize = ToCssSize(element.FontSize)
    })

    element.Events.MouseEnter = () => element.Hover = true
    element.Events.MouseLeave = () => element.Hover = false


    


}
