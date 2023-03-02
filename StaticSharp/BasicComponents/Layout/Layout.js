


/**
 * @constructor
 * @param {Block} element
 * @param {LayoutBlock} layoutBlock
 */
function LayoutItem(element,layoutBlock) {

    
    /**@type {LayoutBlock}*/ this.layoutBlock = layoutBlock
    /**@type {Block}*/ this.element = element
    
    /**@type {number}*/ this.primaryMargin = element["Margin" + layoutBlock.primarySide] || 0
    /**@type {number}*/ this.primaryMarginOpposite = element["Margin" + layoutBlock.primarySideOpposite] || 0
    /**@type {number}*/ this.secondaryMargin = element["Margin" + layoutBlock.secondarySide] || 0
    /**@type {number}*/ this.secondaryMarginOpposite = element["Margin" + layoutBlock.secondarySideOpposite] || 0

    

    /**@type {number}*/ this.primarySize = element["Preferred" + layoutBlock.primaryDimension] || 0
    /**@type {number}*/ this.secondarySize = element["Preferred" + layoutBlock.secondaryDimension] || 0

    /**@type {number}*/ this.primaryMinSize = element["Min" + layoutBlock.primaryDimension] || 0
    /**@type {number}*/ //this.secondaryMinSize = element["Min" + layoutBlock.secondaryDimension] || 0
    /**@type {number}*/ this.primaryMaxSize = element["Max" + layoutBlock.primaryDimension] || 0
    /**@type {number}*/ //this.secondaryMaxSize = element["Max" + layoutBlock.secondaryDimension] || 0

    /**@type {number}*/
    this.shrink = element.Shrink || 0
    if (this.shrink < 0) this.shrink = 0
    let minimalSize = this.shrink == 0 ? this.primarySize : this.primaryMinSize

    this.grow = element.Grow || 0
    if (this.grow < 0) this.grow = 0
    let maximalSize = this.grow == 0 ? this.primarySize : this.primaryMaxSize

    /**@type {number}*/
    this.shrinkPixels = this.primarySize - minimalSize
    /**@type {number}*/
    this.growPixels = maximalSize - this.primarySize


    /**@type {number}*/ this.primaryPosition = 0
    /**@type {number}*/ this.secondaryPosition = 0

    /**@type {LayoutLine}*/ this.line = undefined
}

LayoutItem.prototype.Write = function () {    
    this.element["Layout" + this.layoutBlock.primaryCoordinate] = this.primaryPosition
    this.element["Layout" + this.layoutBlock.primaryDimension] = this.primarySize

    this.element["Layout" + this.layoutBlock.secondaryCoordinate] = this.line.secondaryPosition + this.secondaryPosition
    this.element["Layout" + this.layoutBlock.secondaryDimension] = this.secondarySize
}


/**@constructor
 * @param {LayoutBlock} layoutBlock
 */
function LayoutLine(layoutBlock) {
    /**@type {LayoutBlock}*/
    this.layoutBlock = layoutBlock

    /**@type {number}*/
    this.marginStop = layoutBlock.primaryMarginStop

    /**@type {number}*/
    this.bodyStop = layoutBlock.primaryBodyStop


    /**@type {number}*/
    this.secondaryPosition = 0

    /**@type {number}*/
    this.secondaryMargin = 0

    /**@type {number}*/
    this.secondaryMarginStop = 0

    /**@type {number}*/
    this.secondaryBodyStop = 0


    /**@type {LayoutItem[]}*/
    this.items = []

    /**@type {number}*/
    this.shrinkPixels = 0
    /**@type {number}*/
    this.growPixels = 0
}

/**
 * @param {LayoutItem} layoutItem
 * @returns {boolean}
 */
LayoutLine.prototype.AddChild = function (layoutItem, gap, sizeLimit = undefined) {

    if (this.items.length == 0)
        gap = 0

    layoutItem.primaryPosition = gap + Math.max(this.bodyStop, this.marginStop + layoutItem.primaryMargin)
    
    let marginStop = layoutItem.primaryPosition + layoutItem.primarySize
    let bodyStop = marginStop + layoutItem.primaryMarginOpposite

    this.shrinkPixels += layoutItem.shrinkPixels
    this.growPixels += layoutItem.growPixels

    if (sizeLimit != undefined) {
        let lineSize = Math.max(
            marginStop + this.layoutBlock.primaryBodyStopOpposite,
            bodyStop + this.layoutBlock.primaryMarginStopOpposite
        )
        //console.log("lineSize", lineSize, "this.shrinkPixels", this.shrinkPixels)
        if ((lineSize - this.shrinkPixels) > sizeLimit)
            return false
    }

    this.marginStop = marginStop
    this.bodyStop = bodyStop
    this.items.push(layoutItem)
    layoutItem.line = this
    return true
}

/**@returns {number}*/
LayoutLine.prototype.GetLineSize = function () {    

    let lineSize = Math.max(
        this.marginStop + this.layoutBlock.primaryBodyStopOpposite,
        this.bodyStop + this.layoutBlock.primaryMarginStopOpposite
    )
    return lineSize
}


/**
 * @param {number} gravity
 * @param {number} grow
 * */
LayoutLine.prototype.AlignPrimary = function (containerSize, gapGrow, gravity = 0) {
    let lineSize = this.GetLineSize()
    let minLineSize = lineSize - this.shrinkPixels
    
    if (lineSize > containerSize) {//Shrink
        let units = 0
        for (let i of this.items) {
            units += i.shrink
        }
        if (units == 0)
            return
        
        let pixelsPerUnit = (lineSize - containerSize) / units

        let offset = 0
        for (let i of this.items) {
            let delta = i.shrink * pixelsPerUnit            
            i.primaryPosition -= offset
            i.primarySize -= delta
            offset += delta
        }
        return
    }
    
    if (lineSize < containerSize) {//Grow
        let maxLineSize = lineSize + this.growPixels

        let activeItems = this.items.filter(x => x.growPixels > 0)

        let units = (this.items.length - 1) * gapGrow

        let targetSize = 0
        if (units > 0) {//gaps can grow
            targetSize = containerSize
            
        }
        else {

            targetSize = Math.min(containerSize, maxLineSize)
            //console.log("targetSize", targetSize, activeItems)
        }

        for (let i of activeItems) {
            units += i.grow
        }
        
        let extraPixels = targetSize - lineSize

        if (units > 0) {
            while (activeItems.length > 0) {
                let pixelsPerUnit = extraPixels / units
                let limitedGrowth = false

                let nextIterationItems = []
                for (let i of activeItems) {

                    let itemExtraPixels = pixelsPerUnit * i.grow

                    if (itemExtraPixels > i.growPixels) {
                        limitedGrowth = true
                        itemExtraPixels = i.growPixels
                        extraPixels -= itemExtraPixels
                        units -= i.grow
                    } else {
                        nextIterationItems.push(i)
                    }

                    i.extraPixels = itemExtraPixels
                }
                if (limitedGrowth) {
                    activeItems = nextIterationItems
                } else {
                    for (let i of nextIterationItems) {
                        extraPixels -= i.extraPixels
                        units -= i.grow
                    }
                    activeItems = []
                }
            }
        }
        
        let gapExtraPixels = (this.items.length>1) ? extraPixels / (this.items.length - 1) : 0
        let freeSpace = containerSize - targetSize
        let offset = freeSpace * (0.5 + 0.5 * gravity) - gapExtraPixels        

        for (let i of this.items) {
            offset += gapExtraPixels
            i.primaryPosition += offset
            i.primarySize += i.extraPixels
            offset += i.extraPixels || 0
        }        
        return
    }    
}

/**
 * @param {number} gravity
 * @param {boolean} grow
 * */
LayoutLine.prototype.AlignSecondary = function (gravity, grow) {    
    this.secondaryMargin = 0
    this.secondaryMarginStop = 0 
    this.secondaryBodyStop = 0

    let secondaryLineSize = 0
    for (let i of this.items) {
        secondaryLineSize = Math.max(secondaryLineSize, i.secondarySize)
    }

    if (grow) {
        for (let i of this.items) {
            i.secondarySize = secondaryLineSize
        }
        //todo: (optimization) simplified secondary secondaryMargin/secondaryMarginStop/secondaryBodyStop calc
    } else {
        for (let i of this.items) {
            let dif = secondaryLineSize - i.secondarySize
            i.secondaryPosition = (0.5 + 0.5 * gravity) * dif 
        }
    }

    for (let i of this.items) {
        this.secondaryMargin = Math.max(this.secondaryMargin, i.secondaryMargin - i.secondaryPosition)
        let bottom = i.secondaryPosition + i.secondarySize
        this.secondaryMarginStop = Math.max(this.secondaryMarginStop, bottom)
        let secondaryBodyStop = bottom + i.secondaryMarginOpposite
        this.secondaryBodyStop = Math.max(this.secondaryBodyStop, secondaryBodyStop)
    }
}

/**
 * @constructor
 * @param {boolean} vertical
 * @param {Block} container
 */
function LayoutBlock(vertical, container) {

    /**@type {"Top" | "Left"}*/
    this.primarySide = vertical ? "Top" : "Left"

    /**@type {"Bottom" | "Right"}*/
    this.primarySideOpposite = vertical ? "Bottom" : "Right"

    /**@type {"Y" | "X"}*/
    this.primaryCoordinate = vertical ? "Y" : "X"

    /**@type {"Height" | "Width"}*/
    this.primaryDimension = vertical ? "Height" : "Width"

    /**@type {"Top" | "Left"}*/
    this.secondarySide = !vertical ? "Top" : "Left"

    /**@type {"Bottom" | "Right"}*/
    this.secondarySideOpposite = !vertical ? "Bottom" : "Right"

    /**@type {"Y" | "X"}*/
    this.secondaryCoordinate = !vertical ? "Y" : "X"

    /**@type {"Height" | "Width"}*/
    this.secondaryDimension = !vertical ? "Height" : "Width"

    /**@type {number}*/
    let primaryPadding = container["Padding" + this.primarySide]
    /**@type {number}*/
    let primaryPaddingOpposite = container["Padding" + this.primarySideOpposite]


    /**@type {number}*/
    this.primaryMarginStop = 0
    /**@type {number}*/
    this.primaryBodyStop = 0
    if (primaryPadding != undefined) {
        this.primaryBodyStop = primaryPadding
    } else {
        let margin = container["Margin" + this.primarySide]
        if (margin !== undefined)
            this.primaryMarginStop = -margin
    }
    /**@type {number}*/
    this.primaryMarginStopOpposite = 0
    /**@type {number}*/
    this.primaryBodyStopOpposite = 0
    if (primaryPaddingOpposite != undefined) {
        this.primaryBodyStopOpposite = primaryPaddingOpposite
    } else {
        let margin = container["Margin" + this.primarySideOpposite]
        if (margin !== undefined)
            this.primaryMarginStopOpposite = -margin
    }
    /**@type {number}*/
    let secondaryPadding = container["Padding" + this.secondarySide]
    /**@type {number}*/
    let secondaryPaddingOpposite = container["Padding" + this.secondarySideOpposite]

    /**@type {number}*/
    this.secondaryMarginStop = 0
    /**@type {number}*/
    this.secondaryBodyStop = 0
    if (secondaryPadding != undefined) {
        this.secondaryBodyStop = secondaryPadding
    } else {
        let margin = container["Margin" + this.secondarySide]
        if (margin !== undefined)
            this.secondaryMarginStop = -margin
    }

    this.secondaryMarginStopOpposite = 0
    this.secondaryBodyStopOpposite = 0
    if (secondaryPaddingOpposite != undefined) {
        this.secondaryBodyStopOpposite = secondaryPaddingOpposite
    } else {
        let margin = container["Margin" + this.secondarySideOpposite]
        if (margin !== undefined)
            this.secondaryMarginStopOpposite = -margin
    }

    /**@type {LayoutLine[]}*/
    this.lines = []
}

/**
 * @returns {LayoutLine}
 */
LayoutBlock.prototype.AddLine = function(){
    let result = new LayoutLine(this)
    this.lines.push(result)
    return result
}

/**
 * @returns {LayoutItem[]}
 */
LayoutBlock.prototype.ReadChildren = function (children) {
    return children.Select(x => new LayoutItem(x,this)).ToArray()
}

/**
 * @param {number} secondaryGap
 */
LayoutBlock.prototype.AlignLines = function (
    secondaryGap
    ) {

    for (let i = 0; i < this.lines.length; i++){
        let line = this.lines[i]
        
        if (i > 0)
            line.secondaryPosition = Math.max(
                this.secondaryBodyStop,
                this.secondaryMarginStop + Math.max(secondaryGap,line.secondaryMargin))
        else
            line.secondaryPosition = Math.max(this.secondaryBodyStop, this.secondaryMarginStop + line.secondaryMargin)


        this.secondaryBodyStop = line.secondaryPosition + line.secondaryBodyStop
        this.secondaryMarginStop = line.secondaryPosition + line.secondaryMarginStop
    }
}

/**
 * @param {number} extraPixels
 * @param {number} secondaryGapGrow
 */
LayoutBlock.prototype.GrowLines = function (
    extraPixels,
    secondaryGapGrow) {

    let numLines = this.lines.length

    let numGaps = numLines - 1
    let totalGrowUnits = numLines + numGaps * secondaryGapGrow

    let lineExtraPixels = extraPixels / totalGrowUnits
    let gapExtraPixels = lineExtraPixels * secondaryGapGrow

    let offset = -gapExtraPixels
    for (let line of this.lines) {

        offset += gapExtraPixels
        line.secondaryPosition += offset
        let secondaryLineSize = line.secondaryMarginStop + lineExtraPixels
        offset += lineExtraPixels

        for (let i of line.items) {
            i.secondarySize = secondaryLineSize
        }
    }
}


LayoutBlock.prototype.GetSecondarySize = function () {
    let size = Math.max(
        this.secondaryMarginStop + this.secondaryBodyStopOpposite,
        this.secondaryBodyStop +   this.secondaryMarginStopOpposite
    )
    return size
}

/**
 * @param {LayoutItem[]} children
 */
LayoutBlock.prototype.WriteChildren = function (children) {
    for (let i of children) {
        i.Write()
    }
}


function Layout(element) {
    Block(element)


    element.Reactive = {
        Vertical: false,
        PrimaryGap: 0,
        PrimaryGapGrow: 0,
        PrimaryGravity: -1,

        IntralinearGravity: -1,
        SecondaryGap: 0,
        SecondaryGapGrow: 0,
        FillSecondary: true,

        Multiline: false

    }


    new Reaction(() => {

        let layoutBlock = new LayoutBlock(element.Vertical, element)

        let children = layoutBlock.ReadChildren(element.Children)
        
        let primaryGap = element.PrimaryGap

        let line = layoutBlock.AddLine()
        for (let i of children) {
            line.AddChild(i, primaryGap)
        }
        let lineSize = line.GetLineSize()        
        element["Internal" + layoutBlock.primaryDimension] = lineSize

        let sizeLimit = element[layoutBlock.primaryDimension]

        if (lineSize > sizeLimit) {
            layoutBlock.lines = []
            line = undefined
            
            for (let i of children) {
                if (!line || !line.AddChild(i, primaryGap, sizeLimit)) {
                    line = layoutBlock.AddLine()
                    line.AddChild(i, primaryGap)
                }
            }
            element.Multiline = true
        } else {
            element.Multiline = false
        }

        for (let i of layoutBlock.lines) {
            i.AlignSecondary(element.IntralinearGravity, element.FillSecondary)
        }

        layoutBlock.AlignLines(element.SecondaryGap)

        let dependOnSecondarySize = element.FillSecondary || element.SecondaryGapGrow > 0

        let finalSecondatySizePrevious = 0
        if (dependOnSecondarySize) {
            //Read secondaryDimension
            finalSecondatySizePrevious = element[layoutBlock.secondaryDimension]
        }

        let secondarySize = layoutBlock.GetSecondarySize()
        element["Internal" + layoutBlock.secondaryDimension] = secondarySize

        if (dependOnSecondarySize) {
            //Read secondaryDimension            
            let finalSecondatySize = element[layoutBlock.secondaryDimension]
            if (finalSecondatySizePrevious != finalSecondatySize)
                return
            let extraPixels = finalSecondatySize - secondarySize
            //if (extraPixels > 0) {
            layoutBlock.GrowLines(extraPixels, element.SecondaryGapGrow)
            //}
        }
        
        for (let i of layoutBlock.lines) {
            i.AlignPrimary(sizeLimit, element.PrimaryGapGrow, element.PrimaryGravity)
        }

        layoutBlock.WriteChildren(children)

    })


}