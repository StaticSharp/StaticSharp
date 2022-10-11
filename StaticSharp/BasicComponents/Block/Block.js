
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
    

    BaseModifier(element)

    element.isBlock = true



    element.Reactive = {
        
        Depth: () => {
            if (element.overlaySign == 0) {
                return 0;
            } else {
                if (element.overlaySign == 1) {//overlay
                    return Sum(element.OverlayIndex * 100, - element.Parent.NestingDepth)
                } else {//underlay
                    return -1
                }
            }
        },
        OverlayIndex: () => element.IsRoot ? 0 : (element.overlaySign ? Sum(element.Parent.OverlayIndex, 1) : element.Parent.OverlayIndex),

        Overlay: undefined,
        Underlay: undefined,



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

        ScrollX: undefined,
        ScrollY: undefined,

        ScrollXActual: 0,
        ScrollYActual: 0,
        
    }

    DepthToStyle(element)

    if (element.overlaySign != 0) {
        new Reaction(() => {
            let top = element.MarginTop
            let bottom = element.MarginBottom
            let left =   element.MarginLeft
            let right = element.MarginRight

            element.LayoutX = First(left,0)
            element.LayoutY = First(top,0)

            element.LayoutWidth = Sum( element.Parent.Width, -left, -right)
            element.LayoutHeight = Sum( element.Parent.Height, -top, -bottom)
        })
    }

    if (element.overlaySign == 1) {
        new Reaction(() => {
            element.style.left = ToCssSize(element.AbsoluteX)
        })
        new Reaction(() => {
            element.style.top = ToCssSize(element.AbsoluteY)
        })
    } else {
        XToStyle(element);
        YToStyle(element);
    }

    WidthToStyle(element)
    HeightToStyle(element)

    

    new Reaction(() => {
        let l = element.PaddingLeft
        let r = element.PaddingRight
        let w = element.Width
        if (l + r > w) {
            let m = w / (l + r)
            l *= m
            r *= m
        }
        element.style.paddingLeft = ToCssSize(l)
        element.style.paddingRight = ToCssSize(r)
    })
    new Reaction(() => {
        
    })

    new Reaction(() => {
        element.style.paddingTop = ToCssSize(element.PaddingTop)
    })
    new Reaction(() => {
        element.style.paddingBottom = ToCssSize(element.PaddingBottom)
    })



    new Reaction(() => {
        element.style.fontSize = ToCssSize(element.FontSize)
    })

    element.Events.MouseEnter = () => element.Hover = true
    element.Events.MouseLeave = () => element.Hover = false


    element.Events.Scroll = () => {
        let d = Reaction.beginDeferred()
        element.ScrollXActual = element.scrollLeft
        element.ScrollYActual = element.scrollTop
        d.end()
    }


}
