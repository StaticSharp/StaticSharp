function Column(element) {
    Block(element)

    element.Reactive = {

        InternalWidth: () => {
            let internalWidth = Sum(element.PaddingLeft,element.PaddingRight)
            for (let child of element.Children) {
                let left = CalcOffset(element, child, "Left")
                let right = CalcOffset(element, child, "Right")

                //let spaceLeft = Max(element.PaddingLeft, child.MarginLeft, 0)
                //let spaceRight = Max(element.PaddingRight, child.MarginRight, 0)

                let internalWidthByCurrentChild = Sum(/*child.InternalWidth*/child.Layer.Width, left + right)

                internalWidth = Max(internalWidth, internalWidthByCurrentChild)
            }
            //console.log("internalWidth", internalWidth)
            return internalWidth
        },

        //InternalHeight: undefined,
        //Height: () => element.InternalHeight,
    }


    new Reaction(() => {
        for (let child of element.Children) {
            if (child.isBlock) {

                child./*LayoutX*/Layer.X = CalcOffset(element, child, "Left")               

                let left = CalcOffset(element, child, "Left")
                let right = CalcOffset(element, child, "Right")

                //child.LayoutWidth = element.Width - left - right                
                child.Layer.Width = element.Width - left - right                
            }
        }
    })

    new Reaction(() => {
        let previousMargin = undefined
        let freeSpaceUnits = 0
        let freeSpacePixels
        let contentHeight = 0

        let isFirstBlock = true
        let lastChild = undefined

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

                lastChild = child

                if (isFirstBlock) {
                    let offset = CalcOffset(element, child, "Top")
                    //console.log(element)
                    //console.log(contentHeight, offset)
                    contentHeight += offset
                } else {
                    let offset = Max(child.MarginTop, previousMargin)
                    contentHeight += offset
                }
                isFirstBlock = false

                //console.log("child.Height", Max(child.Height, 0), child)

                /*let margin = (previousMargin == undefined)
                    ? CalcOffset()
                    : Max(child.MarginTop, previousMargin)
                contentHeight += margin*/


                if (assignDimensions) {
                    //console.log("child.LayoutY", contentHeight)
                    //child.LayoutY = contentHeight
                    child.Layer.Y = contentHeight
                }
                contentHeight += Max(child.Height, 0)

                previousMargin = child.MarginBottom || 0

                return true
            }
            return true
        }


        

        for (let i of element.Children) {
            if (!addElement(i, false)) {
                return
            }
        }


        if (!lastChild) {
            element.InternalHeight = Sum(element.PaddingTop, element.PaddingBottom)
            return
        }

        let bottomOffset = CalcOffset(element, lastChild, "Bottom")
        contentHeight += bottomOffset

        previousMargin = element.PaddingTop || 0
        element.InternalHeight = contentHeight;
        if (!element.Height)
            return

        freeSpacePixels = element.Height - contentHeight;// Math.max( element.Height - contentHeight, 0)
        //console.log(element.Height, contentHeight, element)
        /*if (freeSpacePixels < 0) {//overflow-y: overlay;
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
        }*/

        //console.log("freeSpacePixels", freeSpacePixels)
        isFirstBlock = true
        contentHeight = 0

        for (let i of element.Children) {
            if (!addElement(i, true)) {
                return
            }
        }
    })


}
