
/**
 * @constructor
 * @param {boolean} vertical
*/
function LayoutPropertiesNames(vertical) {

    /**@type {Array<string>}*/
    this.side = [
        vertical ? "Top" : "Left",
        vertical ? "Bottom" : "Right"
    ]
    /**@type {"Y" | "X"}*/
    this.cordinate = vertical ? "Y" : "X"

    /**@type {"Height" | "Width"}*/
    this.dimension = vertical ? "Height" : "Width"
}



function LayoutBorder(sideIndex, layoutPrpertiesNames, marginStop = 0, bodyStop = 0) {
    this.layoutPrpertiesNames = layoutPrpertiesNames
    this.sideIndex = sideIndex
    this.oppositeSideIndex = (sideIndex + 1) % 2
    //this.sideOpposite = layoutPrpertiesNames.side[(sideIndex+1)%2]
    this.marginStop = marginStop
    this.bodyStop = bodyStop
}

LayoutBorder.prototype.ShiftMaxOf = function (children) {    

    let marginStop = 0
    let bodyStop = 0

    for (let child of children) {
        let margin = child["Margin" + this.layoutPrpertiesNames.side[this.sideIndex]] || 0
        let marginOpposite = child["Margin" + this.layoutPrpertiesNames.side[this.oppositeSideIndex]] || 0
        let size = child.Layer[this.layoutPrpertiesNames.dimension] || 0

        let position = Math.max(this.bodyStop, this.marginStop + margin)

        marginStop = Math.max(marginStop, position + size)
        bodyStop = Math.max(bodyStop, this.marginStop + marginOpposite)
    }

    this.marginStop = marginStop
    this.bodyStop = bodyStop
}

LayoutBorder.prototype.Shift = function (child, size = undefined) {


    let margin = child["Margin" + this.layoutPrpertiesNames.side[this.sideIndex]] || 0
    let marginOpposite = child["Margin" + this.layoutPrpertiesNames.side[this.oppositeSideIndex]] || 0
    if (size == undefined)
        size = child.Layer[this.layoutPrpertiesNames.dimension] || 0

    let position = Math.max(this.bodyStop, this.marginStop + margin)

    this.marginStop = position + size
    this.bodyStop = this.marginStop + marginOpposite

    return position
}

LayoutBorder.prototype.ShiftByPixels = function (pixels) {
    this.marginStop += pixels
    this.bodyStop += pixels
}




function LinearLayoutRegion(vertical) {
    let names = new LayoutPropertiesNames(vertical)
    this.vertical = vertical
    this.border = [
        new LayoutBorder(0, names),
        new LayoutBorder(1, names),
    ]
}

LinearLayoutRegion.prototype.GetSize = function () {
    return Math.max(
        this.border[0].marginStop + this.border[1].bodyStop,
        this.border[1].marginStop + this.border[0].bodyStop
    )
}


LinearLayoutRegion.formContainer = function (container, vertical) {
    let result = new LinearLayoutRegion(vertical)

    let names = new LayoutPropertiesNames(vertical)

    /**@type {Array<number>}*/
    let padding = [
        container["Padding" + names.side[0]],
        container["Padding" + names.side[1]],
    ]

    for (let i = 0; i < 2; i++) {
        
        result.border[i].bodyStop = 0
        result.border[i].marginStop = 0
        if (padding[i] != undefined) {
            result.border[i].bodyStop = padding[i]
        } else {
            let margin = container["Margin" + names.side[i]]
            if (margin !== undefined)
                result.border[i].marginStop = -margin
        }
    }
    return result
}



function LinearLayout(element) {
    Block(element)

    element.Reactive = {

        Vertical: true,

        ItemGrow: 1,
        Gap: 0,
        GapGrow: 0,

        PrimaryGravity: undefined,
        SecondaryGravity: undefined,
        Reverse: false,

        OrderedChildren: e => {
            if (e.Reverse)
                return e.Children.Reverse().ToArray()
            else
                return e.Children.ToArray()
        },

        InternalSecondarySize: e => {
            var names = new LayoutPropertiesNames(!element.Vertical)

            let result = Sum(e["Padding" + names.side[0]], e["Padding" + names.side[1]], 0)

            for (let child of e.OrderedChildren) {
                let firstOffset = CalcOffset(e, child, names.side[0])
                let lastOffset = CalcOffset(e, child, names.side[1])

                let current = Sum(child.Layer[names.dimension], firstOffset + lastOffset)

                result = Max(result, current)
            }
            //console.log("InternalSecondarySize", result)
            return result
        },

        InternalPrimarySize: e => {
            //var names = new LayoutPrpertiesNames(element.Vertical)
            let region = LinearLayoutRegion.formContainer(e, e.Vertical)
            let gap = e.Gap
            for (const [i, child] of e.OrderedChildren.entries()) {
                if (i > 0) {
                    region.border[0].ShiftByPixels(gap)
                }
                region.border[0].Shift(child)
            }
            return region.GetSize()
        },

        InternalWidth: e => e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,
        InternalHeight: e => !e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,

        PrimarySize : e => e.Vertical ? e.Height : e.Width,
        SecondarySize : e => e.Vertical ? e.Width : e.Height,

    }

    /*new Reaction(() => {
        console.log(element.ItemGrow)
    })*/

    new Reaction(() => {
        var names = new LayoutPropertiesNames(!element.Vertical)

        var internalSize = element.SecondarySize

        if (element.SecondaryGravity == undefined) {
            for (let child of element.OrderedChildren) {
                let firstOffset = CalcOffset(element, child, names.side[0])
                let lastOffset = CalcOffset(element, child, names.side[1])

                child.Layer[names.cordinate] = firstOffset
                child.Layer[names.dimension] = internalSize - firstOffset - lastOffset                
            }
        } else {
            for (let child of element.OrderedChildren) {
                let firstOffset = CalcOffset(element, child, names.side[0])
                let lastOffset = CalcOffset(element, child, names.side[1])

                let childSize = child.Layer[names.dimension]
                let availableSize = internalSize - firstOffset - lastOffset
                let extraPixels = availableSize - childSize

                if (extraPixels > 0) {
                    firstOffset += (0.5 * element.SecondaryGravity + 0.5) * extraPixels
                } else {
                    childSize = availableSize
                }

                child.Layer[names.cordinate] = firstOffset
                child.Layer[names.dimension] = childSize
            }
        }        
    })
   


    new Reaction(() => {

        let children = element.OrderedChildren
        var names = new LayoutPropertiesNames(element.Vertical)
        let region = LinearLayoutRegion.formContainer(element, element.Vertical)
        let gap = element.Gap
        if (element.InternalPrimarySize > element.PrimarySize) {//Shrink
            let shrinkRate = element.PrimarySize / element.InternalPrimarySize

            for (const [i, child] of children.entries()) {
                if (i > 0)
                    region.border[0].ShiftByPixels(gap * shrinkRate)

                let size = child.Layer[names.dimension]
                let position = region.border[0].Shift(child, size)
                child.Layer[names.cordinate] = position * shrinkRate
                child.Layer[names.dimension] = size * shrinkRate
                
            }
        } else {

            let itemGrow = 0
            let gapGrow = 0
            let extraPixels = element.PrimarySize - element.InternalPrimarySize
            let growUnits = 0
            let pixelPerUnit = 0

            if (element.PrimaryGravity == undefined) {
                itemGrow = element.ItemGrow
                gapGrow = element.GapGrow
                growUnits = itemGrow * children.length + gapGrow * (children.length - 1)
                pixelPerUnit = (growUnits != 0) ? extraPixels / growUnits : 0
            } else {
                let offset = (0.5 * element.PrimaryGravity + 0.5) * extraPixels
                region.border[0].ShiftByPixels(offset)
            }


            for (const [i, child] of children.entries()) {
                if (i > 0)
                    region.border[0].ShiftByPixels(gap + gapGrow * pixelPerUnit)

                let size = child.Layer[names.dimension] || 0
                    
                size = size + itemGrow * pixelPerUnit
                    
                let position = region.border[0].Shift(child, size)

                child.Layer[names.cordinate] = position
                child.Layer[names.dimension] = size


                //console.log("size", size, child.Layer.originalProperties.get(names.dimension))

            }
            
        }
    })


}
