




function LinearLayout(element) {
    BlockWithChildren(element)

    


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
                return e.ExistingChildren.Reverse().ToArray()
            else
                return e.ExistingChildren.ToArray()
        },

        InternalSecondarySize: e => {
            var names = new LayoutPropertiesNames(!element.Vertical)

            let result = Num.Sum(e["Padding" + names.side[0]], e["Padding" + names.side[1]], 0)

            for (let child of e.OrderedChildren) {
                let firstOffset = CalcOffset(e, child, names.side[0])
                let lastOffset = CalcOffset(e, child, names.side[1])

                let current = Num.Sum(child.Layer[names.dimension], firstOffset + lastOffset)

                result = Num.Max(result, current)
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

        var secondarySize = element.SecondarySize
        if (secondarySize == undefined)
            return

        var names = new LayoutPropertiesNames(!element.Vertical)

        if (element.SecondaryGravity == undefined) {
            for (let child of element.OrderedChildren) {
                let firstOffset = CalcOffset(element, child, names.side[0])
                let lastOffset = CalcOffset(element, child, names.side[1])
                let size = secondarySize - firstOffset - lastOffset
                child.Layer[names.cordinate] = firstOffset
                child.Layer[names.dimension] = size               
            }
        } else {
            for (let child of element.OrderedChildren) {
                let firstOffset = CalcOffset(element, child, names.side[0])
                let lastOffset = CalcOffset(element, child, names.side[1])

                let childSize = child.Layer[names.dimension]
                let availableSize = secondarySize - firstOffset - lastOffset
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

        if (element.PrimarySize == undefined)
            return

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
