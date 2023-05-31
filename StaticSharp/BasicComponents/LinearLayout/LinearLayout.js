
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

LayoutAlgorithms.SequenceLayout = function (vertical, container, children, measuredContainerSize, gravity, itemGrow, gap, gapGrow, startGapGrow = 0, endGapGrow = 0) {
    var containerSize = vertical ? container.Height : container.Width

    var names = new LayoutPropertiesNames(vertical)
    let region = LinearLayoutRegion.formContainer(container, vertical)

    if (measuredContainerSize > containerSize) {//Shrink
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
    } else {

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


        for (const [i, child] of children.entries()) {
            if (i > 0) {
                var currentGap = gap + gapGrow * pixelPerUnit
                region.border[0].ShiftByGap(currentGap)
            } else {
                region.border[0].ShiftByGap(startGapGrow * pixelPerUnit)
            }

            let size = child.Layer[names.dimension] || 0

            size = size + itemGrow * pixelPerUnit

            let position = region.border[0].Shift(child, size)

            child.Layer[names.coordinate] = position
            child.Layer[names.dimension] = size
        }
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

    if (defaultGravity == undefined) {
        for (let child of children) {
            let firstOffset = CalcOffset(container, child, names.side[0])
            let lastOffset = CalcOffset(container, child, names.side[1])
            let size = containerSize - firstOffset - lastOffset
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




function LinearLayout(element) {
    BlockWithChildren(element)
    element.isLinearLayout = true

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
                return e.ExistingChildren.Reverse().ToArray()
            else
                return e.ExistingChildren.ToArray()
        },

        InternalSecondarySize: e => LayoutAlgorithms.ParallelMeasure(!e.Vertical, e, e.OrderedChildren),
        InternalPrimarySize: e => LayoutAlgorithms.SequenceMeasure(e.Vertical, e, e.OrderedChildren, e.Gap),

        InternalWidth: e => e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,
        InternalHeight: e => !e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,

        PrimarySize : e => e.Vertical ? e.Height : e.Width,
        SecondarySize : e => e.Vertical ? e.Width : e.Height,

    }

    new Reaction(() => {
        LayoutAlgorithms.ParallelLayout(!element.Vertical, element, element.OrderedChildren, element.SecondaryGravity)
    })

    new Reaction(() => {
        LayoutAlgorithms.SequenceLayout(element.Vertical, element, element.OrderedChildren, element.InternalPrimarySize, element.PrimaryGravity, element.ItemGrow, element.Gap, element.GapGrow, element.StartGapGrow, element.EndGapGrow)
        
    })


}
