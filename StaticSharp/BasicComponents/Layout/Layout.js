


/**
 * @constructor
 * @param {Block} element
 * @param {LayoutBlock} layoutBlock
 */
function LayoutItem(element,layoutBlock) {

    /**@type {LayoutBlock}*/
    this.layoutBlock = layoutBlock

    
    /**@type {number}*/ this.primaryMargin = element["Margin" + layoutBlock.primarySide] || 0
    /**@type {number}*/ this.primaryMarginOpposite = element["Margin" + layoutBlock.primarySideOpposite] || 0
    /**@type {number}*/ this.secondaryMargin = element["Margin" + layoutBlock.secondarySide] || 0
    /**@type {number}*/ this.secondaryMarginOpposite = element["Margin" + layoutBlock.secondarySideOpposite] || 0

    /**@type {Block}*/ this.element = element

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

    /*for (let item of line.items) {
        item.secondaryPosition = line.secondaryPosition
    }*/


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
    
    console.log("lineSize", containerSize, lineSize)
    
    if (lineSize > containerSize) {//Shrink
        let units = 0
        for (let i of this.items) {
            units += i.shrink
        }
        let pixelsPerUnit = (lineSize - containerSize) / units

        
        let offset = 0
        for (let i of this.items) {
            let delta = i.shrink * pixelsPerUnit
            
            i.primaryPosition -= offset

            i.primarySize -= delta
            offset += delta
            //secondaryLineSize = Math.max(secondaryLineSize, i.secondarySize)
        }

        

        return
    }

    
    if (lineSize < containerSize) {//Grow
        let maxLineSize = lineSize + this.growPixels
        
        
        

        let activeItems = this.items.filter(x => x.growPixels > 0)

        let units = (this.items.length - 1) * gapGrow

        let targetSize = 0
        if (units > 0)
            targetSize = containerSize
        else
            targetSize = Math.min(containerSize, maxLineSize)

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
        
        let gapExtraPixels = extraPixels / (this.items.length - 1)
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
 * @param {number} grow
 * */
LayoutLine.prototype.AlignSecondary = function (gravity = 0, grow = 0) {
    
    this.secondaryMargin = 0
    this.secondaryMarginStop = 0 
    this.secondaryBodyStop = 0


    let secondaryLineSize = 0
    for (let i of this.items) {
        secondaryLineSize = Math.max(secondaryLineSize, i.secondarySize)
    }


    for (let i of this.items) {
        let dif = secondaryLineSize - i.secondarySize
        i.secondarySize = i.secondarySize + grow * dif

        dif = secondaryLineSize - i.secondarySize
        i.secondaryPosition = (0.5 + 0.5 * gravity) * dif        
    }


    for (let i of this.items) {

        this.secondaryMargin = Math.max(this.secondaryMargin, i.secondaryMargin - i.secondaryPosition)

        let bottom = i.secondaryPosition + i.secondarySize

        this.secondaryMarginStop = Math.max(this.secondaryMarginStop, bottom)
        let secondaryBodyStop = bottom + i.secondaryMarginOpposite
        this.secondaryBodyStop = Math.max(this.secondaryBodyStop, secondaryBodyStop)

        //console.log("i.secondaryMarginOpposite", i.secondaryMarginOpposite)
    }
    //console.log("AlignSecondary",this.secondaryBodyStop, this.secondaryMarginStop)
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
 * @param {number} size
 * @param {number} secondaryGapGrow
 * @param {number} secondaryGrow
 */
LayoutBlock.prototype.AlignLines = function (
    secondaryGap,
    size = undefined,
    secondaryGapGrow = undefined,
    secondaryGrow = undefined) {

    for (let i = 0; i < this.lines.length; i++){
        let line = this.lines[i]
        line.secondaryPosition = Math.max(this.secondaryBodyStop, this.secondaryMarginStop + line.secondaryMargin)
        if (i > 0)
            line.secondaryPosition += secondaryGap
        
        this.secondaryBodyStop = line.secondaryPosition + line.secondaryBodyStop
        this.secondaryMarginStop = line.secondaryPosition + line.secondaryMarginStop
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
        PrimaryGap: 0,
        PrimaryGapGrow: 0,
        PrimaryGravity: -1,
        SecondaryGap: 10,
        SecondaryGapGrow: 1,
        SecondaryGrow: 1
    }

    


    new Reaction(() => {

        
        let layoutBlock = new LayoutBlock(false, element)

        let children = layoutBlock.ReadChildren(element.Children)
        
        let primaryGap = element.PrimaryGap

        let line = layoutBlock.AddLine()
        for (let i of children) {
            line.AddChild(i, primaryGap)
        }
        let lineSize = line.GetLineSize()        
        element["Internal" + layoutBlock.primaryDimension] = lineSize

        layoutBlock.lines = []
        line = undefined

        let sizeLimit = element[layoutBlock.primaryDimension]

        for (let i of children) {
            if (!line || !line.AddChild(i, primaryGap, sizeLimit)) {
                line = layoutBlock.AddLine()
                line.AddChild(i, primaryGap)
            }
        }

        for (let i of layoutBlock.lines) {
            i.AlignSecondary()
        }
        layoutBlock.AlignLines(element.SecondaryGap, element.SecondaryGapGrow, element.SecondaryGrow)

        for (let i of layoutBlock.lines) {
            i.AlignPrimary(sizeLimit, element.PrimaryGapGrow, element.PrimaryGravity)
        }

        layoutBlock.WriteChildren(children)

        element["Internal" + layoutBlock.secondaryDimension] = layoutBlock.GetSecondarySize()

        //console.log("layoutBlock.lines", layoutBlock.lines)
            


            /*for (let i of children) {
                let lineWithExtraElement = lineLayout.AddChild(i)
                if (lineWithExtraElement.numChildren > 1) {
                    if (lineWithExtraElement.GetContainerSize() >= maxContainerSize) {
                        break;
                    }
                }
                lineLayout = lineWithExtraElement
            }

            let numChildrenInLine = lineLayout.numChildren
            let childrenInLine = children.splice(0, numChildrenInLine)
            //console.log("childrenInLine", childrenInLine.length)

            lineLayout = primaryLayoutState.Clone()

            lineLayout.LayoutSequenceGrow(childrenInLine)

            secondaryLayoutState.LayoutParallel(childrenInLine)
            console.log(secondaryLayoutState)

            if (children.length == 0)
                break*/
        


        




        /*let result = {
        ...this
    }*/
        /*let primarySide = Sides[element.Side]
        let primaryLayoutDirection = new LayoutDirection(primarySide)
        let primaryLayoutState = new LayoutState(primaryLayoutDirection,element)

        let secondarySide = Sides.Top
        let secondaryLayoutDirection = new LayoutDirection(secondarySide)
        let secondaryLayoutState = new LayoutState(secondaryLayoutDirection, element)

        console.log("secondaryLayoutDirection", secondaryLayoutDirection)

        let children = element.Children.ToArray()

        let internalLayout = primaryLayoutState
        for (let i of children) {
            internalLayout = internalLayout.AddChild(i)
        }

        element[primaryLayoutDirection.internalDimensionName] = internalLayout.GetContainerSize()


        let maxContainerSize = element[primaryLayoutDirection.dimensionName]
        while (true) {
            let lineLayout = primaryLayoutState
            for (let i of children) {
                let lineWithExtraElement = lineLayout.AddChild(i)
                if (lineWithExtraElement.numChildren > 1) {
                    if (lineWithExtraElement.GetContainerSize() >= maxContainerSize) {
                        break;
                    }
                }
                lineLayout = lineWithExtraElement
            }

            let numChildrenInLine = lineLayout.numChildren
            let childrenInLine = children.splice(0, numChildrenInLine)
            //console.log("childrenInLine", childrenInLine.length)

            lineLayout = primaryLayoutState.Clone()

            lineLayout.LayoutSequenceGrow(childrenInLine)

            secondaryLayoutState.LayoutParallel(childrenInLine)
            console.log(secondaryLayoutState)

            if (children.length == 0)
                break
        }*/


        /*primaryLayoutDirection.layoutDimensionName
        let mainDimensionName = mainSide.GetDimensionName()
        let mainCoordinateName = mainSide.GetCoordinateName()

        
        let mainLayout = new LayoutHelper(mainSide, element)
        let descriptions = mainLayout.Read(children)

        let internalLayout = Object.create(mainLayout)

        

        internalLayout.AddChildren(descriptions, Infinity)

        element["Internal" + mainDimensionName] = internalLayout.GetContainerSize()


        let maxSize = element[mainDimensionName]
        while (true) {
            let lineLayout = Object.create(mainLayout)
            let numChildrenInLine = lineLayout.AddChildren(descriptions, maxSize)
            console.log("numChildrenInLine", numChildrenInLine)
            let childrenInLine = descriptions.shift(numChildrenInLine)



            console.log(descriptions,numChildrenInLine,childrenInLine)
            if (descriptions.length == 0)
                break
        }*/
        

        /*for (let i of element.Children) {
            let position = helper.AddChild(i)
            i["Layout" + helper.startSide.GetCoordinateName()] = position
        }

        let size = helper.GetContainerSize()*/


        


    })


}