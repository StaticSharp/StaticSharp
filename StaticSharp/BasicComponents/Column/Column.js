function Column(element) {
    Block(element)

    element.Reactive = {
        InternalWidth: () => {

            let internalWidth = undefined
            for (let child of element.Children) {
                let left = CalcOffset(element, child, "Left")
                let right = CalcOffset(element, child, "Right")

                //let spaceLeft = Max(element.PaddingLeft, child.MarginLeft, 0)
                //let spaceRight = Max(element.PaddingRight, child.MarginRight, 0)

                let internalWidthByCurrentChild = Sum(child.InternalWidth, left + right)

                internalWidth = Max(internalWidth, internalWidthByCurrentChild)
            }
            return internalWidth
        },

        //InternalHeight: undefined,
        //Height: () => element.InternalHeight,
    }


    new Reaction(() => {
        for (let child of element.Children) {
            if (child.isBlock) {

                child.LayoutX = () => CalcOffset(element, child, "Left")               

                child.LayoutWidth = () => {
                    let left = CalcOffset(element, child, "Left")
                    let right = CalcOffset(element, child, "Right")
                    return element.Width - left - right
                }
            }
        }
    })

    new Reaction(() => {
        let previousMargin
        let freeSpaceUnits
        let freeSpacePixels
        let contentHeight

        function addElement(child, assignDimensions) {
            if (child.isSpace) {
                if (assignDimensions) {
                    contentHeight += freeSpacePixels / freeSpaceUnits * child.Between
                } else {
                    freeSpaceUnits += child.Between
                }
                return true;
            }

            if (child.isBlock) {
                /*if (!child.Height) {
                    console.log("Column !child.Height", child)
                    return false

                }*/

                //console.log("child.Height", Max(child.Height, 0), child)


                let margin = Max(child.MarginTop, previousMargin)
                previousMargin = child.MarginBottom || 0


                contentHeight += margin
                if (assignDimensions) {
                    child.LayoutY = contentHeight
                }
                contentHeight += Max(child.Height, 0)


                return true
            }
            return true
        }



        previousMargin = element.PaddingTop || 0
        freeSpaceUnits = 0
        contentHeight = 0


        for (let i of element.Children) {
            if (!addElement(i, false)) {
                return
            }
        }

        //console.log("ColumnAfter Reaction", element, element.LayoutChildren)

        //Here "previousMargin" contains last-child.MarginBottom
        contentHeight += Max(previousMargin, element.PaddingBottom)
        //console.log("Vertical layout", element, contentHeight)

        previousMargin = element.PaddingTop || 0
        element.InternalHeight = contentHeight;
        if (!element.Height)
            return

        freeSpacePixels = element.Height - contentHeight;// Math.max( element.Height - contentHeight, 0)

        if (freeSpacePixels < 0) {//overflow-y: overlay;
            element.style.overflowY = "auto"
            freeSpacePixels = 0
            if (!element.innerSizeHolder) {
                element.innerSizeHolder = document.createElement('holder')
                element.innerSizeHolder.style.position = "absolute"
                element.appendChild(element.innerSizeHolder)
                element.innerSizeHolder.style.width = "1px"
            }
            element.innerSizeHolder.style.height = contentHeight + "px"
        } else {
            element.style.overflowY = ""
            if (element.innerSizeHolder) {
                element.removeChild(element.innerSizeHolder)
                element.innerSizeHolder = undefined
            }
        }

        //console.log("freeSpacePixels", freeSpacePixels)

        contentHeight = 0

        for (let i of element.Children) {
            if (!addElement(i, true)) {
                return
            }
        }
    })

    WidthToStyle(element)
    HeightToStyle(element)

}
