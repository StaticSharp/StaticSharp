
if (!window.LayoutAlgorithms) window.LayoutAlgorithms = {}

LayoutAlgorithms.SequenceMeasure = function (vertical, container, children, gap) {
    let region = LinearLayoutRegion.formContainer(container, vertical)
    for (const [i, child] of children.entries()) {
        if (i > 0) {
            region.border[0].ShiftByGap(gap)
        }
        region.border[0].Shift(child)
    }
    return region.GetSize()
}


LayoutAlgorithms.SequenceLayoutOverflowShrink = function (vertical, container, children, containerSize, measuredContainerSize, gap) {
    var names = new LayoutPropertiesNames(vertical)
    let region = LinearLayoutRegion.formContainer(container, vertical)

    let shrinkRate = containerSize / measuredContainerSize

    for (const [i, child] of children.entries()) {
        if (i > 0) {
            var currentGap = gap * shrinkRate
            region.border[0].ShiftByGap(currentGap)
        }

        let size = child.Layer[names.dimension]
        let position = region.border[0].Shift(child, size)

        

        child.Layer[names.coordinate] = position * shrinkRate
        child.Layer[names.dimension] = size * shrinkRate

    }
}


LayoutAlgorithms.SequenceOverflowRemove = function (vertical, container, children, containerSize, gap, ellipsis) {
    const visibleChildren = []
    let measuredContainerSize = 0
    var names = new LayoutPropertiesNames(vertical)
    let region = LinearLayoutRegion.formContainer(container, vertical)

    if (ellipsis) {
        region.border[1].Shift(ellipsis)
        ellipsis.Layer.Exists = true
    }

    for (const [i, child] of children.entries()) {
        if (i > 0) {
            region.border[0].ShiftByGap(gap)
        }

        //let size = child.Layer[names.dimension]
        let position = region.border[0].Shift(child)
        //child.Layer[names.coordinate] = position
        //child.Layer[names.dimension] = size

        const currentSize = region.GetSize()

        if (currentSize > containerSize) {
            child.Layer.Exists = false
        } else {
            child.Layer.Exists = true
            visibleChildren.push(child)
            measuredContainerSize = currentSize
        }       
    }

    if (ellipsis) {
        visibleChildren.push(ellipsis)
    }

    return {
        measuredContainerSize,
        visibleChildren
    }
}



LayoutAlgorithms.SequenceLayout = function (vertical, container, children, containerSize, measuredContainerSize, gravity, itemGrow, gap, gapGrow, startGapGrow = 0, endGapGrow = 0) {
    var containerSize = vertical ? container.Height : container.Width

    var names = new LayoutPropertiesNames(vertical)
    let region = LinearLayoutRegion.formContainer(container, vertical)



    let extraPixels = containerSize - measuredContainerSize
    
    let growUnits = 0
    let pixelPerUnit = 0

    if (gravity == undefined) {
        growUnits = itemGrow * children.length + gapGrow * (children.length - 1) + startGapGrow + endGapGrow
        pixelPerUnit = (growUnits != 0) ? extraPixels / growUnits : 0
    } else {
        let offset = (0.5 * gravity + 0.5) * extraPixels
        region.border[0].ShiftByPixels(offset)
    }

    //console.log(container.id, "growUnits", growUnits)
    for (const [i, child] of children.entries()) {
        
        if (i > 0) {
            region.border[0].ShiftByGap(gap)

            var extraGapPixels = gapGrow * pixelPerUnit
            //console.log(container.id, "extraGapPixels", extraGapPixels)
            region.border[0].ShiftByPixels(extraGapPixels)
        } else {
            region.border[0].ShiftByPixels(startGapGrow * pixelPerUnit)
        }

        let size = child.Layer[names.dimension] || 0
        //console.log(container.id, "size", size)
        size = size + itemGrow * pixelPerUnit
        
        let position = region.border[0].Shift(child, size)
        child.Layer[names.coordinate] = position
        child.Layer[names.dimension] = size
    }
    
}

LayoutAlgorithms.ParallelMeasure = function (vertical, container, children) {
    var names = new LayoutPropertiesNames(vertical)

    let result = Num.Sum(container["Padding" + names.side[0]], container["Padding" + names.side[1]], 0)

    for (let child of children) {
        let firstOffset = CalcOffset(container, child, names.side[0])
        let lastOffset = CalcOffset(container, child, names.side[1])
        let current = Num.Sum(child.Layer[names.dimension], firstOffset + lastOffset)
        result = Num.Max(result, current)
    }
    return result
}

LayoutAlgorithms.ParallelLayout = function (vertical, container, children, defaultGravity) {
    var containerSize = vertical ? container.Height : container.Width
    
    if (Num.IsNaNOrNull(containerSize))
        return

    var names = new LayoutPropertiesNames(vertical)
    /*if (!vertical) {
        console.log("defaultGravity", defaultGravity)
    }*/
    if (defaultGravity == undefined) {
        for (let child of children) {
            let firstOffset = CalcOffset(container, child, names.side[0])
            let lastOffset = CalcOffset(container, child, names.side[1])
            let size = containerSize - firstOffset - lastOffset

            //console.log(names.coordinate, firstOffset)

            child.Layer[names.coordinate] = firstOffset
            child.Layer[names.dimension] = size
        }
    } else {
        for (let child of children) {
            let firstOffset = CalcOffset(container, child, names.side[0])
            let lastOffset = CalcOffset(container, child, names.side[1])

            let childSize = child.Layer[names.dimension]
            let availableSize = containerSize - firstOffset - lastOffset
            let extraPixels = availableSize - childSize

            if (extraPixels > 0) {
                firstOffset += (0.5 * defaultGravity + 0.5) * extraPixels
            } else {
                childSize = availableSize
            }
            
            child.Layer[names.coordinate] = firstOffset
            child.Layer[names.dimension] = childSize
        }
    }
}


StaticSharpClass("StaticSharp.LinearLayout", (element) => {
    StaticSharp.BlockWithChildren(element)

    CreateSocket(element, "Ellipsis", element)

    element.Reactive = {

        Vertical: true,

        ItemGrow: 1,
        Gap: 0,
        GapGrow: 0,
        StartGapGrow: 0,
        EndGapGrow: 0,

        PrimaryGravity: undefined,
        SecondaryGravity: undefined,
        Reverse: false,

        OrderedChildren: e => {
            if (e.Reverse)
                return e.Children.Where(x => x.Layer.Exists).Reverse().ToArray()
            else
                return e.Children.Where(x => x.Layer.Exists).ToArray()
        },

        InternalSecondarySize: e => LayoutAlgorithms.ParallelMeasure(!e.Vertical, e, e.OrderedChildren),
        InternalPrimarySize: e => LayoutAlgorithms.SequenceMeasure(e.Vertical, e, e.OrderedChildren, e.Gap),

        InternalWidth: e => e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,
        InternalHeight: e => !e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,

        PrimarySize : e => e.Vertical ? e.Height : e.Width,
        SecondarySize : e => e.Vertical ? e.Width : e.Height,

        Overflow: "Shrink",


    }


    let baseHtmlNodesOrdered = element.HtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* baseHtmlNodesOrdered
        const ellipsis = element.Ellipsis
        if (ellipsis && ellipsis.Exists)
            yield ellipsis
    })


    new Reaction(() => {
        const children = element.OrderedChildren.filter(x=>x.Exists)
        const ellipsis = element.Ellipsis
        if (ellipsis && ellipsis.Exists)
            children.push(ellipsis)
        //console.log("children", children)
        LayoutAlgorithms.ParallelLayout(!element.Vertical, element, children, element.SecondaryGravity)
    })

    new Reaction(() => {
        const vertical = element.Vertical
        var containerSize = vertical ? element.Height : element.Width
        var measuredContainerSize = element.InternalPrimarySize


        //

        if (measuredContainerSize > containerSize) {
            //console.log(element.OrderedChildren)
            if (element.Overflow == "Shrink") {
                LayoutAlgorithms.SequenceLayoutOverflowShrink(
                    vertical,
                    element,
                    element.OrderedChildren,

                    containerSize,
                    measuredContainerSize,
                    element.Gap)
            } else {
                var removeResult = LayoutAlgorithms.SequenceOverflowRemove(
                    vertical,
                    element,
                    element.OrderedChildren,

                    containerSize,
                    element.Gap,
                    element.Ellipsis
                )
                
                LayoutAlgorithms.SequenceLayout(
                    vertical,
                    element,
                    removeResult.visibleChildren,

                    containerSize,
                    removeResult.measuredContainerSize,

                    element.PrimaryGravity,
                    element.ItemGrow,
                    element.Gap,
                    element.GapGrow,
                    element.StartGapGrow,
                    element.EndGapGrow,

                )
            }

        } else {
            const ellipsis = element.Ellipsis
            if (ellipsis)
                ellipsis.Layer.Exists = false

            for (let i of element.Children) {
                i.Layer.Exists = i.Layer.Exists
            }
            //console.log(element.id, "element.GapGrow", element.GapGrow, containerSize, measuredContainerSize)
            LayoutAlgorithms.SequenceLayout(
                vertical,
                element,
                element.OrderedChildren,

                containerSize,
                measuredContainerSize,

                element.PrimaryGravity,
                element.ItemGrow,
                element.Gap,
                element.GapGrow,
                element.StartGapGrow,
                element.EndGapGrow,

            )
        }

        
        
    })


})
