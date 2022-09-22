
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
        //OverlayDepth: () => element.Parent.OverlayDepth + element.overlaySign != 0 ? 1 : 0,

        //Depth: () => element.Parent.Depth + (element.overlaySign == 0) ? Depth.nestingIncrement : Depth.getOverlayIncrement(element.OverlayDepth),

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

        //FontSize: undefined,
        HierarchyFontSize: () => element.FontSize || element.Parent.HierarchyFontSize,

        //public Expression<Func< FinalJs, float >> FontSize { set { AssignProperty(value); } }

        Hover: false
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




    XToStyle(element);

    YToStyle(element);

    new Reaction(() => {
        element.style.paddingLeft = ToCssSize(element.PaddingLeft)
    })
    new Reaction(() => {
        element.style.paddingRight = ToCssSize(element.PaddingRight)
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
