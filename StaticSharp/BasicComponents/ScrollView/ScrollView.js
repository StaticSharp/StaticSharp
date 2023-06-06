
StaticSharpClass("StaticSharp.Thumb", (element) => {
    StaticSharp.Block(element)
    element.Reactive = {
        BackgroundColor: () => element.Parent.HierarchyForegroundColor,

        Radius: () => Num.Min(element.Width, element.Height) / 2,
        Depth: 1,
        Visibility: () => (window.Touch || element.ThumbSizeScale >= 1) ? 0 : element.Hover ? 0.5 : 0.25,
    }
})

function SetupPointerDrag(element, func) {
    element.Events.PointerDown = () => {
        event.stopPropagation()
        event.preventDefault()
        element.setPointerCapture(event.pointerId)

        element.Events.PointerMove = () => {
            let x = event.movementX
            let y = event.movementY
            if (x != 0 || y != 0) {
                func(x,y)
            }            
        }
        element.Events.PointerUp = () => {
            element.Events.PointerUp = undefined
            element.Events.PointerMove = undefined
        }
        return false
    }
}



StaticSharpClass("StaticSharp.ScrollView", (element) => {
    StaticSharp.Block(element)


    var modificationSource = undefined
    let scrollable = document.createElement("scrollable")
    scrollable.style.touchAction = "manipulation"
    scrollable.style.overflow = "auto"

    scrollable.Events.Scroll = () => {
        if (modificationSource == "Property")
            return
        modificationSource = "Event"
        let d = Reaction.beginDeferred()
        element.ScrollX = scrollable.scrollLeft
        element.ScrollY = scrollable.scrollTop
        d.end()
        modificationSource = undefined
    }


    element.Reactive = {
        InternalWidth: () => Num.Sum(element.Child.Layer.Width, element.LeftOffset, element.RightOffset),
        InternalHeight: () => Num.Sum(element.Child.Layer.Height, element.TopOffset, element.BottomOffset),

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,

        LeftOffset: () => CalcOffset(element, element.Child, "Left"),
        RightOffset: () => CalcOffset(element, element.Child, "Right"),
        TopOffset: () => CalcOffset(element, element.Child, "Top"),
        BottomOffset: () => CalcOffset(element, element.Child, "Bottom"),

        ChildAreaWidth: () => element.Width - element.LeftOffset - element.RightOffset,
        ChildAreaHeight: () => element.Height - element.TopOffset - element.BottomOffset,

        ScrollX: undefined,
        ScrollY: undefined, //Storage.Store("scroll", () => element.ScrollYActual),

        /*ScrollXActual: 0,
        ScrollYActual: 0,*/

        ScrollBarThickness: 4,
        ScrollBarMargin: 2,
        ShowScrollBars: () => !window.Touch,

    }
    CreateSocket(element, "Child", element)
    CreateSocket(element, "VerticalThumb", element).setValue(Create(element, StaticSharp.Thumb))
    CreateSocket(element, "HorizontalThumb", element).setValue(Create(element, StaticSharp.Thumb))

    let verticalThumb = element.VerticalThumb
    verticalThumb.Reactive = {
        ThumbTravel: () => element.Height - 2 * element.ScrollBarMargin,
        ThumbPositionScale: () => verticalThumb.ThumbTravel / element.Child./*InternalHeight*/Layer.Height,
        ThumbSizeScale: () => element.ChildAreaHeight / element.Child./*InternalHeight*/Layer.Height, 
        X: () => element.Width - verticalThumb.Width - element.ScrollBarMargin,
        Y: () => element.ScrollBarMargin + element.ScrollY * verticalThumb.ThumbPositionScale,
        Width: () => element.ScrollBarThickness,        
        Height: () => verticalThumb.ThumbTravel * verticalThumb.ThumbSizeScale,
    }
    

    let horizontalThumb = element.HorizontalThumb
    horizontalThumb.Reactive = {
        ThumbTravel: () => element.Width - 2 * element.ScrollBarMargin,
        ThumbPositionScale: () => horizontalThumb.ThumbTravel / element.Child.Width,
        ThumbSizeScale: () => element.ChildAreaWidth / element.Child.Width,        
        X: () => element.ScrollBarMargin + element.ScrollX * horizontalThumb.ThumbPositionScale,
        Y: () => element.Height - horizontalThumb.Height - element.ScrollBarMargin,
        Width: () => horizontalThumb.ThumbTravel * horizontalThumb.ThumbSizeScale,
        Height: () => element.ScrollBarThickness,        
    }


    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield verticalThumb
        yield horizontalThumb
        yield scrollable
        yield* element.UnmanagedChildren
    })

    new Reaction(() => {
        SyncChildren(scrollable, [element.Child])
    })

    SetupPointerDrag(verticalThumb, (x, y) => {
        scrollable.scrollTop += y / verticalThumb.ThumbPositionScale
    })

    SetupPointerDrag(horizontalThumb, (x, y) => {
        scrollable.scrollLeft += x / horizontalThumb.ThumbPositionScale
    })


    

    
    new Reaction(() => {
        element.Child.Layer.Width = Math.max(element.Child.Layer.Width, element.ChildAreaWidth)
        element.Child.Layer.Height = Math.max(element.Child.Layer.Height, element.ChildAreaHeight)
    })

    
    
    new Reaction(() => {
        scrollable.style.left = ToCssSize(element.LeftOffset)
        scrollable.style.top = ToCssSize(element.TopOffset)
        scrollable.style.width = ToCssSize(element.ChildAreaWidth)
        scrollable.style.height = ToCssSize(element.ChildAreaHeight)
    })



    new Reaction(() => {

        element.ScrollY
        element.ScrollX

        if (modificationSource == "Event")
            return
        modificationSource = "Property"
        
        window.requestAnimationFrame(() => {
            scrollable.scrollTop = element.ScrollY
            scrollable.scrollLeft = element.ScrollX
            modificationSource = undefined
        });

        
        
    })

})