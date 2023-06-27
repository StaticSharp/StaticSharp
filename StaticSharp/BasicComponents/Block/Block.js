

function CalcOffset(container, child, sideName) {
    let paddingName = "Padding" + sideName
    let marginName = "Margin" + sideName
    if (container[paddingName] != undefined) {
        return Math.max(container[paddingName], child[marginName] || 0)
    }
    if (container[marginName] == undefined) {
        return child[marginName] || 0
    }
    if (child[marginName] == undefined) {
        return 0
    }
    return Math.max(child[marginName] - container[marginName], 0)
}


/**
* Calculates offsets for an array of elements so that they are placed in sequence with minimal gap possible 
* (respecting margins and paddings)
* @param {Element} container - Container DOM element
* @param {Array[Element]} children - Array of children DOM elements
* @param {string} sideName - "Left", "Right", "Top", "Bottom" - side of container to start from
* @returns Array of offsets: X - if direction is "Left" or "Right", Y - otherwise
*/
//function CalcSequentialOffsets(container, children, sideName) {
//    let result = [];
//    let oppositeSideName = {
//        "Left": "Right",
//        "Right": "Left",
//        "Top": "Bottom",
//        "Bottom": "Top"
//    }[sideName];

//    let marginName = "Margin" + sideName
//    let oppositeMarginName = "Margin" + oppositeSideName
//    let sizeName = {
//        "Left": "Width",
//        "Right": "Width",
//        "Top": "Height",
//        "Bottom": "Height"
//    }[sideName]

//    if (sideName == "Right" || sideName == "Bottom") {
//        throw new Error("sideName = \"Right\" || sideName == \"Bottom\" - not supported");
//    }
    
//    let offset = CalcOffset(container, children[0], sideName)
//    result[0] = offset;
//    let previousOppositeMargin = First(children[0][oppositeMarginName], 0)
//    offset += children[0][sizeName]

//    for (let i = 1; i < children.length; i++) {
//        let margin = Max(previousOppositeMargin, children[i][marginName], 0)
//        result[i] = offset + margin
//        offset += children[i][sizeName] + margin
//        previousOppositeMargin = First(children[i][oppositeMarginName], 0)
//    }

//    return result;
//}

function GetClipRect(clippingElement, contentX, contentY, contentWidth, contentHeight ) {
    let left = -contentX
    let top = -contentY
    let right = -clippingElement.Width + contentWidth + contentX
    let bottom = -clippingElement.Height + contentHeight + contentY
    let offsets = `${top}px ${right}px ${bottom}px ${left}px`

    let round = ""
    let borderRadius = as(clippingElement, "BorderRadius")
    if (borderRadius != undefined) {
        round = ` round ${borderRadius.RadiusTopLeft || 0}px ${borderRadius.RadiusTopRight || 0}px ${borderRadius.RadiusBottomRight || 0}px ${borderRadius.RadiusBottomLeft || 0}px`
    }

    return `inset(${offsets}${round})`
}

StaticSharpClass("StaticSharp.Block", (element) => {
    StaticSharp.BaseModifier(element)


    element.Reactive = {
        
        Depth: undefined,

        PaddingLeft: undefined,
        PaddingTop: undefined,
        PaddingRight: undefined,
        PaddingBottom: undefined,

        MarginLeft: undefined,
        MarginTop: undefined,
        MarginRight: undefined,
        MarginBottom: undefined,
        
        X: 0,
        Y: 0,

        AbsoluteX: () => {
            let parent = element.Parent
            if (parent) {
                return Num.Sum(element.Parent.AbsoluteX, element.Parent.ScrollX, element.X)
            } else {
                return 0
            }
        },
        AbsoluteY: () => {
            let parent = element.Parent
            if (parent) {
                return Num.Sum(element.Parent.AbsoluteY, -element.Parent.ScrollY, element.Y)
            } else {
                return 0
            }
        },

        Width: 0, //e => e.InternalWidth, // e => First(e.LayoutWidth, e.PreferredWidth),
        Height: 0, //e => e.InternalHeight, // e => First(e.LayoutHeight, e.PreferredHeight),

        ViewportVisibility: e => {

            const vw = e.Root.Width
            if (!Num.ValidNumber(vw) || vw==0) return 0

            const vh = e.Root.Height
            if (!Num.ValidNumber(vh) || vw == 0) return 0

            

            const w = e.Width
            if (!Num.ValidNumber(w)) return 0

            const h = e.Height
            if (!Num.ValidNumber(h)) return 0
            const x = e.AbsoluteX
            if (!Num.ValidNumber(x)) return 0
            const y = e.AbsoluteY
            if (!Num.ValidNumber(y)) return 0

            const x0 = Math.max(x, 0)
            const x1 = Math.min(x + w, vw)
            
            const y0 = Math.max(y || 0, 0)
            const y1 = Math.min(y + h, vh)

            const xr = Math.max(x1 - x0, 0) / w
            const yr = Math.max(y1 - y0, 0) / h
            return xr*yr
        },


        FontSize: undefined,
        HierarchyFontSize: () => element.FontSize || element.Parent.HierarchyFontSize,


        //ClipByParent: false
    }


    new Reaction(() => {
        let depth = element.Depth
        element.style.zIndex = depth == undefined ? "" : depth
    })

    XToStyle(element);
    YToStyle(element);
    WidthToStyle(element)
    HeightToStyle(element)


    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* element.ExistingUnmanagedChildren
    })

    new Reaction(() => {
        let tergetChildren = [...element.HtmlNodesOrdered]
        SyncChildren(element, tergetChildren)
    })


    new Reaction(() => {
        element.style.overflow = element.ClipChildren ? "clip" : ""        
    })


    

})
